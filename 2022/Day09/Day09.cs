using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day09
{
    public static void Run()
    {
        var input = Utils.ReadInputAsStrings(Utils.GetInputPath());

        //input = Utils.ReadInputAsStrings(Utils.GetTestPath(0));

        Console.WriteLine(ShortRope(input));

        Console.WriteLine(LongRope(input));
    }

    private static int ShortRope(string[] input)
    {
        var commands = input.Select(x => x.Split()).Select(x => (x[0], int.Parse(x[1])));

        (int x, int y) head = (0, 0), tail = (0, 0);
        HashSet<(int x, int y)> result = new();

        foreach (var command in commands)
        {
            (int commandX, int commandY) = Decode(command.Item1);
            for (int i = 0; i < command.Item2; i++)
            {
                head.x += commandX;
                head.y += commandY;

                Follow(head, ref tail);

                if (!result.Contains(tail))
                    result.Add(tail);
            }
        }

        return result.Count;
    }

    private static int LongRope(string[] input)
    {
        var commands = input.Select(x => x.Split()).Select(x => (x[0], int.Parse(x[1])));

        (int x, int y)[] rope = Enumerable.Repeat((0, 0), 10).ToArray();
        HashSet<(int x, int y)> result = new();

        foreach (var command in commands)
        {
            (int commandX, int commandY) = Decode(command.Item1);
            for (int commandIndex = 0; commandIndex < command.Item2; commandIndex++)
            {
                rope[0].x += commandX;
                rope[0].y += commandY;

                for (int ropeIndex = 1; ropeIndex < rope.Length; ropeIndex++)
                    Follow(rope[ropeIndex - 1], ref rope[ropeIndex]);

                if (!result.Contains(rope[9]))
                    result.Add(rope[9]);
            }
        }

        return result.Count;
    }

    private static (int, int) Decode(string direction)
    {
        if (direction == "U")
            return (0, 1);
        else if (direction == "R")
            return (1, 0);
        else if (direction == "D")
            return (0, -1);
        else if (direction == "L")
            return (-1, 0);
        return (0, 0);
    }
    
    private static void Follow ((int x, int y) head, ref (int x, int y) tail)
    {
        if (Math.Sqrt(Math.Pow((head.x - tail.x), 2) + Math.Pow((head.y - tail.y), 2)) >= 2)
        {
            if (head.x > tail.x + 1)
            {
                tail.x += 1;
                if (head.y > tail.y)
                {
                    tail.y += 1;
                }
                else if (head.y < tail.y)
                {
                    tail.y -= 1;
                }
            }
            else if (head.x < tail.x - 1)
            {
                tail.x -= 1;
                if (head.y > tail.y)
                {
                    tail.y += 1;
                }
                else if (head.y < tail.y)
                {
                    tail.y -= 1;
                }
            }
            else if (head.y > tail.y + 1)
            {
                tail.y += 1;
                if (head.x > tail.x)
                {
                    tail.x += 1;
                }
                else if (head.x < tail.x)
                {
                    tail.x -= 1;
                }
            }
            else if (head.y < tail.y - 1)
            {
                tail.y -= 1;
                if (head.x > tail.x)
                {
                    tail.x += 1;
                }
                else if (head.x < tail.x)
                {
                    tail.x -= 1;
                }
            }
        }
    }
}
