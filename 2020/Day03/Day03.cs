using System;

namespace AdventOfCode2020
{
    public class Day03
    {
        static string[] input;

        public static void Run()
        {
            input = Utils.ReadInputAsStrings(Utils.GetInputPath(2020, 3));

            Console.WriteLine(CountTrees((3, 1)));

            Console.WriteLine(CountTrees((1, 1), (3, 1), (5, 1), (7, 1), (1, 2)));
        }

        private static double CountTrees(params (int, int)[] speeds)
        {
            double res = 1;
            foreach (var speed in speeds)
                res *= CountTrees(speed);
            return res;
        }

        private static double CountTrees((int, int) speed)
        {
            int xSpeed = speed.Item1;
            int ySpeed = speed.Item2;
            int x = 0, y = 0;
            int stringLength = input[0].Length;
            double trees = 0;
            while (y < input.Length)
            {
                if (x >= stringLength)
                    x -= stringLength;
                if (input[y][x] == '#')
                    trees++;
                x += xSpeed;
                y += ySpeed;
            }
            return trees;
        }
    }
}