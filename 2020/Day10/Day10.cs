using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day10
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsInts(Utils.GetInputPath(2020, 10));

            Console.WriteLine(FindDifferencesMultiplied(input));

            Console.WriteLine(FindArrangements(input));
        }

        private static int FindDifferencesMultiplied(int[] input)
        {
            List<int> adapters = input.ToList();
            adapters.Sort();
            adapters.Add(adapters.Max() + 3);

            int one = 0;
            int three = 0;
            int previous = 0;
            foreach (int adapter in adapters)
            {
                if (adapter - previous == 1)
                    one++;
                if (adapter - previous == 3)
                    three++;
                previous = adapter;
            }

            return one * three;
        }

        private static double FindArrangements(int[] input)
        {
            List<int> adapters = input.ToList();
            adapters.Add(0);
            adapters.Sort();
            int[] adaptersArray = adapters.ToArray();
            double arrangements = 1;
            List<int> subsequence = new List<int>();
            int index = -1;
            while (index < adaptersArray.Length - 1)
            {
                do
                {
                    index++;
                    subsequence.Add(adaptersArray[index]);
                }
                while (index + 1 < adaptersArray.Length && adaptersArray[index + 1] - adaptersArray[index] != 3);

                arrangements *= CalculateSubSequence(subsequence);
                subsequence = new List<int>();

            }

            return arrangements;
        }

        private static double CalculateSubSequence(List<int> input)
        {
            if (input.Count == 1)
                return 1;
            if (input.Count == 2)
                return 1;
            if (input.Count == 3)
                return 2;
            if (input.Count == 4)
                return 4;
            if (input.Count == 5)
                return 7;
            return -1;
        }
    }
}