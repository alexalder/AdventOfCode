using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day18
{
    public static void Run()
    {
        var input = Utils.ReadInputAsStrings(Utils.GetInputPath());

        Console.WriteLine(FindSurfaceArea(input));

        Console.WriteLine(FindExternalSurfaceArea(input));
    }

    static int FindSurfaceArea(string[] input)
    {
        HashSet<(int x, int y, int z)> blocks = new();

        foreach (var line in input)
        {
            var blockLine = line.Split(',').Select(x=> int.Parse(x)).ToArray();
            blocks.Add((blockLine[0], blockLine[1], blockLine[2]));
        }

        int surfaceArea = 0;

        foreach (var block in blocks)
        {
            if (!blocks.Contains((block.x + 1, block.y, block.z)))
                surfaceArea++;
            if (!blocks.Contains((block.x - 1, block.y, block.z)))
                surfaceArea++;
            if (!blocks.Contains((block.x, block.y + 1, block.z)))
                surfaceArea++;
            if (!blocks.Contains((block.x, block.y - 1, block.z)))
                surfaceArea++;
            if (!blocks.Contains((block.x, block.y, block.z + 1)))
                surfaceArea++;
            if (!blocks.Contains((block.x, block.y, block.z - 1)))
                surfaceArea++;
        }

        return surfaceArea;
    }

    static int FindExternalSurfaceArea(string[] input)
    {
        HashSet<(int x, int y, int z)> blocks = new();
        HashSet<(int x, int y, int z)> externalEmptyBlocks = new();

        foreach (var line in input)
        {
            var blockLine = line.Split(',').Select(x => int.Parse(x)).ToArray();
            blocks.Add((blockLine[0], blockLine[1], blockLine[2]));
        }

        int surfaceArea = 0;
        int blockIndex = 0;

        foreach (var block in blocks)
        {
            foreach (var nearblock in NearBlocks(block))
                if (CheckBlock(blocks, externalEmptyBlocks, nearblock))
                    surfaceArea++;
        }

        return surfaceArea;
    }

    static IEnumerable<(int x, int y, int z)> NearBlocks((int x, int y, int z) block)
    {
        yield return (block.x + 1, block.y, block.z);
        yield return (block.x - 1, block.y, block.z);
        yield return (block.x, block.y + 1, block.z);
        yield return (block.x, block.y - 1, block.z);
        yield return (block.x, block.y, block.z + 1);
        yield return (block.x, block.y, block.z - 1);
    }

    static bool CheckBlock(HashSet<(int x, int y, int z)> blocks, HashSet<(int x, int y, int z)> externalEmptyBlocks, (int x, int y, int z) block)
    {
        if (blocks.Contains(block))
            return false;
        else 
        {
            List<(int x, int y, int z)> emptyBlocks = new();
            List<(int x, int y, int z)> liveEmptyBlocks = new();

            int minX = blocks.Min(block => block.x);
            int minY = blocks.Min(block => block.y);
            int minZ = blocks.Min(block => block.z);
            int maxX = blocks.Max(block => block.x);
            int maxY = blocks.Max(block => block.y);
            int maxZ = blocks.Max(block => block.z);
            // Block is empty

            liveEmptyBlocks.Add(block);

            while (liveEmptyBlocks.All(x => !externalEmptyBlocks.Contains(x) && x.x >= minX && x.x <= maxX && x.y >= minY && x.y <= maxY && x.z >= minZ && x.z <= maxZ)){

                List<(int x, int y, int z)> curBlocks = new();

                foreach (var liveEmptyBlock in liveEmptyBlocks)
                    foreach (var nearblock in NearBlocks(liveEmptyBlock))
                        if (!blocks.Contains(nearblock))
                            if (!liveEmptyBlocks.Contains(nearblock) && !emptyBlocks.Contains(nearblock) && !curBlocks.Contains(nearblock))
                                curBlocks.Add(nearblock);

                emptyBlocks.AddRange(liveEmptyBlocks);
                liveEmptyBlocks.Clear();
                if (curBlocks.Count == 0)
                    return false;
                liveEmptyBlocks.AddRange(curBlocks);
                curBlocks.Clear();
            }
            foreach (var emptyBlock in emptyBlocks)
                externalEmptyBlocks.Add(emptyBlock);
            foreach (var emptyBlock in liveEmptyBlocks)
                externalEmptyBlocks.Add(emptyBlock);
            return true;
        }
    }
}
