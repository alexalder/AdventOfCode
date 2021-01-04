using System;
using System.Collections.Generic;

namespace AdventOfCode2020
{
    public class Day08
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsStrings(Utils.GetInputPath(2020, 8));

            Console.WriteLine(DebugLoop(input));

            Console.WriteLine(FixLoop(input));
        }

        private static int DebugLoop(string[] input)
        {
            int accumulator = 0;
            int curLine = 0;
            int nextLine = 0;
            List<int> ranLine = new List<int>();

            while (!ranLine.Contains(curLine))
            {
                accumulator += RunLine(input[curLine], out nextLine);
                ranLine.Add(curLine);
                curLine += nextLine;
            }
            return accumulator;
        }

        private static int FixLoop(string[] input)
        {
            int maxTries = 0;
            foreach (string line in input)
                if (line.Contains("jmp") || line.Contains("nop"))
                    maxTries++;

            for (int curTry = 0; curTry < maxTries; curTry++)
            {
                string[] newInput = new string[input.Length];
                Array.Copy(input, newInput, input.Length);

                int lineFound = 0;
                for (int line = 0; line < newInput.Length; line++)
                {
                    if (newInput[line].Contains("jmp") || newInput[line].Contains("nop"))
                    {
                        if (lineFound == curTry)
                        {
                            if (newInput[line].Contains("jmp"))
                                newInput[line] = newInput[line].Replace("jmp", "nop");
                            else if (newInput[line].Contains("nop"))
                                newInput[line] = newInput[line].Replace("nop", "jmp");
                            break;
                        }
                        lineFound++;
                    }
                }

                try
                {
                    return (RunProgram(newInput));
                }
                catch (Exception e)
                {
                    continue;
                }
            }

            return -1;
        }

        private static int RunLine(string line, out int nextLine)
        {
            int accumulation = 0;
            nextLine = 1;
            string op = line.Split(' ')[0];
            if (op == "acc")
                accumulation = int.Parse(line.Split(' ')[1]);
            else if (op == "jmp")
                nextLine = int.Parse(line.Split(' ')[1]);
            return accumulation;
        }

        private static int RunProgram(string[] input)
        {
            int accumulator = 0;
            int curLine = 0;
            int nextLine = 0;

            List<int> ranLine = new List<int>();

            while (curLine != input.Length)
            {
                if (ranLine.Contains(curLine))
                    throw new Exception("Looping");
                accumulator += RunLine(input[curLine], out nextLine);
                ranLine.Add(curLine);
                curLine += nextLine;
            }

            return accumulator;
        }
    }
}