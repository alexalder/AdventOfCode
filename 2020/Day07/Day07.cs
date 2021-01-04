using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day07
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsStrings(Utils.GetInputPath(2020, 7));
            
            Console.WriteLine(FindContainingBagsNumber(input));

            Console.WriteLine(FindContainedBagsNumber(input));
        }

        private static Dictionary<string, string[]> CreateRules(string[] input)
        {
            Dictionary<string, string[]> rules = new Dictionary<string, string[]>();

            foreach (string line in input)
            {
                var trimmedLine = line.Trim(new char[] { '.' });
                string key = trimmedLine.Split(new string[] { " contain " }, StringSplitOptions.None)[0];
                string[] children = trimmedLine.Split(new string[] { " contain " }, StringSplitOptions.None)[1]
                    .Split(new string[] { ", " }, StringSplitOptions.None).Skip(0).ToArray();

                rules.Add(key, children);
            }

            return rules;
        }

        private static int FindContainingBagsNumber(string[] input)
        {
            Dictionary<string, string[]> rules = CreateRules(input);

            string targetBag = "shiny gold";

            List<string> result = new List<string>();

            int addedBags = 0;
            do
            {
                addedBags = 0;
                foreach (KeyValuePair<string, string[]> entry in rules)
                {
                    string key = entry.Key.Split(new string[] { " bag" }, StringSplitOptions.None)[0];
                    foreach (string bag in entry.Value)
                    {
                        string value = GetBagType(bag);
                        if (value.Contains(targetBag) || result.Contains(value))
                        {
                            if (!result.Contains(key))
                            {
                                result.Add(key);
                                addedBags++;
                            }

                        }
                    }
                }
            }
            while (addedBags > 0);

            return result.Count;
        }

        private static int FindContainedBagsNumber(string[] input)
        {
            Dictionary<string, string[]> rules = CreateRules(input);

            string targetBag = "shiny gold";
            return ContainedBags(rules, targetBag) - 1;
        }

        private static int ContainedBags(Dictionary<string, string[]> rules, string bag)
        {
            int res = 0;
            string bagString = bag + " bags";
            string[] searchResults = rules[bagString];
            if (searchResults.Contains("no other bags"))
                res = 1;
            else
            {
                foreach (string result in searchResults)
                    res += GetBagsNumber(result) * ContainedBags(rules, GetBagType(result));
                res += 1;
            }
            return res;
        }

        private static string GetBagType(string bag)
        {
            string res = bag.Substring(2).Split(new string[] { " bag" }, StringSplitOptions.None)[0];
            return res;
        }

        private static int GetBagsNumber(string bag)
        {
            int res = int.Parse(bag[0].ToString());
            return res;
        }
    }
}