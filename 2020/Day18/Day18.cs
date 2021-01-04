using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day18
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsStrings(Utils.GetInputPath(2020, 18));

            Console.WriteLine(SolveSheet(input));

            Console.WriteLine(SolveSheet(input, true));
        }

        private static double SolveSheet(string[] input, bool advanced = false)
        {
            double res = 0;

            foreach (string line in input)
                res += SolveMath(line, advanced);

            return res;
        }

        private static double SolveMath(string line, bool advanced = false)
        {
            List<string> problem = new List<string>();

            char[] numbers = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

            for (int i = 0; i < line.Length; i++)
            {
                char symbol = line[i];
                if (symbol == ' ')
                    continue;
                else if (numbers.Contains(symbol))
                {
                    problem.Add(symbol.ToString());
                }
                else if (symbol == '+')
                    problem.Add(symbol.ToString());
                else if (symbol == '*')
                    problem.Add(symbol.ToString());
                else if (symbol == '(')
                {
                    i++;
                    int parenthesis = 1;
                    int j = i;
                    for (; j < line.Length; j++)
                    {
                        symbol = line[j];
                        if (symbol == '(')
                            parenthesis++;
                        else if (symbol == ')')
                            parenthesis--;
                        if (parenthesis == 0)
                        {
                            problem.Add(SolveMath(line.Substring(i, j - i), advanced).ToString());
                            break;
                        }
                    }
                    i = j;
                }

            }

            double res = 0;

            if (advanced)
            {
                List<string> afterAddictions = new List<string>();

                for (int i = 0; i < problem.Count; i++)
                {
                    double sum = 0;
                    while (problem[i] != "*" && i < problem.Count)
                    {
                        if (problem[i] != "+")
                            sum += double.Parse(problem[i]);
                        i++;
                        if (i == problem.Count)
                        {
                            break;
                        }

                    }
                    afterAddictions.Add(sum.ToString());
                    if (i < problem.Count)
                        afterAddictions.Add("*");
                }

                res = 1;

                foreach (string element in afterAddictions)
                {
                    if (element != "*")
                        res *= double.Parse(element);
                }
            }
            else
            {
                string curOperator = "+";
                foreach (string element in problem)
                {
                    if (element == "+")
                        curOperator = element;
                    else if (element == "*")
                        curOperator = element;
                    else
                    {
                        if (curOperator == "+")
                            res += double.Parse(element.ToString());
                        else
                            res *= double.Parse(element.ToString());
                    }
                }
            }
            
            return res;
        }
    }
}