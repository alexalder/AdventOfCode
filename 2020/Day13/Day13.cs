using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day13
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsStrings(Utils.GetInputPath(2020, 13));

            Console.WriteLine(FirstDeparture(input));

            Console.WriteLine(FindBusAlignment(input));
        }

        private static int FirstDeparture(string[] input)
        {
            int departure = FindDepartureTime(input);

            int[] busIDs = FindBusLines(input);

            int chosenBus = 0;
            int wait = int.MaxValue;
            foreach (int busID in busIDs)
            {
                int accumulator = 0;
                while (accumulator < departure)
                    accumulator += busID;
                if (accumulator - departure < wait)
                {
                    wait = accumulator - departure;
                    chosenBus = busID;
                }
            }
            return chosenBus * wait;
        }

        private static double FindBusAlignment(string[] input)
        {
            int[] busIDs = FindBusLinesWithSpaces(input);
            double multiple = 1;
            double result = 0;

            for (int considered = 2; considered < busIDs.Length; considered++)
            {
                double alignment = busIDs[0];
                if (result != 0)
                    alignment = result;

                while (1 < 2)
                {
                    bool found = true;
                    for (int i = 0; i <= considered; i++)
                    {
                        if (busIDs[i] != -1)
                        {
                            int offset = (int)((alignment + i) % busIDs[i]);
                            if ((alignment + i) % busIDs[i] != 0)
                            {
                                found = false;
                                break;
                            }
                        }
                    }
                    if (found)
                        break;
                    else
                        alignment += multiple;
                }
                result = alignment;

                multiple = 1;
                foreach (string id in input[1].Split(',').Take(considered))
                    if (id != "x")
                        multiple *= int.Parse(id);
            }

            return result;
        }

        private static int FindDepartureTime(string[] input)
        {
            return int.Parse(input[0]);
        }

        private static int[] FindBusLinesWithSpaces(string[] input)
        {
            List<int> busIDs = new List<int>();
            foreach (string id in input[1].Split(','))
            {
                if (id != "x")
                    busIDs.Add(int.Parse(id));
                else
                    busIDs.Add(-1);
            }
            return busIDs.ToArray();
        }

        private static int[] FindBusLines(string[] input)
        {
            List<int> busIDs = new List<int>();
            foreach (string id in input[1].Split(','))
                if (id != "x")
                    busIDs.Add(int.Parse(id));

            return busIDs.ToArray();
        }
    }
}