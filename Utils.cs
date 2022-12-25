using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AdventOfCodeUtils
{
    public static class Utils
    {
        public static int GetDay()
        {
            StackTrace stackTrace = new();
            string day = "Utils";
            int i = 1;
            while (day.Contains("Utils"))
            {
                MethodBase methodBase = stackTrace.GetFrame(i)!.GetMethod()!;
                day = methodBase.DeclaringType!.Name;
                i++;
            }
            day = day.Replace("Day", "");
            return int.Parse(day);
        }

        public static int GetYear()
        {
            StackTrace stackTrace = new();
            string year = "Utils";
            int i = 1;
            while (year.Contains("Utils"))
            {
                MethodBase methodBase = stackTrace.GetFrame(i)!.GetMethod()!;
                Type typo = methodBase.DeclaringType!;
                year = typo.FullName!;
                i++;
            }
            year = year.Substring(12, 4);
            return int.Parse(year);
        }

        public static string[] ReadInputAsStrings(string path)
        {
            return File.ReadAllLines(path);
        }

        public static int[] ReadInputAsInts(string path)
        {
            var strings = File.ReadAllLines(path);
            return strings.Select(x => int.Parse(x)).ToArray<int>();
        }

        public static double[] ReadInputAsDoubles(string path)
        {
            var strings = File.ReadAllLines(path);
            return strings.Select(x => double.Parse(x)).ToArray<double>();
        }

        public static int[] GetDigits(int input)
        {
            string read = input.ToString();
            int[] res = read.ToString().Select(t => int.Parse(t.ToString())).ToArray();
            return res;
        }

        public static int[] GetDigits(long input)
        {
            string read = input.ToString();
            int[] res = read.ToString().Select(t => int.Parse(t.ToString())).ToArray();
            return res;
        }

        public static int[] GetDigits(string input)
        {
            return input.Select(t => int.Parse(t.ToString())).ToArray();
        }

        public static string ReverseString(string input)
        {
            var charArr = input.ToCharArray();
            charArr = charArr.Reverse().ToArray();
            return new string(charArr);
        }

        public static string SortString(string input)
        {
            char[] chars = input.ToCharArray();
            Array.Sort(chars);
            return new string(chars);
        }

        public static int DivideIntegersRoundUp(int a, int b, out int remainder)
        {
            int result = (a + b - 1) / b;
            remainder = b * result - a;
            return result;
        }

        public static double DivideDoubleRoundUp(double a, double b, out double remainder)
        {
            double result = Math.Ceiling(a / b);
            remainder = b * result - a;
            return result;
        }

        public static double BinaryToDouble(string a)
        {
            return Convert.ToInt64(a, 2);
        }

        public static int BinaryToDecimal(string a)
        {
            return Convert.ToInt32(a, 2);
        }

        public static string DecimalToBinary(int a, int padding = 0)
        {
            return Convert.ToString(a, 2).PadLeft(padding, '0');
        }

        public static string HexToBinary(string a)
        {
            return Convert.ToString(Convert.ToInt32(a, 16), 2);
        }

        public static string GetFolder(int year, int day)
        {
            string[] separators = { "bin" };
            return Path.Combine(Directory.GetCurrentDirectory().Split(separators, StringSplitOptions.RemoveEmptyEntries)[0], year.ToString(), "Day" + day.ToString("D2"));
        }

        public static string GetInputPath()
        {
            return Path.Combine(GetFolder(GetYear(), GetDay()), "input.txt");
        }

        public static string GetInputPath(int year, int day)
        {
            return Path.Combine(GetFolder(year, day), "input.txt");
        }

        public static string GetTestPath(int test = 0)
        {
            return Path.Combine(GetFolder(GetYear(), GetDay()), "testInput" + test + ".txt");

        }

        public static string GetTestPath(int year, int day, int test = 0)
        {
            return Path.Combine(GetFolder(year, day), "testInput" + test + ".txt");

        }

        public static string[][] SplitInput(string[] input, string separator = "")
        {
            List<string[]> res = new();
            List<string> curList = new();

            for (int i = 0; i < input.Length; i++)
            {
                var line = input[i];
                if (line != separator)
                    curList.Add(line);
                if (line == separator || i == input.Length - 1)
                {
                    res.Add(curList.ToArray());
                    curList = new List<string>();
                }
            }

            return res.ToArray();
        }

        public static double Factorial(double numb)
        {
            if (numb <= 1) return 1;
            double final = 1;
            for (double i = 1; i <= numb; i++)
            {
                final *= i;
            }
            return final;
        }

        public static double FindLeastCommonMultiple(params int[] numbers)
        {
            Dictionary<int, int> factors = new();

            foreach (int number in numbers)
            {
                foreach (var item in FindPrimeFactorsDictionary(number))
                {
                    if (factors.ContainsKey(item.Key))
                    {
                        if (factors[item.Key] < item.Value)
                        {
                            factors[item.Key] = item.Value;
                        }

                    }
                    else
                        factors.Add(item.Key, item.Value);
                }
            }

            double res = 1;

            foreach (var item in factors)
            {
                for (int i = 0; i < item.Value; i++)
                    res *= item.Key;
            }

            return res;
        }

        public static Dictionary<int, int> FindPrimeFactorsDictionary(int number)
        {
            Dictionary<int, int> factors = new();

            int factor = 2;

            while (number > 1)
            {
                if (!(factor > 2 && factor % 2 == 0))
                {

                    if (number % factor == 0)
                    {
                        if (factors.ContainsKey(factor))
                            factors[factor] = factors[factor] + 1;
                        else
                            factors.Add(factor, 1);
                        number /= factor;
                    }
                    else
                    {
                        factor++;
                    }
                }
                else
                {
                    factor++;
                }

            }

            return factors;
        }

        public static int[] FindPrimeFactors(int number)
        {
            List<int> factors = new();

            foreach (var item in FindPrimeFactorsDictionary(number))
            {
                for (int i = 0; i < item.Value; i++)
                    factors.Add(item.Key);
            }

            return factors.ToArray();
        }

        public static IEnumerable<int[]> PermutationsRepetitions(int min, int max, int dimensions)
        {
            int[] cur = Enumerable.Repeat(min, dimensions).ToArray();

            yield return cur;

            for (double i = 1; i < Math.Pow((max - min + 1), dimensions); i++)
            {
                cur[^1]++;
                for (int j = cur.Length - 1; j >= 0; j--)
                {
                    if (cur[j] > max)
                    {
                        cur[j] = min;
                        cur[j - 1]++;
                    }
                }
                yield return cur;
            }
        }

        public static void DrawPoints(List<(int x, int y)> points)
        {
            int xMax = points.Max(x => x.x) + 1;
            int yMax = points.Max(x => x.y) + 1;

            char[,] res = new char[xMax, yMax];

            for (int x = 0; x < xMax; x++)
                for (int y = 0; y < yMax; y++)
                    res[x, y] = ' ';

            foreach (var (x, y) in points)
            {
                res[x, y] = 'O';
            }

            for (int y = 0; y < yMax; y++)
            {
                string print = "";
                for (int x = 0; x < xMax; x++)
                    print += res[x, y];
                Console.WriteLine(print);
            }
        }

        public static long BisectionMethod(long min, double minRes, long max, double maxRes, Func<long, double> function, long zero)
        {
            long mid = (max - min) / 2 + min;
            double midRes = function(mid);

            if (midRes == zero)
                return mid;
            else if (minRes < zero && midRes > zero)
                return BisectionMethod(min, minRes, mid, midRes, function, zero);
            else if (midRes < zero && maxRes > zero)
                return BisectionMethod(mid, midRes, max, maxRes, function, zero);
            else if (maxRes < zero && midRes > zero)
                return BisectionMethod(mid, midRes, max, maxRes, function, zero);
            else if (midRes < zero && minRes > zero)
                return BisectionMethod(min, minRes, mid, midRes, function, zero);

            throw new Exception();
        }
    }

    public class TreeNode<T>
    {
        public T value;
        public TreeNode<T> parent;
        public List<TreeNode<T>> children = new();
        public int depth;

        public TreeNode(T value)
        {
            this.value = value;
            this.depth = 0;
            this.parent = null;
        }

        public TreeNode(T value, int depth)
        {
            this.value = value;
            this.depth = depth;
            this.parent = null;
        }

        public TreeNode<T> AddNode(T value)
        {
            TreeNode<T> newNode = new(value, depth + 1);
            children.Add(newNode);
            newNode.parent = this;
            return newNode;
        }

        public TreeNode<T> AddTree(TreeNode<T> node)
        {
            TreeNode<T> newNode = node;
            newNode.Traverse((node) => node.depth++);
            children.Add(newNode);
            newNode.parent = this;
            return newNode;
        }

        public void Traverse(Action<TreeNode<T>> action)
        {
            action(this);
            foreach (TreeNode<T> node in children)
                node.Traverse(action);
        }

        public TreeNode<T> Clone()
        {
            TreeNode<T> newNode = new(value.DeepClone(), depth);
            foreach (TreeNode<T> node in children)
            {
                var newChild = node.Clone();
                newNode.children.Add(newChild);
                newChild.parent = newNode;
            }
            return newNode;
        }
    }

    public class Grid<T> : IEnumerable<(int x, int y, T value)>
    {
        private T[,] internalGrid;
        public readonly int xMax;
        public readonly int yMax;

        public T[,] GridValues { get => internalGrid; }
        public int Count { get => xMax * yMax; }

        public Grid(string[] input, Func<char, T> conversionFunction)
        {
            internalGrid = new T[input[0].Length, input.Length];
            for (int x = 0; x < input[0].Length; x++)
                for (int y = 0; y < input.Length; y++)
                    internalGrid[x, y] = conversionFunction(input[y][x]);

            xMax = input[0].Length;
            yMax = input.Length;
        }

        public Grid(int xMax, int yMax, T startValue)
        {
            internalGrid = new T[xMax, yMax];
            for (int x = 0; x < xMax; x++)
                for (int y = 0; y < yMax; y++)
                    internalGrid[x, y] = startValue;

            this.xMax = xMax;
            this.yMax = yMax;
        }

        public Grid(Grid<T> otherGrid)
        {
            internalGrid = new T[otherGrid.xMax, otherGrid.yMax];
            for (int x = 0; x < otherGrid.xMax; x++)
                for (int y = 0; y < otherGrid.yMax; y++)
                    internalGrid[x, y] = otherGrid.GridValues[x, y];

            xMax = otherGrid.xMax;
            yMax = otherGrid.yMax;
        }

        public T[] GetAdjacentValues(int x, int y)
        {
            List<T> res = new();
            if (x > 0 && y > 0)
                res.Add(internalGrid[x - 1, y - 1]);
            if (y > 0)
                res.Add(internalGrid[x, y - 1]);
            if (x < xMax - 1 && y > 0)
                res.Add(internalGrid[x + 1, y - 1]);

            if (x > 0)
                res.Add(internalGrid[x - 1, y]);

            if (x < xMax - 1)
                res.Add(internalGrid[x + 1, y]);

            if (x > 0 && y < yMax - 1)
                res.Add(internalGrid[x - 1, y + 1]);
            if (y < yMax - 1)
                res.Add(internalGrid[x, y + 1]);
            if (x < xMax - 1 && y < yMax - 1)
                res.Add(internalGrid[x + 1, y + 1]);

            return res.ToArray();
        }

        public T[] GetAdjacentValues(int x, int y, T defaulto)
        {
            List<T> res = new();
            if (x > 0 && y > 0)
                res.Add(internalGrid[x - 1, y - 1]);
            else
                res.Add(defaulto);
            if (y > 0)
                res.Add(internalGrid[x, y - 1]);
            else
                res.Add(defaulto);
            if (x < xMax - 1 && y > 0)
                res.Add(internalGrid[x + 1, y - 1]);
            else
                res.Add(defaulto);

            if (x > 0)
                res.Add(internalGrid[x - 1, y]);
            else
                res.Add(defaulto);

            if (x < xMax - 1)
                res.Add(internalGrid[x + 1, y]);
            else
                res.Add(defaulto);

            if (x > 0 && y < yMax - 1)
                res.Add(internalGrid[x - 1, y + 1]);
            else
                res.Add(defaulto);
            if (y < yMax - 1)
                res.Add(internalGrid[x, y + 1]);
            else
                res.Add(defaulto);
            if (x < xMax - 1 && y < yMax - 1)
                res.Add(internalGrid[x + 1, y + 1]);
            else
                res.Add(defaulto);

            return res.ToArray();
        }

        //public (int x, int y, T value)[] GetAdjacentPoints(int x, int y)
        //{
        //    List<(int x, int y, T value)> res = new();
        //    if (x < xMax - 1)
        //        res.Add((x + 1, y, internalGrid[x + 1, y]));
        //    if (y < yMax - 1)
        //        res.Add((x, y + 1, internalGrid[x, y + 1]));
        //    if (x > 0)
        //        res.Add((x - 1, y, internalGrid[x - 1, y]));
        //    if (y > 0)
        //        res.Add((x, y - 1, internalGrid[x, y - 1]));
        //    if (x > 0 && y > 0)
        //        res.Add((x - 1, y - 1, internalGrid[x - 1, y - 1]));
        //    if (x > 0 && y < yMax - 1)
        //        res.Add((x - 1, y + 1, internalGrid[x - 1, y + 1]));
        //    if (x < xMax - 1 && y > 0)
        //        res.Add((x + 1, y - 1, internalGrid[x + 1, y - 1]));
        //    if (x < xMax - 1 && y < yMax - 1)
        //        res.Add((x + 1, y + 1, internalGrid[x + 1, y + 1]));
        //    return res.ToArray();
        //}

        public (int x, int y, T value)[] GetAdjacentPoints(int x, int y, bool horizontalVerticalOnly = false)
        {
            List<(int x, int y, T value)> res = new();
            if (x < xMax - 1)
                res.Add((x + 1, y, internalGrid[x + 1, y]));
            if (y < yMax - 1)
                res.Add((x, y + 1, internalGrid[x, y + 1]));
            if (x > 0)
                res.Add((x - 1, y, internalGrid[x - 1, y]));
            if (y > 0)
                res.Add((x, y - 1, internalGrid[x, y - 1]));

            if (!horizontalVerticalOnly)
            {
                if (x > 0 && y > 0)
                    res.Add((x - 1, y - 1, internalGrid[x - 1, y - 1]));
                if (x > 0 && y < yMax - 1)
                    res.Add((x - 1, y + 1, internalGrid[x - 1, y + 1]));
                if (x < xMax - 1 && y > 0)
                    res.Add((x + 1, y - 1, internalGrid[x + 1, y - 1]));
                if (x < xMax - 1 && y < yMax - 1)
                    res.Add((x + 1, y + 1, internalGrid[x + 1, y + 1]));
            }
            return res.ToArray();
        }

        public (int x, int y, T value)[] GetAdjacentPoints((int x, int y) point, bool horizontalVerticalOnly = false)
        {
            return GetAdjacentPoints(point.x, point.y, horizontalVerticalOnly);
        }

        IEnumerator<(int x, int y, T value)> IEnumerable<(int x, int y, T value)>.GetEnumerator()
        {
            for (int y = 0; y < yMax; y++)
                for (int x = 0; x < xMax; x++)
                    yield return ((x, y, internalGrid[x, y]));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int y = 0; y < yMax; y++)
                for (int x = 0; x < xMax; x++)
                    yield return ((x, y, internalGrid[x, y]));
        }

        public void UpdateGridViaAdjacent(Func<T, T[], T> updateFunc)
        {
            Grid<T> oldValues = this.Clone();
            for (int x = 0; x < xMax; x++)
                for (int y = 0; y < yMax; y++)
                    internalGrid[x, y] = updateFunc(oldValues.GridValues[x, y], oldValues.GetAdjacentValues(x, y));
        }

        public Grid<T> ResizeCanvas(int increase, T defaulto)
        {
            Grid<T> res = new Grid<T>(xMax + increase * 2, yMax + increase * 2, defaulto);
            foreach (var point in this)
            {
                res.internalGrid[point.x + increase, point.y + increase] = point.value;
            }
            return res;
        }

        public bool UpdateGrid(Func<int, int, Grid<T>, T> updateFunc)
        {
            Grid<T> oldGrid = this.Clone();
            bool changed = false;
            for (int x = 0; x < xMax; x++)
            {
                for (int y = 0; y < yMax; y++)
                {
                    T newValue = updateFunc(x, y, oldGrid);
                    if (!internalGrid[x, y]!.Equals(newValue))
                    {
                        internalGrid[x, y] = newValue;
                        changed = true;
                    }
                }
            }
            return changed;
        }

        public bool UpdateGrid(Func<T, T> updateFunc)
        {
            return UpdateGrid((x, y, grid) => updateFunc(grid.internalGrid[x, y]));
        }

        public void UpdatePoint(int x, int y, T newValue)
        {
            internalGrid[x, y] = newValue;
        }

        public void UpdatePoint((int x, int y) point, T newValue)
        {
            UpdatePoint(point.x, point.y, newValue);
        }


        public int CountValues(T value)
        {
            int res = 0;
            for (int x = 0; x < xMax; x++)
                for (int y = 0; y < yMax; y++)
                    if (internalGrid[x, y]!.Equals(value))
                        res++;

            return res;
        }

        public override bool Equals(object? obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Grid<T> other = (Grid<T>)obj;
                if (xMax != other.xMax || yMax != other.yMax)
                    return false;
                for (int x = 0; x < xMax; x++)
                    for (int y = 0; y < yMax; y++)
                        if (internalGrid[x, y]!.Equals(other.internalGrid[x, y]))
                            return false;
                return true;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public Grid<T> Clone()
        {
            return new Grid<T>(this);
        }

        public void Clone(Grid<T> destination)
        {
            for (int x = 0; x < xMax; x++)
                for (int y = 0; y < yMax; y++)
                    destination.internalGrid[x, y] = this.internalGrid[x, y];
        }

        public void Print()
        {
            Console.WriteLine("---------------");
            Console.WriteLine();
            for (int x = 0; x < xMax; x++)
            {
                for (int y = 0; y < yMax; y++)
                    Console.Write(internalGrid[x, y]);
                Console.WriteLine();
            }

        }

        public void Print(Dictionary<T, char> legend)
        {
            Console.WriteLine("---------------");
            Console.WriteLine();
            for (int y = 0; y < yMax; y++)
            {
                for (int x = 0; x < xMax; x++)
                    Console.Write(legend[internalGrid[x, y]]);
                Console.WriteLine();
            }

        }

        public bool Exists(int x, int y)
        {
            if (x >= 0 && x < xMax && y >= 0 && y < yMax)
                return true;
            return false;
        }

        public Grid<T> MergeHorizontal(Grid<T> adding)
        {
            if (this == null)
                return adding;
            else if (adding == null)
                return this;
            Grid<T> res = new(xMax + adding.xMax, yMax, default);
            for (int y = 0; y < yMax; y++)
            {
                for (int x1 = 0; x1 < xMax; x1++)
                    res.internalGrid[x1, y] = this.internalGrid[x1, y];
                for (int x2 = xMax; x2 < xMax + adding.xMax; x2++)
                    res.internalGrid[x2, y] = adding.internalGrid[x2 - xMax, y];
            }
            return res;
        }

        public Grid<T> MergeVertical(Grid<T> adding)
        {
            if (this == null)
                return adding;
            else if (adding == null)
                return this;
            Grid<T> res = new(xMax, yMax + adding.yMax, default);
            for (int x = 0; x < xMax; x++)
            {
                for (int y1 = 0; y1 < yMax; y1++)
                    res.internalGrid[x, y1] = this.internalGrid[x, y1];
                for (int y2 = yMax; y2 < yMax + adding.yMax; y2++)
                    res.internalGrid[x, y2] = adding.internalGrid[x, y2 - yMax];
            }
            return res;
        }
    }

    public class Range
    {
        public readonly int Start;
        public readonly int End;

        private readonly int Min;
        private readonly int Max;

        public int Length
        {
            get => Max - Min;
        }

        public Range(int start, int end)
        {
            this.Start = start;
            this.End = end;
            this.Min = Math.Min(Start, End);
            this.Max = Math.Max(Start, End);
        }

        public Range(string[] value) : this(int.Parse(value[0]), int.Parse(value[1])) { }

        public bool Contains(int value)
        {
            if (value >= Min && value <= Max)
                return true;
            return false;
        }

        public bool Contains(Point point)
        {
            if (point.x >= Min && point.y <= Max)
                return true;
            return false;
        }

        public bool Contains(Range range)
        {
            if (this.Min <= range.Min && this.Max >= range.Max)
                return true;
            return false;
        }

        public bool Overlaps(Range range)
        {
            if (this.Max < range.Min)
                return false;
            else if (this.Min > range.Max)
                return false;
            return true;
        }
         
        public Range Merge(Range range)
        {
            if (this.Overlaps(range))
                return new Range(Math.Min(this.Min, range.Min), Math.Max(this.Max, range.Max));
            else
                throw new InvalidOperationException();
        }
    }

    public class MultiRange
    {
        readonly Range[] ranges;

        public MultiRange(params Range[] ranges)
        {
            this.ranges = ranges;
        }

        public MultiRange(params string[][] value)
        {
            List<Range> ranges = new();
            for (int i = 0; i < value.Length; i++)
            {
                ranges.Add(new Range(value[i]));
            }
            this.ranges = ranges.ToArray();
        }

        public bool Contains(int value)
        {
            foreach (Range range in ranges)
                if (range.Contains(value))
                    return true;
            return false;
        }
    }

    public class Point
    {
        public int x, y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Point other = (Point)obj;
                if (x != other.x || y != other.y)
                    return false;
                return true;
            }
        }

        public double Distance(Point other)
        {
            return Math.Sqrt(Math.Pow(x - other.x, 2) + Math.Pow(y - other.y, 2));
        }

        public override int GetHashCode()
        {
            return int.Parse(x + "0" + y);
        }

        public override string ToString()
        {
            return x + "," + y;
        }
    }

    public class Line
    {
        public Point a, b;

        public Line(Point x, Point y)
        {
            this.a = x;
            this.b = y;
        }

        public Line(int a, int b, int c, int d)
        {
            this.a = new Point(a, b);
            this.b = new Point(c, d);
        }

        public bool IsVertical
        {
            get => a.x == b.x;
        }

        public bool IsHorizontal
        {
            get => a.y == b.y;
        }

        public string[] GetPointsAsStrings()
        {
            int xValue = Math.Sign(b.x - a.x);
            int yValue = Math.Sign(b.y - a.y);

            Point seeker = new(a.x, a.y);
            List<string> res = new();

            while (!seeker.Equals(b))
            {
                res.Add(seeker.ToString());
                seeker.x += xValue;
                seeker.y += yValue;
            }
            res.Add(seeker.ToString());

            return res.ToArray();
        }
    }

    public class Djikstra
    {
        private readonly Grid<int> costs;

        private readonly Grid<int> distances;

        private readonly List<(int x, int y)> closed = new();

        public Djikstra(Grid<int> grid)
        {
            this.costs = grid;
            this.distances = new Grid<int>(costs.xMax, costs.yMax, int.MaxValue);
        }

        public int Calculate((int x, int y) start)
        {
            return Calculate(start, start);
        }

        public int Calculate((int x, int y) start, (int x, int y) end)
        {
            bool searchingEnd = (end != start);

            distances.UpdatePoint(start, 0);
            (int x, int y) curPoint = start;

            var finished = false;
            int res = 0;
            Dictionary<(int x, int y), int> nearestPoints = new();

            while (!finished)
            {
                closed.Add(curPoint);
                var curDistance = distances.GridValues[curPoint.x, curPoint.y];
                res = curDistance;

                if (searchingEnd)
                    if (curPoint == end)
                        return res;

                foreach (var (x, y, value) in costs.GetAdjacentPoints(curPoint, true))
                {
                    if (!closed.Contains((x, y)))
                        if (curDistance + value < distances.GridValues[x, y])
                            distances.UpdatePoint(x, y, curDistance + value);
                }
                foreach (var newPoint in costs.GetAdjacentPoints(curPoint, true).Where(x => !closed.Contains((x.x, x.y))))
                    if (!nearestPoints.ContainsKey((newPoint.x, newPoint.y)) || (curDistance + newPoint.value) < nearestPoints[(newPoint.x, newPoint.y)])
                        nearestPoints.Add((newPoint.x, newPoint.y), curDistance + newPoint.value);

                finished = nearestPoints.Keys.All(x => closed.Contains(x));
                if (!finished)
                {
                    var nearestNotVisited = nearestPoints.Keys.Where(x => !closed.Contains(x)).MinBy(x => nearestPoints[x]);
                    nearestPoints.Remove(nearestNotVisited);
                    var nearestPoint = (nearestNotVisited.x, nearestNotVisited.y);
                    curPoint = nearestPoint;
                }
            }
            return res;
        }

        public Grid<int> GetDistancesFromStart()
        {
            return distances;
        }
    }
    public static class CollectionExtensions
    {
        public static void AddOrSum<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        where TKey : notnull
        where TValue : notnull
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (dictionary.ContainsKey(key))
            {
                dynamic x = dictionary[key];
                dynamic y = value!;
                dictionary[key] = x + y;
            }
            else
            {
                dictionary[key] = value;
            }
        }

    }

    public static class ExtensionMethods
    {
        // Deep clone
        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
            => self.Select((item, index) => (item, index));


        static IList<IList<T>> Permute<T>(this T[] nums)
        {
            var list = new List<IList<T>>();
            return DoPermute(nums, 0, nums.Length - 1, list);
        }

        static IList<IList<T>> DoPermute<T>(T[] nums, int start, int end, IList<IList<T>> list)
        {
            if (start == end)
            {
                // We have one of our possible n! solutions,
                // add it to the list.
                list.Add(new List<T>(nums));
            }
            else
            {
                for (var i = start; i <= end; i++)
                {
                    Swap(ref nums[start], ref nums[i]);
                    DoPermute(nums, start + 1, end, list);
                    Swap(ref nums[start], ref nums[i]);
                }
            }

            return list;
        }

        static void Swap<T>(ref T a, ref T b)
        {
            var temp = a;
            a = b;
            b = temp;
        }
    }
}