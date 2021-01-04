using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day15
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsStrings(Utils.GetInputPath(2020, 15));

            Console.WriteLine(MemoryGameDictionary(input, 2020));

            Console.WriteLine(MemoryGameDictionary(input, 30000000));
        }

        private static int MemoryGameDictionary(string[] input, int spoken)
        {
            Dictionary<int, int> pastNumbers = new Dictionary<int, int>();
            var startingNumbers = input[0].Split(',').ToList().ConvertAll(x => int.Parse(x)).ToArray();
            int i = 1;
            foreach (int number in startingNumbers)
                pastNumbers[number] = i++;

            int lastNumber = 0;
            int newValue = 0;
            int spokeNumber = 0;

            for (int curNumber = startingNumbers.Length; curNumber <= spoken; curNumber++)
            {
                if (curNumber == startingNumbers.Length)
                    continue;
                if (pastNumbers.Keys.Contains(lastNumber))
                {
                    spokeNumber = lastNumber;
                    newValue = curNumber - pastNumbers[lastNumber];

                    pastNumbers[lastNumber] = curNumber;

                    lastNumber = newValue;
                }
                else
                {
                    spokeNumber = lastNumber;

                    if (!pastNumbers.Keys.Contains(lastNumber))
                        pastNumbers.Add(lastNumber, curNumber);
                    else
                        pastNumbers[lastNumber] = curNumber;

                    newValue = 0;
                    lastNumber = newValue;
                }
            }

            return spokeNumber;
        }
    }
}