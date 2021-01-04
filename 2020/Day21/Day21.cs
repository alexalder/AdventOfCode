using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day21
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsStrings(Utils.GetInputPath(2020, 21));

            Console.WriteLine(FindSafeIngredientsAppearances(input));

            Console.WriteLine(FindAllergensList(input));
        }

        private static int FindSafeIngredientsAppearances(string[] input)
        {
            List<string> ingredients = ParseFoodList(input).Values.SelectMany(x => x).ToList();

            Dictionary<string, List<string>> allergensToIngredients = FindPossibleAllergens(input);

            int res = 0;

            List<string> problematicIngredients = new List<string>();
            allergensToIngredients.Values.SelectMany(x => x).ToList().ForEach(x =>
            {
                if (!problematicIngredients.Contains(x))
                    problematicIngredients.Add(x);
            });

            foreach (var ingredient in ingredients)
                if (!problematicIngredients.Contains(ingredient))
                    res++;

            return res;
        }

        private static string FindAllergensList(string[] input)
        {
            Dictionary<string, List<string>> possibleAllergens = FindPossibleAllergens(input);

            Dictionary<string, string> allergensDictionary = new Dictionary<string, string>();

            while (possibleAllergens.Count > 0)
            {
                foreach (var value in possibleAllergens)
                {
                    if (value.Value.Count == 1)
                    {
                        allergensDictionary.Add(value.Key, value.Value[0]);
                        foreach (var otherValue in possibleAllergens)
                        {
                            if (value.Key == otherValue.Key)
                                continue;
                            if (otherValue.Value.Contains(value.Value[0]))
                            {
                                otherValue.Value.Remove(value.Value[0]);
                            }
                        }
                        possibleAllergens.Remove(value.Key);
                        break;
                    }
                }
            }

            allergensDictionary = allergensDictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            string res = string.Join(",", allergensDictionary.Values);
            return res;
        }

        private static Dictionary<List<string>, List<string>> ParseFoodList(string[] input)
        {
            Dictionary<List<string>, List<string>> foodList = new Dictionary<List<string>, List<string>>();

            foreach (string line in input)
            {
                bool secondPart = false;
                List<string> ingredients = new List<string>();
                List<string> allergens = new List<string>();
                foreach (string word in line.Split(new char[] { ' ', ',', ')', '(' }, StringSplitOptions.None))
                {
                    if (string.IsNullOrEmpty(word))
                        continue;
                    if (word == "contains")
                    {
                        secondPart = true;
                        continue;
                    }
                    if (secondPart)
                    {
                        allergens.Add(word);
                    }
                    else
                    {
                        ingredients.Add(word);
                    }

                }
                foodList.Add(allergens, ingredients);
            }

            return foodList;
        }

        private static Dictionary<string, List<string>> FindPossibleAllergens(string[] input)
        {
            Dictionary<List<string>, List<string>> foodList = ParseFoodList(input);

            Dictionary<string, List<string>> possibleAllergens = new Dictionary<string, List<string>>();

            List<string> allergensFound = new List<string>();
            foodList.Keys.SelectMany(x => x).ToList().ForEach(x =>
            {
                if (!allergensFound.Contains(x))
                    allergensFound.Add(x);
            });
            foreach (var allergen in allergensFound)
            {
                List<string> ingredients = new List<string>();
                foreach (var value in foodList)
                {
                    if (value.Key.Contains(allergen))
                    {
                        if (ingredients.Count == 0)
                            value.Value.ForEach(x => ingredients.Add(x));
                        else
                        {
                            ingredients = ingredients.Intersect(value.Value).ToList();
                        }

                    }
                }
                possibleAllergens.Add(allergen, ingredients);
            }

            return possibleAllergens;
        }
    }
}