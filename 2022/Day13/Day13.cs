using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day13
{
    public static void Run()
    {
        var input = Utils.SplitInput(Utils.ReadInputAsStrings(Utils.GetInputPath()));

        Console.WriteLine(ReadPackets(input));

        Console.WriteLine(SortPackets(input));
    }

    private static int ReadPackets(string[][] input)
    {
        var packets = ReadInput(input);
        int sum = 0;

        for (int i = 0; i < packets.Length; i++)
        {
            var packet = packets[i];
            if (ComparePacketLists(packet[0], packet[1]) < 0)
                sum += i + 1;
        }

        return sum;
    }

    private static int SortPackets(string[][] input)
    {
        var packets = ReadInput(input);
        var allPackets = packets.SelectMany(x => x).ToList();
        PacketList packetA = new PacketList { packets = { new PacketList { packets = { new PacketInt { packetInt = 2 } } } } };
        PacketList packetB = new PacketList { packets = { new PacketList { packets = { new PacketInt { packetInt = 6 } } } } };
        allPackets.Add(packetA);
        allPackets.Add(packetB);

        allPackets.Sort(ComparePacketLists);

        int mul = 1;
        for (int i = 0; i < allPackets.Count; i++)
        {
            var packet = allPackets[i];
            if (packet == packetA || packet == packetB)
                mul *= (i+1);
        }

        return mul;
    }

    private static PacketList[][] ReadInput(string[][] input)
    {
        List<PacketList[]> res = new();
        foreach (var pair in input)
        {
            List <PacketList> resPair = new();
            foreach (var packet in pair)
            {
                resPair.Add(ReadPacket(packet.Substring(1)));
            }
            res.Add(resPair.ToArray());
        }
        return res.ToArray();
    }

    private static PacketList ReadPacket(string packetString)
    {
        PacketList packetList = new PacketList();
        string accumulator = "";
        for (int i = 0; i < packetString.Length; i++)
        {
            char c = packetString[i];
            if (c == ',')
            {
                packetList.packets.Add(new PacketInt { packetInt = int.Parse(accumulator) });
                accumulator = "";
            }
            else if (c == '[')
            {
                int levels = 1;
                int start = i;
                for (i++ ; i < packetString.Length && levels > 0; i++)
                {
                    c = packetString[i];
                    if (c == '[')
                        levels++;
                    else if (c == ']')
                        levels--;
                }
                packetList.packets.Add(ReadPacket(packetString.Substring(start + 1, i - 1 - start)));
            }
            else if (c == ']')
            {
                if (!string.IsNullOrEmpty(accumulator))
                    packetList.packets.Add(new PacketInt { packetInt = int.Parse(accumulator) });
                return packetList;
            } 
            else
            {
                accumulator += c;
            }
        }
        return packetList;
    }

    static int ComparePacketLists(PacketList a, PacketList b)
    {
        for (int i = 0; i < a.packets.Count; i++)
        {
            if (i >= b.packets.Count)
                return 1;
            var compare = ComparePackets(a.packets[i], b.packets[i]);
            if (compare != 0)
                return compare;
        }
        return -1;
    }

    static int ComparePackets(Packet a, Packet b) { 
        if (a is PacketInt && b is PacketInt)
        {
            PacketInt packetIntA = (PacketInt)a;
            PacketInt packetIntB = (PacketInt)b;

            if (packetIntA.packetInt == packetIntB.packetInt)
                return 0;
            else if (packetIntA.packetInt > packetIntB.packetInt)
                return 1;
            else if (packetIntA.packetInt < packetIntB.packetInt)
                return -1;
        }
        else
        {
            PacketList packetA, packetB;
            if (a is PacketInt)
            {
                packetA = new PacketList { packets = new List<Packet> { a } };
                packetB = (PacketList)b;
            }
            else if (b is PacketInt)
            {
                packetA = (PacketList)a;
                packetB = new PacketList { packets = new List<Packet> { b } };
            }
            else
            {
                packetA = (PacketList)a;
                packetB = (PacketList)b;
            }

            for (int i= 0; i<packetA.packets.Count; i++)
            {
                if (i >= packetB.packets.Count)
                    return 1;
                var compare = ComparePackets(packetA.packets[i], packetB.packets[i]);
                if (compare != 0)
                    return compare;
            }
            if (packetA.packets.Count == packetB.packets.Count)
                return 0;
            return -1;
        }
        return 2;
    }

    internal class Packet { }

    internal class PacketList : Packet
    {
        internal List<Packet> packets = new();
    }

    internal class PacketInt : Packet
    {
        internal int packetInt;
    }
}
