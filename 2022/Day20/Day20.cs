using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day20
{
    static readonly long decryptionKey = 811589153;
    static readonly int moreIterations = 10;

    public static void Run()
    {
        var input = Utils.ReadInputAsStrings(Utils.GetInputPath());

        Console.WriteLine(MixFile(input));

        Console.WriteLine(BestThree(input));
    }

    static long MixFile(string[] input)
    {
        List<Number> numbers;
        Dictionary<long, Number> numbersDict;

        (numbers, numbersDict) = ParseInput(input);

        for (int curNumber = 0; curNumber < numbers.Count; curNumber++)
        {
            var index = numbers.IndexOf(numbersDict[curNumber]);
            MoveElement(numbersDict[curNumber], numbers, numbersDict[curNumber].value + index);
        }

        return GetGroveCoordinates(numbers);
    }


    static long BestThree(string[] input)
    {
        List<Number> numbers;
        Dictionary<long, Number> numbersDict;

        (numbers, numbersDict) = ParseInput(input, decryptionKey);

        for (int iterations = 0; iterations < moreIterations; iterations++)
        {
            for (int curNumber = 0; curNumber < numbers.Count; curNumber++)
            {
                var index = numbers.IndexOf(numbersDict[curNumber]);
                MoveElement(numbersDict[curNumber], numbers, numbersDict[curNumber].value + index);
            }
        }

        return GetGroveCoordinates(numbers);
    }

    static (List<Number> numbers, Dictionary<long, Number> numbersDict) ParseInput(string[] input, long multiplier = 1)
    {
        var parsed = input.Select(x => int.Parse(x)).ToList();


        List<Number> numbers = new();
        Dictionary<long, Number> numbersDict = new();
        Number zero = new Number { start = 0, value = 0 };
        int i = 0;
        foreach (var x in parsed)
        {
            var realValue = x * multiplier;

            var number = new Number { start = i, value = realValue };
            numbersDict.Add(i, number);
            numbers.Add(number);
            if (realValue == 0)
                zero = number;
            i++;
        }

        return (numbers, numbersDict);
    }

    static void MoveElement<T>(T element, List<T> list, long index)
    {
        int count = list.Count;

        list.Remove(element);

        while (index > count)
            index = index % (count - 1);
        while (index < 0)
        {
            index = index % (count - 1);
            index = index >= 0 ? index : index + count - 1;
        }
        if (index == 0)
            index = count - 1;

        list.Insert((int)index, element);
    }

    static long GetGroveCoordinates(List<Number> numbers)
    {
        Number zero = numbers.First(x => x.value == 0);
        return GetElementAfter(numbers, zero, 1000).value + GetElementAfter(numbers, zero, 2000).value + GetElementAfter(numbers, zero, 3000).value;
    }

    static T GetElementAfter<T>(List<T> list, T target, int index)
    {
        return GetElement(list, index + list.IndexOf(target));
    }

    static T GetElement<T>(List<T> list, int index)
    {
        while (index > list.Count)
            index = (index) % list.Count;
        return list[index];
    }

    internal class Number
    {
        internal long start;
        internal long value;
    }
}
