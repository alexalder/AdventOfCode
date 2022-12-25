using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day21
{
    public static void Run()
    {
        var input = Utils.ReadInputAsStrings(Utils.GetInputPath());

        Console.WriteLine(FindMonkeyShout(input));

        Console.WriteLine(FindMonkeyAndHumanShout(input));
    }

    static long FindMonkeyShout(string[] input)
    {
        Dictionary<string, (string, char, string)> again;
        Dictionary<string, int> numbers;
        (again, numbers) = ParseInput(input);

        return (long)SolveMonkeys("root", again, numbers);
    }

    static long FindMonkeyAndHumanShout(string[] input)
    {
        Dictionary<string, (string firstKey, char op, string secondKey)> again;
        Dictionary<string, int> numbers;
        (again, numbers) = ParseInput(input);

        string firstHalfKey = again["root"].secondKey;
        long firstHalf = (long)SolveMonkeys(firstHalfKey, again, numbers);

        string secondHalfKey = again["root"].firstKey;

        return SolveMonkeysBisection(secondHalfKey, again, numbers, firstHalf);
    }

    static (Dictionary<string, (string firstKey, char op, string secondKey)>, Dictionary<string, int>) ParseInput(string[] input)
    {
        Dictionary<string, (string, char, string)> again = new();
        Dictionary<string, int> numbers = new();

        foreach (var line in input)
        {
            var all = line.Split(": ");
            int number = 0;
            if (int.TryParse(all[1], out number))
                numbers.Add(all[0], number);
            else
                again.Add(all[0], (all[1].Split()[0], all[1].Split()[1][0], all[1].Split()[2]));
        }

        return (again, numbers);
    }

    static double SolveMonkeys(string key, Dictionary<string, (string firstKey, char op, string secondKey)> again, Dictionary<string, int> numbers, long? human = null)
    {
        if (key == "humn" && human != null)
            return (long)human;
        else if (numbers.ContainsKey(key))
            return numbers[key];
        else
        {
            var agains = again[key];

            if (agains.op == '+')
                return SolveMonkeys(agains.firstKey, again, numbers, human) + SolveMonkeys(agains.secondKey, again, numbers, human);
            else if (agains.op == '-')
                return SolveMonkeys(agains.firstKey, again, numbers, human) - SolveMonkeys(agains.secondKey, again, numbers, human);
            else if (agains.op == '*')
                return SolveMonkeys(agains.firstKey, again, numbers, human) * SolveMonkeys(agains.secondKey, again, numbers, human);
            else if (agains.op == '/')
                return SolveMonkeys(agains.firstKey, again, numbers, human) / SolveMonkeys(agains.secondKey, again, numbers, human);
            else
                throw new NotImplementedException();
        }
    }

    static long SolveMonkeysBisection(string secondHalfKey, Dictionary<string, (string, char, string)> again, Dictionary<string, int> numbers, long firstHalf)
    {
        long min = 0;
        long max;

        int signo = Math.Sign(SolveMonkeys(secondHalfKey, again, numbers, 1) - firstHalf);

        for (long humn = 1; ; humn *= 2)
        {
            var secondHalf = SolveMonkeys(secondHalfKey, again, numbers, humn);
            var newSigno = Math.Sign(secondHalf - firstHalf);

            if (firstHalf == secondHalf)
                return humn;
            else if (signo == newSigno)
                min = humn;
            else if (signo != newSigno)
            {
                max = humn;
                break;
            }
        }

        return Utils.BisectionMethod(
            min, SolveMonkeys(secondHalfKey, again, numbers, min),
            max, SolveMonkeys(secondHalfKey, again, numbers, max),
            x => SolveMonkeys(secondHalfKey, again, numbers, x),
            firstHalf);
    }
}
