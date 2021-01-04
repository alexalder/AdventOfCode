using System;
using System.Reflection;

namespace AdventOfCode2020
{
    public class Day12
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsStrings(Utils.GetInputPath(2020, 12));

            Console.WriteLine(MoveShip(input));

            Console.WriteLine(MoveShipWaypoint(input));
        }

        private static int MoveShipWaypoint(string[] input)
        {
            Position ship = new Position();
            Position waypoint = new Position(10, 1, 0);

            foreach (string command in input)
            {
                char action = command[0];
                int value = int.Parse(command.Substring(1));

                if (action == 'N')
                {
                    waypoint += value * Position.Nord;
                }
                if (action == 'S')
                {
                    waypoint += value * Position.South;
                }
                if (action == 'E')
                {
                    waypoint += value * Position.East;
                }
                if (action == 'W')
                {
                    waypoint += value * Position.West;
                }
                if (action == 'L')
                {
                    waypoint.Rotate(-value);
                }
                if (action == 'R')
                {
                    waypoint.Rotate(value);
                }
                if (action == 'F')
                {
                    ship.Move(value * waypoint.x, value * waypoint.y);
                }
            }
            return ship.ManhattanDistance;
        }

        private static int MoveShip(string[] input)
        {
            Position curPosition = new Position();

            foreach (string command in input)
            {
                char action = command[0];
                int value = int.Parse(command.Substring(1));

                if (action == 'N')
                {
                    curPosition += value * Position.Nord;
                }
                if (action == 'S')
                {
                    curPosition += value * Position.South;
                }
                if (action == 'E')
                {
                    curPosition += value * Position.East;
                }
                if (action == 'W')
                {
                    curPosition += value * Position.West;
                }
                if (action == 'L')
                {
                    curPosition.Steer(-value);
                }
                if (action == 'R')
                {
                    curPosition.Steer(value);
                }
                if (action == 'F')
                {
                    curPosition.GoForward(value);
                }
            }

            return curPosition.ManhattanDistance;
        }

        class Position
        {
            public int x, y;
            public int heading = 90;

            public Position()
            {
                this.x = 0;
                this.y = 0;
            }

            public Position(int x, int y, int heading)
            {
                this.x = x;
                this.y = y;
            }

            public void Steer(int degrees)
            {
                heading += degrees;
                if (heading >= 360)
                    heading -= 360;
                if (heading < 0)
                    heading += 360;
            }

            public void GoForward(int value)
            {
                if (heading == 0)
                    y += value;
                else if (heading == 90)
                    x += value;
                else if (heading == 180)
                    y -= value;
                else if (heading == 270)
                    x -= value;
            }

            public void Move(int x, int y)
            {
                this.x += x;
                this.y += y;
            }

            public void Rotate(int value)
            {
                while (value >= 360)
                    value -= 360;
                while (value < 0)
                    value += 360;

                while (value > 0)
                {
                    int oldX = x;
                    int oldY = y;

                    this.x = oldY;
                    this.y = -oldX;
                    value -= 90;
                }
            }

            public int ManhattanDistance
            {
                get => Math.Abs(x) + Math.Abs(y);
            }

            public static Position operator +(Position a) => a;

            public static Position operator +(Position a, Position b)
            {
                a.Move(b.x, b.y);
                return a;
            }

            public static Position operator *(Position a, int b)
                => new Position(a.x * b, a.y * b, a.heading);

            public static Position operator *(int a, Position b)
                => new Position(b.x * a, b.y * a, b.heading);

            public static Position Nord
            {
                get => new Position(0, 1, 0);
            }
            public static Position South
            {
                get => new Position(0, -1, 0);
            }
            public static Position West
            {
                get => new Position(-1, 0, 0);
            }
            public static Position East
            {
                get => new Position(1, 0, 0);
            }
        }
    }
}