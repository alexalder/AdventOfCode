using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day17
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsStrings(Utils.GetInputPath(2020, 17));

            Console.WriteLine(ResolveDimensionsCycles(input, 3, 6));

            Console.WriteLine(ResolveDimensionsCycles(input, 4, 6));
        }

        private static int ResolveDimensionsCycles(string[] input, int dimensions, int cycles)
        {
            var grid = NDimensionalList.From2DInput(input, dimensions);

            foreach (int addY in Enumerable.Range(0, cycles))
            {
                grid.UpdateGridAdjacent(Evolve);
            }
            return grid.CountValues('#');
        }

        private static char Evolve(char value, char[] adjacents)
        {
            if (value == '#')
            {
                if ((adjacents.Count((x) => x == '#')) == 2 || (adjacents.Count((x) => x == '#')) == 3)
                    return '#';
                else
                    return '.';

            }
            else if (value == '.')
            {
                if ((adjacents.Count((x) => x == '#')) == 3)
                    return '#';
                else
                    return '.';
            }
            return value;
        }
    }

    public class NDimensionalList
    {
        Dictionary<int[], char> values = new Dictionary<int[], char>(new MyEqualityComparer());
        int dimensions;

        public NDimensionalList() { }

        public NDimensionalList(NDimensionalList otherGrid)
        {
            foreach (var newValue in otherGrid.values)
                values.Add(newValue.Key, newValue.Value);
        }

        public static NDimensionalList From2DInput(string[] input, int dimensions)
        {
            NDimensionalList res = new NDimensionalList();
            res.dimensions = dimensions;

            for (int x = 0; x < input[0].Length; x++)
            {
                for (int y = 0; y < input.Length; y++)
                {
                    int[] coordinates = new int[dimensions];
                    Array.Clear(coordinates, 0, coordinates.Length);
                    coordinates[0] = x;
                    coordinates[1] = y;
                    res.values.Add(coordinates, input[y][x]);
                }
            }

            return res;
        }

        public char[] GetAdjacentValues(int[] address)
        {
            List<char> res = new List<char>();
            MyEqualityComparer comparer = new MyEqualityComparer();

            foreach (var nearby in Utils.PermutationsRepetitions(-1, 1, address.Length))
            {
                int[] nearbyAddress = address.Zip(nearby, (a, b) => a + b).ToArray();
                if (comparer.Equals(address, nearbyAddress))
                    continue;
                if (values.ContainsKey(nearbyAddress))
                    res.Add(values[nearbyAddress]);
            }

            return res.ToArray();
        }

        public void UpdateGridAdjacent(Func<char, char[], char> updateFunc, char startingValue = '.')
        {
            NDimensionalList oldValues = EnlargeList(this, startingValue);
            foreach (var value in oldValues.values)
                values[value.Key] = updateFunc(value.Value, oldValues.GetAdjacentValues(value.Key));
        }

        public NDimensionalList EnlargeList(NDimensionalList start, char startingValue)
        {
            NDimensionalList res = new NDimensionalList(start);

            foreach (var nearby in Utils.PermutationsRepetitions(-1, 1, start.dimensions))
            {
                foreach (var value in start.values)
                {
                    int[] address = value.Key;
                    int[] nearbyAddress = address.Zip(nearby, (a, b) => a + b).ToArray();
                    if (!start.values.ContainsKey(nearbyAddress))
                        if (!res.values.ContainsKey(nearbyAddress))
                            res.values.Add(nearbyAddress, startingValue);
                }
            }
            return res;
        }

        public int CountValues(char target)
        {
            int res = 0;
            foreach (var value in values)
                if (value.Value == target)
                    res++;

            return res;
        }

        public NDimensionalList Clone()
        {
            return new NDimensionalList(this);
        }

        public void Print()
        {
            if (dimensions == 3)
                foreach (var value in values)
                    Console.WriteLine(value.Key[0].ToString() + value.Key[1].ToString() + value.Key[2].ToString() + " = " + value.Value);
        }
    }

    public class MyEqualityComparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[] x, int[] y)
        {
            if (x.Length != y.Length)
            {
                return false;
            }
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(int[] obj)
        {
            int result = 17;
            for (int i = 0; i < obj.Length; i++)
            {
                unchecked
                {
                    result = result * 23 + obj[i];
                }
            }
            return result;
        }
    }
}