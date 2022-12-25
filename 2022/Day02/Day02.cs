using System;
using AdventOfCode2019;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day02
{
    public static void Run()
    {
        var input = Utils.ReadInputAsStrings(Utils.GetInputPath());

        Console.WriteLine(CalculateScore(input));

        Console.WriteLine(CalculateStrategyGuideScore(input));
    }

    private static int CalculateScore(string[] input)
    {

        int score = 0;

        foreach (var line in input)
        {
            var you = line.Split()[0];
            var me = line.Split()[1];

            if (me == "X")
            {
                score += 1;
                if (you == "A")
                    score += 3;
                if (you == "C")
                    score += 6;
            }
            if (me == "Y")
            {
                score += 2;
                if (you == "A")
                    score += 6;
                if (you == "B")
                    score += 3;
            }
            if (me == "Z")
            {
                score += 3;
                if (you == "B")
                    score += 6;
                if (you == "C")
                    score += 3;
            }

        }

        return score;

    }

    private static int CalculateStrategyGuideScore(string[] input)
    {
        int score = 0;

        foreach (var line in input)
        {
            var you = line.Split()[0];
            var me = line.Split()[1];

            if (you == "A")
            {
                //sasso
                if (me == "X")
                {
                    //lose - forbice
                    score += 3;
                }
                if (me == "Y")
                {
                    //pari - sasso
                    score += 3;
                    score += 1;
                }
                if (me == "Z")
                {
                    //vinci - carta
                    score += 6;
                    score += 2;
                }
            }
            if (you == "B")
            {
                //carta
                if (me == "X")
                {
                    //lose - sasso
                    score += 1;
                }
                if (me == "Y")
                {
                    //pari - carta
                    score += 3;
                    score += 2;
                }
                if (me == "Z")
                {
                    //vinci - forbice
                    score += 6;
                    score += 3;
                }
            }
            if (you == "C")
            {
                //forbice
                if (me == "X")
                {
                    //lose - carta
                    score += 2;
                }
                if (me == "Y")
                {
                    //pari - forbice
                    score += 3;
                    score += 3;
                }
                if (me == "Z")
                {
                    //vinci - sasso
                    score += 6;
                    score += 1;
                }
            }

        }

        return score;
    }

}
