using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day23
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsStrings(Utils.GetInputPath(2020, 23));

            Console.WriteLine(Part1(input));

            // This takes a few hours because i couldn't think of a better data structure :(
            // A simple LinkedNodes Dictionary would have sufficed i think.
            Console.WriteLine(Part2(input));
        }

        private static double Part2(string[] input)
        {
            CupsCircle cups = ParseCupsCircle(input, 1000000);

            cups.DoMoves(10000000);

            double result = 1;

            foreach (int cupValue in cups.GetAfter(1, 2))
            {
                result *= cupValue;
            }

            return result;
        }

        private static string Part1(string[] input)
        {
            CupsCircle cups = ParseCupsCircle(input);

            cups.DoMoves(100);

            return String.Join("", Array.ConvertAll(cups.GetAfter(1, 8), x => x.ToString()));
        }

        private static CupsCircle ParseCupsCircle(string[] input, int cupsNumber = 9)
        {
            string starting = input[0];

            List<int> cups = starting.ToList().ConvertAll(x => x.ToString()).ConvertAll(x => int.Parse(x));
            for (int newCup = 10; newCup <= cupsNumber; newCup++)
                cups.Add(newCup);

            return new CupsCircle(cups);
        }
    }

    public class CupsCircle
    {
        LinkedList<int> cups = new LinkedList<int>();
        List<int> removedCups = new List<int>();
        LinkedListNode<int> currentCupNode;
        int maxValue;

        private int NextCupValue
        {
            get
            {
                int nextCupValue = currentCupNode.Value - 1;

                if (nextCupValue < 1)
                    nextCupValue = maxValue;

                while (removedCups.Contains(nextCupValue))
                {
                    nextCupValue--;
                    if (nextCupValue < 1)
                        nextCupValue = maxValue;
                }

                return nextCupValue;
            }
        }

        private LinkedListNode<int> NextCupNode
        {
            get
            {
                if (currentCupNode.Next == null)
                    return cups.First;
                else
                    return currentCupNode.Next;
            }
        }

        public CupsCircle(List<int> cups)
        {
            foreach (int cup in cups)
            {
                this.cups.AddLast(cup);
            }
            currentCupNode = this.cups.First;
            maxValue = cups.Count;
        }

        public void DoMoves(int moves)
        {
            foreach (int move in Enumerable.Range(0, moves))
            {
                removedCups = new List<int>();

                for (int remov = 0; remov < 3; remov++)
                {
                    removedCups.Add(NextCupNode.Value);
                    cups.Remove(NextCupNode);
                }

                removedCups.Reverse();

                LinkedListNode<int> targetCupNode;
                targetCupNode = Find(NextCupValue);

                for (int remov = 0; remov < 3; remov++)
                {
                    cups.AddAfter(targetCupNode, removedCups[remov]);
                }

                currentCupNode = NextCupNode;
            }
        }

        public int[] GetAfter(int cup, int number)
        {
            List<int> res = new List<int>();
            LinkedListNode<int> found = cups.Find(cup);
            foreach (int times in Enumerable.Range(0, number))
            {
                if (found.Next == null)
                    found = cups.First;
                else
                    found = found.Next;
                res.Add(found.Value);
            }
            return res.ToArray();
        }

        private LinkedListNode<int> Find(int value)
        {
            if (value > maxValue)
                value = 1;
            if (value < 1)
                value = maxValue;
            return cups.Find(value);
        }
    }
}