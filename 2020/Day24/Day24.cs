using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day24
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsStrings(Utils.GetInputPath(2020, 24));

            Console.WriteLine(GetFlippedTiles(input));

            Console.WriteLine(LivingExhibit(input));
        }

        private static List<(int, int)> ParseTiles(string[] input)
        {
            List<(int, int)> res = new List<(int, int)>();

            foreach (string line in input)
            {
                var coordinate = ToCoordinates(line);
                if (res.Contains(coordinate))
                    res.Remove(coordinate);
                else
                    res.Add(coordinate);
            }

            return res;
        }

        private static int GetFlippedTiles(string[] input)
        {
            List<(int, int)> flipped = ParseTiles(input);

            return flipped.Count;
        }

        private static int LivingExhibit(string[] input)
        {
            List<(int, int)> flipped = ParseTiles(input);

            HexagonalGrid grid = new HexagonalGrid(flipped);

            foreach (int i in Enumerable.Range(0, 100))
            {
                grid.UpdateGridAdjacent(UpdateFunction);
            }
            return grid.CountValues();
        }

        private static bool UpdateFunction(bool current, bool[] adjacent)
        {
            if (current)
            {
                if (adjacent.Count(x => x) == 0 || adjacent.Count(x => x) > 2)
                    return false;
                return current;
            }
            else
            {
                if (adjacent.Count(x => x) == 2)
                    return true;
                return current;
            }
        }

        public static (int, int) ToCoordinates(string line)
        {
            (int, int) res = (0, 0);
            while (!string.IsNullOrEmpty(line))
            {
                if (line.StartsWith("e"))
                {
                    res.Item1 = res.Item1 + 2;
                    line = line.Substring(1);
                }
                else if (line.StartsWith("w"))
                {
                    res.Item1 = res.Item1 - 2;
                    line = line.Substring(1);
                }
                else if (line.StartsWith("se"))
                {
                    res.Item1 = res.Item1 + 1;
                    res.Item2 = res.Item2 - 1;
                    line = line.Substring(2);
                }
                else if (line.StartsWith("sw"))
                {
                    res.Item1 = res.Item1 - 1;
                    res.Item2 = res.Item2 - 1;
                    line = line.Substring(2);
                }
                else if (line.StartsWith("nw"))
                {
                    res.Item1 = res.Item1 - 1;
                    res.Item2 = res.Item2 + 1;
                    line = line.Substring(2);
                }
                else if (line.StartsWith("ne"))
                {
                    res.Item1 = res.Item1 + 1;
                    res.Item2 = res.Item2 + 1;
                    line = line.Substring(2);
                }
            }
            return res;
        }
    }

    public class HexagonalGrid
    {
        Dictionary<(int, int), bool> internalGrid = new Dictionary<(int, int), bool>();
        int xMax, yMax, xMin, yMin;

        public Dictionary<(int, int), bool> GridValues { get => internalGrid; }

        public HexagonalGrid(List<(int, int)> input)
        {
            foreach (var inp in input)
            {
                internalGrid.Add(inp, true);
                UpdateBounds(inp);
            }
        }

        private void UpdateBounds((int, int) input)
        {
            if (input.Item1 > xMax)
                xMax = input.Item1;
            else if (input.Item1 < xMin)
                xMin = input.Item1;
            if (input.Item2 > yMax)
                yMax = input.Item2;
            else if (input.Item2 < yMin)
                yMin = input.Item2;
        }

        public HexagonalGrid(HexagonalGrid otherGrid)
        {
            internalGrid = new Dictionary<(int, int), bool>();
            foreach (var element in otherGrid.GridValues)
                internalGrid.Add(element.Key, element.Value);

            xMax = otherGrid.xMax;
            yMax = otherGrid.yMax;
            xMin = otherGrid.xMin;
            yMin = otherGrid.yMin;
        }

        private void ExpandGrid()
        {
            HexagonalGrid oldValues = this.Clone();
            foreach (var value in oldValues.internalGrid)
            {
                int x = value.Key.Item1;
                int y = value.Key.Item2;

                if (!internalGrid.ContainsKey((x + 1, y + 1)))
                    internalGrid.Add((x + 1, y + 1), false);
                if (!internalGrid.ContainsKey((x + 2, y)))
                    internalGrid.Add((x + 2, y), false);
                if (!internalGrid.ContainsKey((x + 1, y - 1)))
                    internalGrid.Add((x + 1, y - 1), false);
                if (!internalGrid.ContainsKey((x - 1, y - 1)))
                    internalGrid.Add((x - 1, y - 1), false);
                if (!internalGrid.ContainsKey((x - 2, y)))
                    internalGrid.Add((x - 2, y), false);
                if (!internalGrid.ContainsKey((x - 1, y + 1)))
                    internalGrid.Add((x - 1, y + 1), false);
            }
        }

        public bool[] GetAdjacentValues(int x, int y)
        {
            List<bool> res = new List<bool>();

            if (internalGrid.ContainsKey((x + 1, y + 1)))
                res.Add(internalGrid[(x + 1, y + 1)]);
            if (internalGrid.ContainsKey((x + 2, y)))
                res.Add(internalGrid[(x + 2, y)]);
            if (internalGrid.ContainsKey((x + 1, y - 1)))
                res.Add(internalGrid[(x + 1, y - 1)]);
            if (internalGrid.ContainsKey((x - 1, y - 1)))
                res.Add(internalGrid[(x - 1, y - 1)]);
            if (internalGrid.ContainsKey((x - 2, y)))
                res.Add(internalGrid[(x - 2, y)]);
            if (internalGrid.ContainsKey((x - 1, y + 1)))
                res.Add(internalGrid[(x - 1, y + 1)]);

            while (res.Count < 6)
                res.Add(false);

            return res.ToArray();
        }

        public void UpdateGridAdjacent(Func<bool, bool[], bool> updateFunc)
        {
            ExpandGrid();
            HexagonalGrid oldValues = this.Clone();
            foreach (var value in oldValues.internalGrid)
                internalGrid[value.Key] = updateFunc(value.Value, oldValues.GetAdjacentValues(value.Key.Item1, value.Key.Item2));
        }

        public int CountValues()
        {
            int res = 0;

            foreach (var value in internalGrid)
                if (value.Value)
                    res++;

            return res;
        }

        public HexagonalGrid Clone()
        {
            return new HexagonalGrid(this);
        }

        public void Clone(HexagonalGrid destination)
        {
            destination.internalGrid = new Dictionary<(int, int), bool>();

            foreach (var value in internalGrid)
                destination.internalGrid.Add(value.Key, value.Value);

            destination.xMax = this.xMax;
            destination.xMin = this.xMin;
            destination.yMax = this.yMax;
            destination.yMin = this.yMin;
        }
    }
}
