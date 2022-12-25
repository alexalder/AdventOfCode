using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day12
{
    public static void Run()
    {
        var input = Utils.ReadInputAsStrings(Utils.GetInputPath());

        Console.WriteLine(CalculatePath(input));

        Console.WriteLine(CalculateHiking(input));
    }

    private static int CalculatePath(string[] input)
    {
        Grid<char> map = new Grid<char>(input, x => x);

        HashSet<(int x, int y, char value)> openPoints = new();
        foreach (var value in map)
        {
            if (value.value == 'S')
                openPoints.Add(value);
        }

        return Djikstra(map, openPoints);
    }

    private static int CalculateHiking(string[] input)
    {
        Grid<char> map = new Grid<char>(input, x => x);
        
        HashSet<(int x, int y, char value)> openPoints = new();
        foreach (var value in map)
        {
            if (value.value == 'a')
                openPoints.Add(value);
        }

        return Djikstra(map, openPoints);
    }

    static int Djikstra(Grid<char> map, HashSet<(int x, int y, char value)> openPoints)
    {
        HashSet<(int x, int y)> visitedPoints = new();
        HashSet<(int x, int y, char value)> newPoints = new();

        bool found = false;
        int steps = 0;
        while (!found)
        {
            foreach (var open in openPoints)
            {
                foreach (var adjacent in map.GetAdjacentPoints((open.x, open.y), true))
                {
                    if (adjacent.value == 'E' && open.value >= 'y')
                    {
                        found = true;
                        break;
                    }
                    if (!visitedPoints.Contains((adjacent.x, adjacent.y)))
                        if (adjacent.value - open.value <= 1 || (open.value == 'S' && adjacent.value <= 'b'))
                            newPoints.Add(adjacent);
                }
            }
            foreach (var open in openPoints)
                visitedPoints.Add((open.x, open.y));
            openPoints.Clear();

            foreach (var newPoint in newPoints)
                openPoints.Add(newPoint);
            newPoints.Clear();

            steps++;
        }

        return steps;
    }
}
