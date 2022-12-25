using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day11
{
    public static void Run()
    {
        var input = Utils.SplitInput(Utils.ReadInputAsStrings(Utils.GetInputPath()));

        Console.WriteLine(MonkeyBusiness(input));

        Console.WriteLine(MonkeyBusinessWorried(input));
    }

    private static int MonkeyBusiness(string[][] input)
    {
        List<Monkey> monkeyList = new();
        foreach (var monkey in input)
        {
            Func<int, int> operation = (x => x);

            string signum = monkey[2].Substring("  Operation: new = old ".Length).Split()[0];
            string valor = monkey[2].Substring("  Operation: new = old ".Length).Split()[1];

            if (valor == "old")
            {
                if (signum == "*")
                    operation = (x => x * x);
                else if (signum == "+")
                    operation = (x => x + x);
            }
            else
                if (signum == "*")
                    operation = (x => x * int.Parse(valor));
                else if (signum == "+")
                    operation = (x => x + int.Parse(valor));

            monkeyList.Add(new Monkey(
                monkey[1].Substring("  Starting items: ".Length).Split(',').Select(x => x.Trim()).Select(x => int.Parse(x)).ToList(),
                operation,
                int.Parse(monkey[3].Substring("  Test: divisible by ".Length)),
                int.Parse(monkey[4].Substring("    If true: throw to monkey ".Length)),
                int.Parse(monkey[5].Substring("    If false: throw to monkey ".Length))
            ));

        }

        for (int i = 0; i < 20; i++)
            foreach (var activeMonkey in monkeyList)
                activeMonkey.Round(monkeyList);

        return monkeyList.Select(x => x.inspectedItems).OrderByDescending(x => x).Take(2).Aggregate(1, (x, y) => x * y);
    }

    private static double MonkeyBusinessWorried(string[][] input)
    {
        List<BigMonkey> monkeyList = new();
        foreach (var monkey in input)
        {
            Action<HugeInt> operation = (x => { });

            string signum = monkey[2].Substring("  Operation: new = old ".Length).Split()[0];
            string valor = monkey[2].Substring("  Operation: new = old ".Length).Split()[1];

            if (valor == "old")
            {
                if (signum == "*")
                    operation = (x => x.Square());
            }
            else
                if (signum == "*")
                operation = (x => x.Multiply(int.Parse(valor)));
            else if (signum == "+")
                operation = (x => x.Sum(int.Parse(valor)));

            monkeyList.Add(new BigMonkey(
                monkey[1].Substring("  Starting items: ".Length).Split(',').Select(x => x.Trim()).Select(x => new HugeInt(int.Parse(x), new List<int> { 13, 3, 7, 2, 19, 5, 11, 17})).ToList(),
                operation,
                int.Parse(monkey[3].Substring("  Test: divisible by ".Length)),
                int.Parse(monkey[4].Substring("    If true: throw to monkey ".Length)),
                int.Parse(monkey[5].Substring("    If false: throw to monkey ".Length))
            ));

        }

        for (int i = 0; i < 10000; i++)
            foreach (var activeMonkey in monkeyList)
                activeMonkey.Round(monkeyList);

        return monkeyList.Select(x => x.inspectedItems).OrderByDescending(x => x).Take(2).Aggregate(1.0, (x, y) => x * y);
    }

    class Monkey
    {
        List<int> items = new();
        Func<int, int> operation;
        int test;
        int ifTrue, ifFalse;

        internal int inspectedItems;

        internal Monkey(List<int> items, Func<int, int> operation, int test, int ifTrue, int ifFalse)
        {
            this.items = items;
            this.operation = operation;
            this.test = test;
            this.ifTrue = ifTrue;
            this.ifFalse = ifFalse;
        }

        internal void Round(List<Monkey> monkeys)
        {
            foreach (int item in items)
            {
                var newItem = operation(item);
                newItem = (int)MathF.Floor((float)newItem / 3);
                if (newItem % test == 0)
                    monkeys[ifTrue].items.Add(newItem);
                else
                    monkeys[ifFalse].items.Add(newItem);

                inspectedItems++;
            }
            items.Clear();
        }
    }

    class BigMonkey
    {
        List<HugeInt> items = new();
        Action<HugeInt> operation;
        internal int testingValue;
        int ifTrue, ifFalse;

        internal int inspectedItems;

        internal BigMonkey(List<HugeInt> items, Action<HugeInt> operation, int test, int ifTrue, int ifFalse)
        {
            this.items = items;
            this.operation = operation;
            this.testingValue = test;
            this.ifTrue = ifTrue;
            this.ifFalse = ifFalse;
        }

        internal void Round(List<BigMonkey> monkeys)
        {
            foreach (HugeInt item in items)
            {
                operation(item);
                if (item.IsDivisible(testingValue))
                    monkeys[ifTrue].items.Add(item);
                else
                    monkeys[ifFalse].items.Add(item);

                inspectedItems++;
            }
            items.Clear();
        }
    }

    public class HugeInt
    {
        Dictionary<int, int> remainders = new Dictionary<int, int>();

        public HugeInt(int value, List<int> divisors)
        {
            foreach (var divisor in divisors)
            {
                remainders[divisor] = value % divisor;
            }
        }

        public void Sum(int value)
        {
            foreach (var remainder in remainders)
            {
                remainders[remainder.Key] = (remainder.Value + value) % remainder.Key;
            }
        }

        public void Multiply(int value)
        {
            foreach (var remainder in remainders)
            {
                remainders[remainder.Key] = (remainder.Value * value) % remainder.Key;
            }
        }

        public void Square()
        {
            foreach (var remainder in remainders)
            {
                remainders[remainder.Key] = (remainder.Value * remainder.Value) % remainder.Key;
            }
        }

        public bool IsDivisible(int value)
        {
            return remainders[value] == 0;
        }
    }
}
