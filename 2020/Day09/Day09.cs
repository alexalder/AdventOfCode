using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day09
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsDoubles(Utils.GetInputPath(2020, 9));

            Console.WriteLine(FindInvalidNumber(input));

            Console.WriteLine(FindEncryptionWeakness(input));
        }

        private static bool IsSum(double number, double[] latest25)
        {
            foreach (double n in latest25)
                foreach (double m in latest25)
                    if (n != m)
                        if (n + m == number)
                            return true;

            return false;
        }

        private static double FindInvalidNumber(double[] input, int preambleLenght = 25)
        {
            for (int curNumber = preambleLenght; curNumber < input.Length; curNumber++)
            {
                if (!IsSum(input[curNumber], input.Skip(curNumber - preambleLenght).Take(preambleLenght).ToArray()))
                    return input[curNumber];
            }
            return -1;
        }

        private static double FindEncryptionWeakness(double[] input)
        {
            var invalidNumber = FindInvalidNumber(input);

            List<double> curList = new List<double>();
            foreach (double number in input)
            {
                if (number == invalidNumber)
                    continue;
                curList.Add(number);
                if (curList.Sum() == invalidNumber)
                {
                    return curList.Max() + curList.Min();
                }
                if (curList.Sum() > invalidNumber)
                {
                    //curList = new List<double>();
                    while (curList.Sum() > invalidNumber)
                        curList.RemoveAt(0);
                    if (curList.Sum() == invalidNumber)
                    {
                        return curList.Max() + curList.Min();
                    }
                }
            }

            return -1;
        }
    }
}