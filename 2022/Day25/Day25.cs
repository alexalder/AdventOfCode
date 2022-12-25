using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day25
{
    public static void Run()
    {
        var input = Utils.ReadInputAsStrings(Utils.GetInputPath());

        Console.WriteLine(BestElf(input));

        Console.WriteLine(BestThree(input));
    }

    static string BestElf(string[] input)
    {
        long total = 0;
        foreach (var line in input)
        {
            long place = 1;
            var test = line.Reverse();
            foreach (var ch in test)
            {
                if (ch == '=')
                    total += -2 * place;
                else if (ch == '-')
                    total += -1 * place;
                else if (ch == '1')
                    total += 1 * place;
                else if (ch == '2')
                    total += 2 * place;

                place *= 5;
            }
        }

        //total = 4890;
        //total = 12345;

        int numbrs = 0;
        double dividing = total;
        while (dividing > 2)
        {
            dividing /= 5;
            numbrs++;
        }


        //long tryo = 0;
        long rest = total;
        List<char> res = new();

        long Max(int numbers)
        {
            long res = 0;
            for (int i = 0; i <= numbers; i++)
                res += 2* (long)Math.Pow(5, i);

            return res;
        }

        if (rest - Math.Pow(5, numbrs) < 0 && (rest <= -Max(numbrs - 1) && Math.Abs(rest + 2 * (long)Math.Pow(5, numbrs)) <= Max(numbrs - 1)) || (rest - Math.Pow(5, numbrs) > 0 && (rest >= Max(numbrs - 1) && Math.Abs(rest - 2 * (long)Math.Pow(5, numbrs)) <= Max(numbrs - 1))))
        {
            res.Add('2');
            rest -= 2 * (long)Math.Pow(5, numbrs);
        }
        else
        {
            res.Add('1');
            rest -= (long)Math.Pow(5, numbrs);
        }

        for (int i = numbrs - 1; i >= 0; i--)
        {
            if (rest < 0)
            {
                if (rest <= - Max(i - 1) && Math.Abs(rest + 2 * (long)Math.Pow(5, i)) <= Max(i - 1))
                {
                    res.Add('=');
                    rest += 2 * (long)Math.Pow(5, i);
                }

                else if (Math.Abs(rest + (long)Math.Pow(5, i)) <= Max(i - 1))
                {
                    res.Add('-');
                    rest += (long)Math.Pow(5, i);
                }

                else
                {
                    res.Add('0');
                }
            }
            else if (rest > 0)
            {
                if (rest >= Max(i - 1) && Math.Abs(rest - 2 * (long)Math.Pow(5, i)) <= Max(i - 1))
                {
                    res.Add('2');
                    rest -= 2 * (long)Math.Pow(5, i);
                }

                else if (Math.Abs(rest - (long)Math.Pow(5, i)) <= Max(i - 1))
                {
                    res.Add('1');
                    rest -= (long)Math.Pow(5, i);
                }

                else
                {
                    res.Add('0');
                }

            }
            else
            {
                res.Add('0');
            }


        }

        return new string(res.ToArray());
    }

    static int BestThree(string[] input)
    {
        return 0;
    }
}
