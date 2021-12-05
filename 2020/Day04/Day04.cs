using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    public class Day04
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsStrings(Utils.GetInputPath(2020, 4));

            Console.WriteLine(CountPassports(input, IsAPassport));

            Console.WriteLine(CountPassports(input, IsValidPassport));
        }

        private static int CountPassports(string[] input, Func<string[], bool> validation)
        {
            int validPassports = 0;

            List<string> passport = new List<string>();
            foreach (string line in input)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    passport.Add(line);
                }
                else
                {
                    if (validation(passport.ToArray()))
                        validPassports++;
                    passport = new List<string>();
                }

                if (line == input.Last())
                    if (validation(passport.ToArray()))
                        validPassports++;

            }
            return validPassports;
        }

        private static bool IsValidPassport(string[] lines)
        {
            try
            {
                Dictionary<string, string> passport = new Dictionary<string, string>();

                if (IsAPassport(lines))
                {
                    foreach (string line in lines)
                    {
                        string[] splitLine = line.Split(' ');
                        foreach (string field in splitLine)
                            passport.Add(field.Split(':')[0], field.Split(':')[1]);
                    }

                    //byr
                    if (passport["byr"].Length != 4)
                        return false;
                    int byr = int.Parse(passport["byr"]);
                    if (byr < 1920 || byr > 2020)
                        return false;

                    //iyr
                    if (passport["iyr"].Length != 4)
                        return false;
                    int iyr = int.Parse(passport["iyr"]);
                    if (iyr < 2010 || iyr > 2020)
                        return false;

                    //eyr
                    if (passport["eyr"].Length != 4)
                        return false;
                    int eyr = int.Parse(passport["eyr"]);
                    if (eyr < 2020 || eyr > 2030)
                        return false;

                    //hgt
                    string hgt = passport["hgt"];
                    int height = 0;
                    if (hgt.EndsWith("cm"))
                    {
                        height = int.Parse(hgt.Split(new string[] { "cm" }, StringSplitOptions.None)[0]);
                        if (height < 150 || height > 193)
                            return false;
                    }
                    else if (hgt.EndsWith("in"))
                    {
                        height = int.Parse(hgt.Split(new string[] { "in" }, StringSplitOptions.None)[0]);
                        if (height < 59 || height > 76)
                            return false;
                    }
                    else
                        return false;

                    //hcl
                    string hcl = passport["hcl"];
                    if (hcl.Length != 7)
                        return false;
                    if (!hcl.StartsWith("#"))
                        return false;
                    Regex r = new Regex("[0-9]|[a-f]");
                    foreach (char character in hcl.Split('#')[1])
                        if (!r.IsMatch(character.ToString()))
                            return false;

                    //ecl
                    string[] eyeColors = { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
                    if (!eyeColors.Contains(passport["ecl"]))
                        return false;

                    //pid
                    if (passport["pid"].Length != 9)
                        return false;
                    r = new Regex("[0-9]");
                    foreach (char character in passport["pid"])
                        if (!r.IsMatch(character.ToString()))
                            return false;

                    //cid
                    // Ignored.

                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool IsAPassport(string[] lines)
        {
            string[] neededFields = { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
            List<string> containedFields = new List<string>();

            foreach (string line in lines)
                foreach (string field in neededFields)
                    if (line.Contains(field))
                        if (!containedFields.Contains(field))
                            containedFields.Add(field);

            foreach (string field in neededFields)
                if (!containedFields.Contains(field))
                    return false;

            return true;
        }
    }
}