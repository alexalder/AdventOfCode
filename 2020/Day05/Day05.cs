using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day05
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsStrings(Utils.GetInputPath(2020, 5));

            Console.WriteLine(FindHighestID(input));

            Console.WriteLine(FindMissingSeat(input));
        }

        private static int FindHighestID(string[] input)
        {
            int highestID = 0;

            foreach (string pass in input)
            {
                if (GetSeatId(pass) > highestID)
                    highestID = GetSeatId(pass);
            }

            return highestID;
        }

        private static int FindMissingSeat(string[] input)
        {
            List<(int, int)> seats = new List<(int, int)>();
            for (int i = 0; i < 128; i++)
                for (int j = 0; j < 8; j++)
                    seats.Add((i, j));

            List<(int, int)> passes = new List<(int, int)>();
            foreach (string pass in input)
                passes.Add(ResolveSeat(pass));

            List<(int, int)> missingSeats = seats.Except(passes).ToList();

            int curSeat = 0;
            foreach (var seat in missingSeats)
            {
                if (seat.Item1 == curSeat)
                    continue;
                if (seat.Item1 == curSeat + 1)
                    curSeat++;
                else
                    return GetSeatId(seat.Item1, seat.Item2);
            }

            return -1;
        }

        private static (int, int) ResolveSeat(string pass)
        {
            int[] seats = new int[128];
            int[] columns = new int[8];

            for (int i = 0; i < seats.Length; i++)
                seats[i] = i;

            for (int i = 0; i < columns.Length; i++)
                columns[i] = i;

            int letterIndex = 0;
            foreach (char letter in pass)
            {
                if (letterIndex < 7)
                {
                    if (letter == 'F')
                        seats = TakeLowerHalf(seats);
                    else
                        seats = TakeUpperHalf(seats);
                }
                else
                {
                    if (letter == 'L')
                        columns = TakeLowerHalf(columns);
                    else
                        columns = TakeUpperHalf(columns);
                }
                letterIndex++;
            }

            return (seats[0], columns[0]);
        }

        private static int[] TakeUpperHalf(int[] array)
        {
            return array.Skip(array.Length / 2).Take(array.Length / 2).ToArray();
        }

        private static int[] TakeLowerHalf(int[] array)
        {
            return array.Take(array.Length / 2).ToArray();
        }

        private static int GetSeatId(int row, int column)
        {
            return row * 8 + column;
        }

        private static int GetSeatId(string pass)
        {
            var seat = ResolveSeat(pass);
            return seat.Item1 * 8 + seat.Item2;
        }
    }
}