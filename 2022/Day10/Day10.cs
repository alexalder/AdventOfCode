using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day10
{
    public static void Run()
    {
        var input = Utils.ReadInputAsStrings(Utils.GetInputPath());

        Console.WriteLine(CheckSignal(input));

        ReadDisplay(input);
    }

    private static int CheckSignal(string[] input)
    {

        int sumStrength = 0;

        OperateDisplay(input, (cycle, register) =>
        {
            if (cycle % 40 == 20)
                sumStrength += cycle * register;
        });

        return sumStrength;
    }

    private static void ReadDisplay(string[] input)
    {
        List<char> display = new();

        OperateDisplay(input, (cycle, register) =>
        {
            int pixel = cycle - 1;
            if (Math.Abs(register - pixel % 40) <= 1)
                display.Add('#');
            else
                display.Add('.');
        });

        foreach (var line in display.Chunk(40))
        {
            Console.WriteLine(line);
        }
    }

    private static void OperateDisplay(string[] input, Action<int, int> program)
    {
        int cycle = 0;
        int register = 1;

        void Cycle()
        {
            cycle++;
            program(cycle, register);
        }

        foreach (var command in input)
        {
            if (command == "noop")
                Cycle();
            else if (command.StartsWith("addx"))
            {
                Cycle();
                Cycle();
                register += int.Parse(command.Substring(5));
            }
        }
    }
}
