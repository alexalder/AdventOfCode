using System;
using System.Collections;
using System.Linq;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day06
{
    public static void Run()
    {
        var input = Utils.ReadInputAsStrings(Utils.GetInputPath())[0];

        Console.WriteLine(FindStartOfPacket(input));

        Console.WriteLine(FindStartOfMessage(input));
    }

    private static int FindStartOfPacket(string input)
    {
        return FindPacketOfLength(input, 4);
    }

    private static int FindStartOfMessage(string input)
    {
        return FindPacketOfLength(input, 14);
    }

    private static int FindPacketOfLength(string input, int length)
    {
        Queue q = new Queue();

        int position = 1;

        foreach (char c in input)
        {
            q.Enqueue(c);
            if (q.Count > length)
                q.Dequeue();
            if (q.Count == length)
            {
                if (q.ToArray().All(x => q.ToArray().Count(y => y.Equals(x)) == 1))
                    return position;
            }
            position++;
        }

        return 0;
    }
}
