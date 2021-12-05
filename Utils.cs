using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Utils
{
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

    public static long[] GetDigits(long input)
    {
        string read = input.ToString();
        long[] res = read.ToString().Select(t => long.Parse(t.ToString())).ToArray();
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

    public static int DivideIntegersRoundUp(int a, int b, out int remainder)
    {
        int result = (a + b - 1) / b;
        remainder = b * result - a;
        return result;
    }

    public static double DivideDoubleRoundUp(double a, double b, out double remainder)
    {
        double result = Math.Ceiling(a/b);
        remainder = b * result - a;
        return result;
    }

    public static int BinaryToDecimal(string a)
    {
        return Convert.ToInt32(a, 2);
    }

    public static string GetFolder(int year, int day)
    {
        string[] separators = { "bin" };
        return Path.Combine(Directory.GetCurrentDirectory().Split(separators, StringSplitOptions.RemoveEmptyEntries)[0], year.ToString(), "Day" + day.ToString("D2"));
    }

    public static string GetInputPath(int year, int day)
    {
        return Path.Combine(GetFolder(year, day), "input.txt");
    }

    public static string GetTestPath(int year, int day, int test = 0)
    {
        return Path.Combine(GetFolder(year, day), "testInput" + test + ".txt");

    }

    public static string[][] SplitInput(string[] input, string separator)
    {
        List<string[]> res = new List<string[]>();
        List<string> curList = new List<string>();

        for (int i = 0; i < input.Length; i++)
        {
            var line = input[i];
            if (line != separator)
                curList.Add(line);
            var sette = Array.IndexOf(input, line);
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
        Dictionary<int, int> factors = new Dictionary<int, int>();

        foreach(int number in numbers)
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
        Dictionary<int, int> factors = new Dictionary<int, int>();

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
                    number = number / factor;
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
        List<int> factors = new List<int>();

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
            cur[cur.Length - 1]++;
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
}

public class TreeNode<T>
{
    public T value;
    public List<TreeNode<T>> children = new List<TreeNode<T>>();
    public int depth;

    public TreeNode(T value)
    {
        this.value = value;
        this.depth = 0;
    }

    public TreeNode(T value, int depth)
    {
        this.value = value;
        this.depth = depth;
    }

    public TreeNode<T> AddNode(T value)
    {
        TreeNode<T> newNode = new TreeNode<T>(value, depth+1);
        children.Add(newNode);
        return newNode;
    }

    public void Traverse(Action<TreeNode<T>> action)
    {
        action(this);
        foreach (TreeNode<T> node in children)
            node.Traverse(action);
    }
}

public class Grid
{
    char[,] internalGrid;
    int xMax, yMax;

    public char[,] GridValues { get => internalGrid; }

    public Grid(string[] input)
    {
        internalGrid = new char[input[0].Length, input.Length];
        for (int x = 0; x < input[0].Length; x++)
            for (int y = 0; y < input.Length; y++)
                internalGrid[x, y] = input[y][x];

        xMax = input[0].Length;
        yMax = input.Length;
    }

    public Grid(Grid otherGrid)
    {
        internalGrid = new char[otherGrid.xMax, otherGrid.yMax];
        for (int x = 0; x < otherGrid.xMax; x++)
            for (int y = 0; y < otherGrid.yMax; y++)
                internalGrid[x, y] = otherGrid.GridValues[x, y];

        xMax = otherGrid.xMax;
        yMax = otherGrid.yMax;
    }

    public char[] GetAdjacentValues(int x, int y)
    {
        List<char> res = new List<char>();
        if (x < xMax - 1)
            res.Add(internalGrid[x + 1, y]);
        if (y < yMax - 1)
            res.Add(internalGrid[x, y + 1]);
        if (x > 0)
            res.Add(internalGrid[x - 1, y]);
        if (y > 0)
            res.Add(internalGrid[x, y - 1]);
        if (x > 0 && y > 0)
            res.Add(internalGrid[x - 1, y - 1]);
        if (x > 0 && y < yMax - 1)
            res.Add(internalGrid[x - 1, y + 1]);
        if (x < xMax - 1 && y > 0)
            res.Add(internalGrid[x + 1, y - 1]);
        if (x < xMax - 1 && y < yMax - 1)
            res.Add(internalGrid[x + 1, y + 1]);
        return res.ToArray();
    }

    public void UpdateGridAdjacent(Func<char, char[], char> updateFunc)
    {
        Grid oldValues = this.Clone();
        for (int x = 0; x < xMax; x++)
            for (int y = 0; y < yMax; y++)
                internalGrid[x, y] = updateFunc(oldValues.GridValues[x, y], oldValues.GetAdjacentValues(x, y));
    }

    public bool UpdateGrid(Func<int, int, Grid, char> updateFunc)
    {
        Grid oldGrid = this.Clone();
        bool changed = false;
        for (int x = 0; x < xMax; x++)
        {
            for (int y = 0; y < yMax; y++)
            {
                char newValue = updateFunc(x, y, oldGrid);
                if (internalGrid[x, y] != newValue)
                {
                    internalGrid[x, y] = newValue;
                    changed = true;
                }
            }
        }
        return changed;
    }

    public int CountValues(char value)
    {
        int res = 0;
        for (int x = 0; x < xMax; x++)
            for (int y = 0; y < yMax; y++)
                if (internalGrid[x, y] == value)
                    res++;

        return res;
    }

    public override bool Equals(Object obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            Grid other = (Grid)obj;
            if (xMax != other.xMax || yMax != other.yMax)
                return false;
            for (int x = 0; x < xMax; x++)
                for (int y = 0; y < yMax; y++)
                    if (internalGrid[x, y] != other.internalGrid[x, y])
                        return false;
            return true;
        }
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public Grid Clone()
    {
        return new Grid(this);
    }

    public void Clone(Grid destination)
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

    public bool Exists(int x, int y)
    {
        if (x >= 0 && x < xMax && y >= 0 && y < yMax)
            return true;
        return false;
    }

    
}

public class Range
{
    int min, max;

    public Range(int min, int max)
    {
        this.min = min;
        this.max = max;
    }

    public Range(string[] value)
    {
        this.min = int.Parse(value[0]);
        this.max = int.Parse(value[1]);
    }

    public bool Contains(int value)
    {
        if (value >= min && value <= max)
            return true;
        return false;
    }
}

public class MultiRange
{
    Range[] ranges;

    public MultiRange(params Range[] ranges)
    {
        this.ranges = ranges;
    }

    public MultiRange(params string[][] value)
    {
        List<Range> ranges = new List<Range>();
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