using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day01
{
    public static void Run()
    {
        var input = Utils.SplitInput(Utils.ReadInputAsStrings(Utils.GetInputPath()));

        Console.WriteLine(BestElf(input));

        Console.WriteLine(BestThree(input));
    }

    static int BestElf(string[][] input)
    {
        return input.Select(x => x.Sum(x => int.Parse(x)))
            .OrderByDescending(x => x)
            .First();
    }

    static int BestThree(string[][] input)
    {
        return input.Select(x => x.Sum(x => int.Parse(x)))
            .OrderByDescending(x => x)
            .Take(3)
            .Sum();
    }
}
