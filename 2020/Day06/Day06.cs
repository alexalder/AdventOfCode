using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day06
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsStrings(Utils.GetInputPath(2020, 6));
            var groups = Utils.SplitInput(input, "");

            Console.WriteLine(GetQuestionsAnsweredByAnyone(groups));
            Console.WriteLine(GetQuestionsAnsweredByEveryone(groups));
        }

        private static int GetQuestionsAnsweredByAnyone(string[][] answers)
        {
            int questionCount = 0;

            foreach (var group in answers)
                questionCount += GetQuestionsAnsweredByAnyone(group);

            return questionCount;
        }

        private static int GetQuestionsAnsweredByAnyone(string[] answers)
        {
            List<char> res = new List<char>();
            foreach (string answer in answers)
                foreach (char letter in answer)
                    if (!res.Contains(letter))
                        res.Add(letter);

            return res.Count;
        }

        private static int GetQuestionsAnsweredByEveryone(string[][] answers)
        {
            int questionCount = 0;

            foreach (var group in answers)
                questionCount += GetQuestionsAnsweredByEveryone(group);

            return questionCount;
        }

        private static int GetQuestionsAnsweredByEveryone(string[] answers)
        {
            List<char> res = new List<char>();

            foreach (char letter in answers[0])
            {
                bool addLetter = true;
                foreach (string answer in answers)
                    if (!answer.Contains(letter))
                        addLetter = false;

                if (addLetter)
                    res.Add(letter);
            }

            return res.Count;
        }
    }
}