using System;

namespace AdventOfCode2020
{
    public static class Day01
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsInts(Utils.GetInputPath(2020, 1));

            Console.WriteLine(Find2(input));

            Console.WriteLine(Find3(input));
        }

        private static double Find2(int[] input)
        {
            foreach (int entry in input)
                foreach (int otherEntry in input)
                    if (entry + otherEntry == 2020)
                        return entry * otherEntry;

            return -1;
        }

        private static double Find3(int[] input)
        {
            foreach (int entry in input)
                foreach (int otherEntry in input)
                    foreach (int anotherEntry in input)
                        if (entry + otherEntry + anotherEntry == 2020)
                            return entry * otherEntry * anotherEntry;

            return -1;
        }
    }
}