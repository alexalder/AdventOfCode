using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day19
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsStrings(Utils.GetInputPath(2020, 19));

            Console.WriteLine(FindMatchingMessages(input));

            Console.WriteLine(FindMatchingMessagesLooping(input));
        }

        private static int FindMatchingMessagesLooping(string[] input)
        {
            var messages = Utils.SplitInput(input, "")[1];

            var rules = GetLoopingRules(input);

            var quarantadue = UnpackRule(rules[42], rules);
            var trentuno = UnpackRule(rules[31], rules);

            int matching = 0;
            foreach (string message in messages)
            {
                if (message == "aabaaaaabaaababaaaabbbbbaababaaabbabbbba")
                    Console.Write("");
                string subString = message;
                bool isQuarantadue = true;
                int howManyQuarantadue = 0;
                while (subString.Length > 0 && isQuarantadue)
                {
                    isQuarantadue = false;
                    foreach (string quarnt in quarantadue)
                        if (subString.StartsWith(quarnt))
                        {
                            howManyQuarantadue++;
                            subString = subString.Substring(quarnt.Length);
                            isQuarantadue = true;
                        }
                }
                if (howManyQuarantadue < 2)
                    continue;
                bool isTrentuno = true;
                int howManyTrentuno = 0;
                if (subString.Length == 0)
                    continue;
                while (subString.Length > 0 && isTrentuno)
                {
                    isTrentuno = false;
                    foreach (string trent in trentuno)
                        if (subString.StartsWith(trent))
                        {
                            howManyTrentuno++;
                            subString = subString.Substring(trent.Length);
                            isTrentuno = true;
                        }
                }
                if (howManyTrentuno < 1)
                    continue;
                if (subString.Length == 0)
                    if (howManyQuarantadue > howManyTrentuno)
                        matching++;
            }
            return matching;
        }

        private static Dictionary<int, string> GetLoopingRules(string[] input)
        {
            Dictionary<int, string> rulesDict = new Dictionary<int, string>();
            var rulesStrings = Utils.SplitInput(input, "")[0];

            foreach (string line in rulesStrings)
            {
                if (line.StartsWith("0:") || line.StartsWith("8:") || line.StartsWith("11:"))
                    continue;
                rulesDict.Add(int.Parse(line.Split(':')[0]), line.Split(new char[] { ' ' }, 2)[1]);
            }

            return rulesDict;
        }

        private static int FindMatchingMessages(string[] input)
        {
            var messages = Utils.SplitInput(input, "")[1];

            var rules = GetRules(input);

            int matching = 0;
            foreach (string message in messages)
            {
                if (rules.Contains(message))
                    matching++;

            }
            return matching;
        }

        private static string[] GetRules(string[] input)
        {
            Dictionary<int, string> rulesDict = new Dictionary<int, string>();
            var rulesStrings = Utils.SplitInput(input, "")[0];

            foreach (string line in rulesStrings)
            {
                rulesDict.Add(int.Parse(line.Split(':')[0]), line.Split(new char[] { ' ' }, 2)[1]);
            }

            return UnpackRule(rulesDict[0], rulesDict);
        }

        private static string[] UnpackRule(string rule, Dictionary<int, string> rulesDict)
        {
            List<List<string>> subRules = new List<List<string>>();
            subRules.Add(new List<string>());
            int curRule = 0;
            List<string> res = new List<string>();

            foreach (string element in rule.Split(' '))
            {
                if (element == "|")
                {
                    subRules.Add(new List<string>());
                    curRule++;
                }
                else if (element.StartsWith("\""))
                    return new string[] { element.Substring(1, 1).ToString() };
                else
                {
                    string[] elementRules = UnpackRule(rulesDict[int.Parse(element)], rulesDict);

                    if (subRules[curRule].Count == 0)
                    {
                        foreach (string elementRule in elementRules)
                        {
                            subRules[curRule].Add(elementRule);
                        }
                    }
                    else
                    {
                        var tempArray = subRules[curRule].ToArray();
                        subRules[curRule] = new List<string>();
                        foreach (string elementRule in elementRules)
                        {
                            foreach (var oldRule in tempArray)
                                subRules[curRule].Add(oldRule + elementRule);
                        }
                    }
                }
            }

            foreach (var rulesList in subRules)
                foreach (var ruleInList in rulesList)
                    res.Add(ruleInList);

            return res.ToArray();
        }
    }
}