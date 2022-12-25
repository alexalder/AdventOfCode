using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day03
{
    public static void Run()
    {
        var input = (Utils.ReadInputAsStrings(Utils.GetInputPath()));

        Console.WriteLine(FirstPart(input));

        Console.WriteLine(SecondPart(input));
    }

    private static int FirstPart(string[] input)
    {
        int sumPriorities = 0;
        foreach (string rucksack in input)
        {
            string compartmentOne = rucksack.Substring(0, rucksack.Length / 2);
            string compartmentTwo = rucksack.Substring(rucksack.Length / 2, rucksack.Length / 2);

            foreach (char item in compartmentOne)
            {
                if (compartmentTwo.Contains(item))
                {
                    sumPriorities += GetItemPriority(item);
                    break;
                }
            }
        }
        return sumPriorities;
    }

    private static int SecondPart(string[] input)
    {

        int sumPriorities = 0;
        foreach (string[] group in input.Chunk(3))
        {
            foreach (char item in group[0])
            {
                if (group.All(x => x.Contains(item)))
                {
                    sumPriorities += GetItemPriority(item);
                    break;
                }
            }
        }
        return sumPriorities;
    }

    static int GetItemPriority(char item)
    {
        if (item > 'Z') // Lowercase
            return (int)item - 96;
        else // Uppercase
            return (int)item - 64 + 26;
    }
}
