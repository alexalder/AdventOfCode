using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day14
{
    public static void Run()
    {
        var input = Utils.ReadInputAsStrings(Utils.GetInputPath());

        Console.WriteLine(FindLastSand(input));

        Console.WriteLine(FillWithSand(input));
    }

    private static int FindLastSand(string[] input)
    {
        HashSet<(int x, int y)> rocks = ReadInput(input);

        var maxY = rocks.Max(rock => rock.y);
        bool ended = false;
        int sanded = 0;

        while (!ended)
        {
            (int x, int y) sand = (500, 0);
            while (true)
            {
                if (!rocks.Contains((sand.x, sand.y + 1)))
                    sand = (sand.x, sand.y + 1);
                else if (!rocks.Contains((sand.x - 1, sand.y + 1)))
                    sand = (sand.x - 1, sand.y + 1);
                else if (!rocks.Contains((sand.x + 1, sand.y + 1)))
                    sand = (sand.x + 1, sand.y + 1);
                else
                {
                    rocks.Add(sand);
                    sanded++;
                    break;
                }

                if (sand.y > maxY)
                {
                    ended = true;
                    break;
                }
            }
        }

        return sanded;
    }

    private static int FillWithSand(string[] input)
    {
        HashSet<(int x, int y)> rocks = ReadInput(input);

        var maxY = rocks.Max(rock => rock.y);
        bool ended = false;
        int sanded = 0;

        while (!ended)
        {
            (int x, int y) sand = (500, 0);
            while (true)
            {
                if (!rocks.Contains((sand.x, sand.y + 1)) && sand.y < maxY + 1)
                    sand = (sand.x, sand.y + 1);
                else if (!rocks.Contains((sand.x - 1, sand.y + 1)) && sand.y < maxY + 1)
                    sand = (sand.x - 1, sand.y + 1);
                else if (!rocks.Contains((sand.x + 1, sand.y + 1)) && sand.y < maxY + 1)
                    sand = (sand.x + 1, sand.y + 1);
                else
                {
                    sanded++;
                    if (sand == (500, 0))
                    {
                        ended = true;
                    }

                    rocks.Add(sand);
                    break;
                }
            }
        }

        return sanded;
    }

    private static HashSet<(int x, int y)> ReadInput(string[] input)
    {
        HashSet<(int x, int y)> rocks = new();

        foreach (var line in input)
        {
            (int x, int y) prevPair = (0, 0);
            foreach (var pair in line.Split(" -> ").Select(x => x.Split(",")))
            {
                (int x, int y) curPair;
                curPair.x = int.Parse(pair[0]);
                curPair.y = int.Parse(pair[1]);

                if (prevPair.x == prevPair.y && prevPair.x == 0)
                    prevPair = curPair;
                else
                {
                    if (curPair.x == prevPair.x)
                    {
                        int curY = prevPair.y;
                        while (true)
                        {
                            rocks.Add((curPair.x, curY));
                            if (curY == curPair.y)
                                break;
                            else
                                curY += Math.Sign(curPair.y - prevPair.y);
                        }
                    }
                    else if (curPair.y == prevPair.y)
                    {
                        int curX = prevPair.x;
                        while (true)
                        {
                            rocks.Add((curX, curPair.y));
                            if (curX == curPair.x)
                                break;
                            else
                                curX += Math.Sign(curPair.x - prevPair.x);
                        }
                    }
                    prevPair = curPair;
                }
            }
        }

        return rocks;
    }
}
