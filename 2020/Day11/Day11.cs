using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day11
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsStrings(Utils.GetInputPath(2020, 11));

            Console.WriteLine(GetOccupiedSeats(input));

            Console.WriteLine(GetOccupiedSeatsUpdated(input));
        }

        private static char SeatsRule(char value, char[] adjacent)
        {
            if (value == 'L')
            {
                if (!adjacent.Any((arg) => arg == '#'))
                    return '#';

            }
            else if (value == '#')
            {
                if (adjacent.Count((arg) => arg == '#') > 3)
                    return 'L';
            }
            return value;
        }

        private static char SeatsRuleUpdated(int x, int y, Grid grid)
        {
            char value = grid.GridValues[x, y];
            List<char> viewSeats = new List<char>();
            int[] directions = { -1, 0, 1 };

            foreach (int xDirection in directions)
            {
                foreach (int yDirection in directions)
                {
                    if (xDirection == 0 && yDirection == 0)
                        continue;
                    int curSeatX = x + xDirection;
                    int curSeatY = y + yDirection;
                    while (grid.Exists(curSeatX, curSeatY))
                    {

                        if (grid.GridValues[curSeatX, curSeatY] == 'L' || grid.GridValues[curSeatX, curSeatY] == '#')
                        {
                            viewSeats.Add(grid.GridValues[curSeatX, curSeatY]);
                            break;
                        }
                        curSeatX = curSeatX + xDirection;
                        curSeatY = curSeatY + yDirection;
                    }
                }
            }

            char[] adjacent = viewSeats.ToArray();

            if (value == 'L')
            {
                if (!adjacent.Any((arg) => arg == '#'))
                    return '#';

            }
            else if (value == '#')
            {
                if (adjacent.Count((arg) => arg == '#') > 4)
                    return 'L';
            }

            return value;
        }

        private static int GetOccupiedSeats(string[] input)
        {
            Grid myGrid = new Grid(input);
            Grid oldGrid = myGrid.Clone();
            while (1 < 2)
            {
                myGrid.UpdateGridAdjacent(SeatsRule);
                if (myGrid.Equals(oldGrid))
                    break;
                oldGrid = myGrid.Clone();
            }
            return myGrid.CountValues('#');
        }

        private static int GetOccupiedSeatsUpdated(string[] input)
        {
            Grid myGrid = new Grid(input);
            while (myGrid.UpdateGrid(SeatsRuleUpdated)) ;
            return myGrid.CountValues('#');
        }
    }
}