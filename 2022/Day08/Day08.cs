using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day08
{
    public static void Run()
    {
        var input = Utils.ReadInputAsStrings(Utils.GetInputPath());

        //var input = Utils.ReadInputAsStrings(Utils.GetTestPath());

        var numbers = input.Select(x => Utils.GetDigits(x)).ToArray();

        Console.WriteLine(CountVisibileTrees(numbers));

        Console.WriteLine(BestThree(numbers));
    }

    private static int CountVisibileTrees(int[][] input)
    {
        int visibleTrees = 0;

        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[y].Length; x++)
            {
                if (x == 0 || y == 0 || x == input[y].Length - 1 || y == input.Length - 1)
                    visibleTrees++;
                else
                {
                    if (input[y].Skip(x + 1).All(tree => tree < input[y][x]))
                        visibleTrees++;
                    else if (input[y].Take(x).All(tree => tree < input[y][x]))
                        visibleTrees++;
                    else if (input.Select(line => line[x]).Take(y).All(tree => tree < input[y][x]))
                        visibleTrees++;
                    else if (input.Select(line => line[x]).Skip(y + 1).All(tree => tree < input[y][x]))
                        visibleTrees++;
                }
            }
        }


        return visibleTrees;
    }

    private static int BestThree(int[][] input)
    {
        int maxVisibility = 0;

        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[y].Length; x++)
            {
                int up = 0, left = 0, down = 0, right = 0;
                if (x == 0 || y == 0 || x == input[y].Length - 1 || y == input.Length - 1)
                    continue;
                else
                {
                    foreach (var tree in input[y].Skip(x + 1))
                    {
                        if (tree < input[y][x])
                            right += 1;
                        else
                        {
                            right += 1;
                            break;
                        }
                            
                    }

                    foreach (var tree in input[y].Take(x).Reverse())
                    {
                        if (tree < input[y][x])
                            left += 1;
                        else
                        {
                            left += 1;
                            break;
                        }
                    }

                    foreach (var tree in input.Select(line => line[x]).Take(y).Reverse())
                    {
                        if (tree < input[y][x])
                            up += 1;
                        else
                        {
                            up += 1;
                            break;
                        }
                    }

                    foreach (var tree in input.Select(line => line[x]).Skip(y + 1))
                    {
                        if (tree < input[y][x])
                            down += 1;
                        else
                        {
                            down += 1;
                            break;
                        }
                    }
                }

                var visibility = up * left * down * right;
                if (visibility > maxVisibility)
                    maxVisibility = visibility;
            }
        }

        return maxVisibility;
    }
}
