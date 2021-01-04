using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020
{
    public class Day14
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsStrings(Utils.GetInputPath(2020, 14));

            Console.WriteLine(RunProgram(input));

            Console.WriteLine(RunProgram(input, 2));
        }

        private static double RunProgram(string[] input, int version = 1)
        {
            string mask = "";

            Dictionary<double, string> memory = new Dictionary<double, string>();

            foreach (string line in input)
            {
                if (line.StartsWith("mask"))
                    mask = line.Split(new string[] { " = " }, StringSplitOptions.None)[1];
                else
                {
                    long baseAddress = long.Parse(line.Split(new string[] { "mem[" }, StringSplitOptions.None)[1].Split(new string[] { "] = " }, StringSplitOptions.None)[0]);
                    long value = long.Parse(line.Split(new string[] { "mem[" }, StringSplitOptions.None)[1].Split(new string[] { "] = " }, StringSplitOptions.None)[1]);
                    if (version == 1)
                    {
                        string binary = ToBinary(value, mask);
                        memory[baseAddress] = binary;
                    }
                    else
                    {
                        double[] addresses = ToBinary2(baseAddress, mask);
                        foreach (double address in addresses)
                            memory[address] = ToBinary(value);
                    }
                }
            }

            double total = 0;
            foreach (var stored in memory)
                total += ToDouble(stored.Value);

            return total;
        }

        private static string ToBinary(long value, string mask = "")
        {
            string binary = Convert.ToString(value, 2).PadLeft(36, '0');
            StringBuilder res = new StringBuilder();
            if (string.IsNullOrEmpty(mask))
                return binary;
            for (int i = 0; i < mask.Length; i++)
            {
                if (mask[i] == 'X')
                    res.Append(binary[i]);
                else
                    res.Append(mask[i]);
            }
            return res.ToString();
        }

        private static double[] ToBinary2(long value, string mask)
        {
            string binary = Convert.ToString(value, 2).PadLeft(36, '0');
            List<StringBuilder> res = new List<StringBuilder>();
            res.Add(new StringBuilder());

            for (int digit = 0; digit < mask.Length; digit++)
            {
                if (mask[digit] == 'X')
                {
                    int curSize = res.Count;
                    for (int result = 0; result < curSize; result++)
                    {
                        StringBuilder newStringBuilder = new StringBuilder(res[result].ToString());
                        newStringBuilder.Append("1");
                        res.Add(newStringBuilder);
                        res[result].Append("0");
                    }
                }
                else
                {
                    if (mask[digit] == '1')
                        foreach (StringBuilder sb in res)
                            sb.Append("1");
                    else
                        foreach (StringBuilder sb in res)
                            sb.Append(binary[digit]);
                }
            }
            return res.ConvertAll(x => x.ToString()).ConvertAll(x => ToDouble(x)).ToArray();
        }

        private static double ToDouble(string binary)
        {
            double res = 0;
            for (int i = 0; i < binary.Length; i++)
            {
                res += double.Parse(binary[i].ToString()) * Math.Pow(2, 35 - i);
            }
            return res;
        }
    }
}