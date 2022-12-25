using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day05
{
    public static void Run()
    {
        var input = Utils.SplitInput(Utils.ReadInputAsStrings(Utils.GetInputPath()));

        Console.WriteLine(PredictCrates(input));

        Console.WriteLine(PredictCrates9001(input));
    }

    private static string PredictCrates(string[][] input)
    {
        List<List<char>> stacks = ReadCrates(input);

        foreach (string movement in input[1])
        {
            var values = movement.Split(new String[] { "move ", " from ", " to " }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();

            for (int i = 0; i < values[0]; i++)
            {
                char crate = stacks[values[1] - 1].Last();
                stacks[values[1] - 1].RemoveAt(stacks[values[1] - 1].Count - 1);
                stacks[values[2] - 1].Add(crate);
            }
        }

        return GetLastCrates(stacks);
    }

    private static string PredictCrates9001(string[][] input)
    {
        List<List<char>> stacks = ReadCrates(input);

        foreach (string movement in input[1])
        {
            var values = movement.Split(new String[] { "move ", " from ", " to " }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();

            for (int i = 0; i < values[0]; i++)
            {
                char crate = stacks[values[1] - 1].ElementAt(stacks[values[1] - 1].Count - values[0] + i);
                stacks[values[1] - 1].RemoveAt(stacks[values[1] - 1].Count - values[0] + i);
                stacks[values[2] - 1].Add(crate);
            }
        }

        return GetLastCrates(stacks);
    }

    static List<List<char>> ReadCrates(string[][] input)
    {
        List<List<char>> stacks = new();

        foreach (string line in input[0].Take(input[0].Length - 1))
        {
            int column = 0;
            foreach (char crate in line.Where((c, i) => i % 4 == 1))
            {
                if (stacks.Count == column)
                    stacks.Add(new List<char>());
                if (crate != ' ')
                    stacks[column].Add(crate);
                column++;
            }
        }

        foreach (var stack in stacks)
            stack.Reverse();

        return stacks;
    }

    static string GetLastCrates(List<List<char>> stacks)
    {
        string res = "";

        foreach (List<char> stack in stacks)
        {
            if (stack.Count > 0)
                res += stack.Last();
        }

        return res;
    }
}
