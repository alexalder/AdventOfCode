using System;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day25
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsStrings(Utils.GetInputPath(2020, 25));

            Console.WriteLine(GetEncryptionKey(input));
        }

        private static double GetEncryptionKey(string[] input)
        {
            double cardKey = double.Parse(input[0]);
            double doorKey = double.Parse(input[1]);

            int cardLoop = GetLoopSize(cardKey);
            double res = Transform(doorKey, cardLoop);

            return res;
        }

        private static double Transform(double subject, int loop)
        {
            double res = 1;
            foreach (int i in Enumerable.Range(0, loop))
            {
                res = res * subject;
                res = res % 20201227;
            }
            return res;
        }

        private static int GetLoopSize(double transform)
        {
            int res = 0;

            int curTransform = 1;
            while (curTransform != transform)
            {
                curTransform = curTransform * 7;
                curTransform = curTransform % 20201227;
                res++;
            }

            return res;
        }
    }
}
