using System;

namespace AdventOfCode2020
{
    public class Day02
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsStrings(Utils.GetInputPath(2020, 02));

            Console.WriteLine(CheckPasswords(input));

            Console.WriteLine(CheckPasswords(input, true));
        }

        private static int CheckPasswords(string[] input, bool positions = false)
        {
            int validPasswords = 0;
            foreach (string line in input)
            {
                string times = line.Split(' ')[0];
                int minimum = int.Parse(times.Split('-')[0]) - 1;
                int maximum = int.Parse(times.Split('-')[1]) - 1;

                string letterDirty = line.Split(' ')[1];
                char letter = letterDirty[0];

                string password = line.Split(' ')[2];

                if (positions)
                {
                    bool first = false;
                    bool second = false;

                    if (minimum < password.Length)
                        if (password[minimum] == letter)
                            first = true;

                    if (maximum < password.Length)
                        if (password[maximum] == letter)
                            second = true;

                    if (first ^ second)
                        validPasswords++;
                }

                else
                {
                    int containedLetter = 0;
                    foreach (char character in password)
                        if (character == letter)
                            containedLetter++;

                    if (containedLetter <= maximum && containedLetter >= minimum)
                        validPasswords++;
                }
            }

            return validPasswords;
        }
    }
}
