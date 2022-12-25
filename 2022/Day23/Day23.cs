using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day23
{
    static readonly (int x, int y) Right = (1, 0);
    static readonly (int x, int y) Down = (0, 1);
    static readonly (int x, int y) Left = (-1, 0);
    static readonly (int x, int y) Up = (0, -1);

    static readonly int readTime = 10;

    public static void Run()
    {
        var input = Utils.ReadInputAsStrings(Utils.GetInputPath());

        Console.WriteLine(BestElf(input));

        Console.WriteLine(BestThree(input));
    }

    static int BestElf(string[] input)
    {
        var parsed = ReadInput(input);

        return SimulateElvesPlanting(parsed, readTime);
    }

    static long BestThree(string[] input)
    {
        var parsed = ReadInput(input);

        return SimulateElvesPlanting(parsed);
    }

    static HashSet<(int x, int y)> ReadInput(string[] input)
    {
        HashSet<(int x, int y)> elves = new();

        for (int stringIndex = 0; stringIndex < input.Length; stringIndex++)
        {
            for (int charIndex = 0; charIndex < input[stringIndex].Length; charIndex++)
            {
                if (input[stringIndex][charIndex] == '#')
                    elves.Add((charIndex, stringIndex));
            }
        }

        return elves;
    }

    static int SimulateElvesPlanting(HashSet<(int x, int y)> elves, int? maxTime = null)
    {
        List<(int x, int y)> directionsOrder = new List<(int x, int y)> { Up, Down, Left, Right };

        for (int round = 0; ; round++)
        {
            if (maxTime.HasValue)
                if (round == maxTime)
                    break;

            Dictionary<(int x, int y), (int x, int y)> proposed = new();

            // Shout
            foreach (var elf in elves)
            {
                // Checks for elves nearby
                bool nearElf = false;
                for (int y = -1; y <= 1; y++)
                    for (int x = -1; x <= 1; x++)
                        if (elves.Contains((elf.x + x, elf.y + y)) && (elf.x + x, elf.y + y) != elf)
                            nearElf = true;

                if (!nearElf)
                    continue;

                // Checks where to move
                foreach (var direction in directionsOrder)
                {
                    if (direction.x != 0)
                    {
                        if (!elves.Contains((elf.x + direction.x, elf.y - 1)) && !elves.Contains((elf.x + direction.x, elf.y)) && !elves.Contains((elf.x + direction.x, elf.y + 1)))
                        {
                            proposed[elf] = (elf.x + direction.x, elf.y);
                            break;
                        }

                    }
                    else //if (direction.y != 0)
                    {
                        if (!elves.Contains((elf.x - 1, elf.y + direction.y)) && !elves.Contains((elf.x, elf.y + direction.y)) && !elves.Contains((elf.x + 1, elf.y + direction.y)))
                        {
                            proposed[elf] = (elf.x, elf.y + direction.y);
                            break;
                        }

                    }
                }
            }

            if (proposed.Count == 0)
                return round + 1;

            // Move
            foreach (var prop in proposed)
            {
                if (proposed.Values.Count(x => x == prop.Value) == 1)
                {
                    elves.Remove(prop.Key);
                    elves.Add(prop.Value);
                }
            }

            // Rotate directions
            var newLastDirection = directionsOrder[0];
            directionsOrder.Remove(newLastDirection);
            directionsOrder.Add(newLastDirection);

        }

        int emptyTiles = 0;

        for (int rectY = elves.Min(x => x.y); rectY <= elves.Max(x => x.y); rectY++)
            for (int rectX = elves.Min(x => x.x); rectX <= elves.Max(x => x.x); rectX++)
                if (!elves.Contains((rectX, rectY)))
                    emptyTiles++;

        return emptyTiles;
    }
}
