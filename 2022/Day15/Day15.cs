using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day15
{
    static int targetLine = 2000000;
    static int maxRange = 4000000;

    public static void Run()
    {
        var input = Utils.ReadInputAsStrings(Utils.GetInputPath());

        Console.WriteLine(FindNoBeacons(input));

        Console.WriteLine(FindBeacon(input));
    }

    static int FindNoBeacons(string[] input)
    {
        var noBeacon = FindNoBeacons(input, targetLine, targetLine, x => { });

        return noBeacon[targetLine].Length;
    }

    static double FindBeacon(string[] input)
    {
        (int x, int y) missingBeacon = (0,0);

        var noBeacons = FindNoBeacons(input, 0, maxRange, x => missingBeacon = x);

        return (double)missingBeacon.x * 4000000 + missingBeacon.y;
    }

    static Dictionary<int, AdventOfCodeUtils.Range> FindNoBeacons(string[] input, int min, int max, Action<(int x, int y)> foundMissing)
    {
        Dictionary<int, List<AdventOfCodeUtils.Range>> noBeacon = new();

        for (int y = min; y <= max; y++)
            noBeacon[y] = new();

        foreach (var line in input)
        {
            var positions = line.Split(new[] { "Sensor at x=", ": closest beacon is at x=", ", y=", ": c", }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();

            (int x, int y) sensorPosition = (positions[0], positions[1]);
            (int x, int y) beaconPosition = (positions[2], positions[3]);

            int distance = Math.Abs(beaconPosition.x - sensorPosition.x) + Math.Abs(beaconPosition.y - sensorPosition.y);

            for (int usefulLine = min; usefulLine <= max; usefulLine++)
            {
                if (Math.Abs(usefulLine - sensorPosition.y) > distance)
                    continue;

                int value = Math.Abs(Math.Abs(usefulLine - sensorPosition.y) - distance);

                noBeacon[usefulLine].Add(new AdventOfCodeUtils.Range(sensorPosition.x - value, sensorPosition.x + value));
            }

        }

        return CompactRanges(noBeacon, foundMissing);
    }

    static Dictionary<int, AdventOfCodeUtils.Range> CompactRanges(Dictionary<int, List<AdventOfCodeUtils.Range>> noBeacon, Action<(int x, int y)> foundMissing)
    {
        bool found = false;
        Dictionary<int, AdventOfCodeUtils.Range> res = new();

        foreach (var listObject in noBeacon)
        {
            var list = listObject.Value;
            while (list.Count > 1)
            {
                for (int otherIndex = 1; otherIndex < list.Count; otherIndex++)
                {
                    if (list[0].Overlaps(list[otherIndex]))
                    {
                        var newRange = list[0].Merge(list[otherIndex]);
                        list.RemoveAt(otherIndex);
                        list.RemoveAt(0);
                        list.Add(newRange);
                        break;
                    }
                    else if (otherIndex == list.Count - 1)
                    {
                        int missingX = 0;
                        if (list[0].Start == list[1].End + 2)
                            missingX = list[1].End + 1;
                        else if (list[1].Start == list[0].End + 2)
                            missingX = list[0].End + 1;
                        foundMissing((missingX, listObject.Key));
                        found = true;
                    }
                }
                if (found)
                    break;
            }
            res.Add(listObject.Key, listObject.Value[0]);
            if (found)
                break;

        }

        return res;
    }

}
