using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day04
{
    public static void Run()
    {
        var input = (Utils.ReadInputAsStrings(Utils.GetInputPath()));

        Console.WriteLine(FirstPart(input));

        Console.WriteLine(SecondPart(input));
    }

    private static int FirstPart(string[] input)
    {
        int containingPairs = 0;
        foreach (string assignmentPair in input)
        {
            List<AdventOfCodeUtils.Range> pairs = new();
            foreach (string assignment in assignmentPair.Split(','))
            {
                pairs.Add(new AdventOfCodeUtils.Range(int.Parse(assignment.Split('-')[0]), int.Parse(assignment.Split('-')[1])));
            }
            if (pairs[0].Contains(pairs[1]) || pairs[1].Contains(pairs[0]))
                containingPairs++;
        }
        return containingPairs;
    }

    private static int SecondPart(string[] input)
    {
        int containingPairs = 0;
        foreach (string assignmentPair in input)
        {
            List<AdventOfCodeUtils.Range> pairs = new();
            foreach (string assignment in assignmentPair.Split(','))
            {
                pairs.Add(new AdventOfCodeUtils.Range(int.Parse(assignment.Split('-')[0]), int.Parse(assignment.Split('-')[1])));
            }
            if (pairs[0].Overlaps(pairs[1]))
                containingPairs++;
        }
        return containingPairs;
    }
}
