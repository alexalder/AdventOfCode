using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day24
{
    static readonly (int x, int y) Right = (1, 0);
    static readonly (int x, int y) Down = (0, 1);
    static readonly (int x, int y) Left = (-1, 0);
    static readonly (int x, int y) Up = (0, -1);

    static readonly (int x, int y) startPosition = (1, 0);

    static Dictionary<string, (int x, int y)> windDirections = new() { { "right", Right }, { "down", Down }, { "left", Left }, { "up", Up } };
    public static void Run()
    {
        var input = Utils.ReadInputAsStrings(Utils.GetInputPath());

        Console.WriteLine(GetThrough(input));

        Console.WriteLine(BackAndAgain(input));
    }

    static int GetThrough(string[] input)
    {
        Dictionary<int, Dictionary<string, HashSet<(int x, int y)>>> windsHistory = ReadInput(input);

        int maxY = input.Length - 1;
        int maxX = input[0].Length - 1;

        (int x, int y) endPosition = (maxX - 1, maxY);

        HashSet<State> possibilites = new();

        possibilites.Add(new State { position = startPosition, maxX = maxX, maxY = maxY });

        return Run(new State { position = startPosition, maxX = maxX, maxY = maxY }, endPosition, windsHistory);
    }

    static int BackAndAgain(string[] input)
    {
        Dictionary<int, Dictionary<string, HashSet<(int x, int y)>>> windsHistory = ReadInput(input);

        int maxY = input.Length - 1;
        int maxX = input[0].Length - 1;

        (int x, int y) endPosition = (maxX - 1, maxY);

        HashSet <State> possibilites = new();

        possibilites.Add(new State { position = startPosition, maxX = maxX, maxY = maxY });

        int time = 0;

        time += Run(new State { position = startPosition, maxX = maxX, maxY = maxY }, endPosition, windsHistory);
        time += Run(new State { position = endPosition, maxX = maxX, maxY = maxY }, startPosition, windsHistory);
        time += Run(new State { position = startPosition, maxX = maxX, maxY = maxY }, endPosition, windsHistory);

        return time;
    }

    static Dictionary<int, Dictionary<string, HashSet<(int x, int y)>>> ReadInput(string[] input)
    {
        HashSet<(int x, int y)> right = new(), down = new(), left = new(), up = new();
        Dictionary<string, HashSet<(int x, int y)>> winds = new() { { "right", right }, { "down", down }, { "left", left }, { "up", up } };
        Dictionary<int, Dictionary<string, HashSet<(int x, int y)>>> windsHistory = new() { { 0, winds } };

        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[y].Length; x++)
            {

                if (input[y][x] == '>')
                    right.Add((x, y));
                else if (input[y][x] == 'v')
                    down.Add((x, y));
                else if (input[y][x] == '<')
                    left.Add((x, y));
                else if (input[y][x] == '^')
                    up.Add((x, y));
            }
        }

        return windsHistory;
    }

    static int Run(State start, (int x, int y) target, Dictionary<int, Dictionary<string, HashSet<(int x, int y)>>> windsHistory)
    {
        HashSet<State> possibilites = new();

        possibilites.Add(start);

        int time = 0;

        while (true)
        {
            time++;
            HashSet<State> newPossibilities = new();

            foreach (var possibility in possibilites)
            {
                var curPossibilities = Solve(possibility, time, windsHistory, possibility.maxX, possibility.maxY, target);

                foreach (var resulting in curPossibilities)
                    if (resulting.position == target)
                        return time;

                foreach (var adding in curPossibilities)
                    newPossibilities.Add(adding);
            }

            possibilites = newPossibilities;
        }

        throw new Exception();
    }

    internal struct State
    {
        internal (int x, int y) position;
        internal int maxX;
        internal int maxY;
    }

    private static List<State> Solve(State curState, int curTime, Dictionary<int, Dictionary<string, HashSet<(int x, int y)>>> windsHistory, int maxX, int maxY, (int x, int y) target)
    {
        List<State> results = new();

        var currentWinds = GetTime(windsHistory, curTime, maxX, maxY);

        foreach (var direction in windDirections.Values)
        {
            (int x, int y) newPosition = (curState.position.x + direction.x, curState.position.y + direction.y);

            // Reached the end
            if (newPosition == target)
            {
                results.Add(new State { position = newPosition, maxX = maxX, maxY = maxY });
                return results;
            }

            // Check boundaries
            if (newPosition.x <= 0 || newPosition.y <= 0 || newPosition.x >= maxX || newPosition.y >= maxY)
                continue;

            // Check winds
            else
            {
                bool occupied = false;
                foreach (var windType in currentWinds.Values)
                    if (windType.Contains(newPosition))
                    {
                        occupied = true;
                        break;
                    }

                // newPosition is free
                if (!occupied)
                    results.Add(new State { position = newPosition, maxX = maxX, maxY = maxY });
            }
        }

        // Check if current position remains free
        bool curOccupied = false;
        foreach (var windType in currentWinds.Values)
            if (windType.Contains(curState.position))
            {
                curOccupied = true;
                break;
            }

        if (!curOccupied)
            results.Add(new State { position = curState.position, maxX = maxX, maxY = maxY });


        return results;
    }
    private static Dictionary<string, HashSet<(int x, int y)>> GetTime(Dictionary<int, Dictionary<string, HashSet<(int x, int y)>>> windsHistory, int time, int maxX, int maxY)
    {
        // Winds have periodicity maxX - 1 * maxY - 1
        int offsettedTime = time;
        if (time >= (maxX - 1) * (maxY - 1))
            offsettedTime = time % ((maxX - 1) * (maxY - 1));

        if (!windsHistory.ContainsKey(offsettedTime))
            SimulateTime(windsHistory, offsettedTime, maxX, maxY);

        return windsHistory[offsettedTime];
    }

    private static void SimulateTime(Dictionary<int, Dictionary<string, HashSet<(int x, int y)>>> windsHistory, int time, int maxX, int maxY)
    {
        Dictionary<string, HashSet<(int x, int y)>> newTime = new();
        Dictionary<string, HashSet<(int x, int y)>> oldTime = windsHistory[time - 1];

        foreach (var direction in new string[] { "right", "down", "left", "up" })
        {
            HashSet<(int x, int y)> winds = new();
            foreach (var wind in oldTime[direction])
            {
                (int x, int y) newPosition = (wind.x + windDirections[direction].x, wind.y + windDirections[direction].y);

                if (newPosition.x == maxX)
                    newPosition.x = 1;
                if (newPosition.x == 0)
                    newPosition.x = maxX - 1;
                if (newPosition.y == maxY)
                    newPosition.y = 1;
                if (newPosition.y == 0)
                    newPosition.y= maxY - 1;

                winds.Add(newPosition);
            }
            newTime[direction] = winds;
        }

        windsHistory[time] = newTime;
    }

}
