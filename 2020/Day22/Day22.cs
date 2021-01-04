using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public class Day22
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsStrings(Utils.GetInputPath(2020, 22));

            Console.WriteLine(GetScore(Combat(input)));

            Console.WriteLine(GetScore(StartRecursiveCombat(input)));
        }

        public static List<int> ParseDecks(string[] input, int deckNumber)
        {
            List<int> deck = new List<int>();
            var decks = Utils.SplitInput(input, "");

            if (deckNumber == 1)
            {
                foreach (string line in decks[0].Skip(1))
                {
                    deck.Add(int.Parse(line));
                }
            }
            else if (deckNumber == 2)
            {
                foreach (string line in decks[1].Skip(1))
                {
                    deck.Add(int.Parse(line));
                }
            }

            return deck;
        }

        private static List<int> StartRecursiveCombat(string[] input)
        {
            List<int> deck1 = ParseDecks(input, 1);
            List<int> deck2 = ParseDecks(input, 2);

            return RecursiveCombat(deck1, deck2);
        }

        private static List<int> RecursiveCombat(List<int> deck1, List<int> deck2)
        {
            List<string> gameStates = new List<string>();
            int round = 0;
            while (deck1.Count > 0 && deck2.Count > 0)
            {
                round++;
                if (gameStates.Contains(GameState(deck1, deck2)))
                    return deck1;

                gameStates.Add(GameState(deck1, deck2));

                if (deck1.Count <= deck1[0] || deck2.Count <= deck2[0])
                {
                    if (deck1[0] > deck2[0])
                        Player1WinsRound(deck1, deck2);
                    else if (deck1[0] < deck2[0])
                        Player2WinsRound(deck1, deck2);
                }
                else
                {
                    List<int> tempDeck1 = deck1.Skip(1).Take(deck1[0]).ToList();
                    List<int> tempDeck2 = deck2.Skip(1).Take(deck2[0]).ToList();
                    List<int> tempWinning = RecursiveCombat(tempDeck1, tempDeck2);
                    if (tempWinning == tempDeck1)
                        Player1WinsRound(deck1, deck2);
                    else
                        Player2WinsRound(deck1, deck2);
                }
            }
            return (deck1.Count > 0) ? deck1 : deck2;
        }

        private static List<int> Combat(string[] input)
        {
            List<int> deck1 = ParseDecks(input, 1);
            List<int> deck2 = ParseDecks(input, 2);

            while (deck1.Count > 0 && deck2.Count > 0)
            {
                if (deck1[0] > deck2[0])
                {
                    Player1WinsRound(deck1, deck2);
                }
                else if (deck1[0] < deck2[0])
                {
                    Player2WinsRound(deck1, deck2);
                }
            }
            return (deck1.Count > 0) ? deck1 : deck2;
        }

        private static int GetScore(List<int> winningDeck)
        {
            int score = 0;

            winningDeck.Reverse();

            for (int card = 0; card < winningDeck.Count; card++)
            {
                score += (card + 1) * winningDeck[card];
            }

            return score;
        }

        public static void Player1WinsRound(List<int> deck1, List<int> deck2)
        {
            deck1.Add(deck1[0]);
            deck1.Remove(deck1[0]);
            deck1.Add(deck2[0]);
            deck2.Remove(deck2[0]);
        }

        public static void Player2WinsRound(List<int> deck1, List<int> deck2)
        {
            deck2.Add(deck2[0]);
            deck2.Remove(deck2[0]);
            deck2.Add(deck1[0]);
            deck1.Remove(deck1[0]);
        }

        public static string GameState(List<int> deck1, List<int> deck2)
        {
            return string.Join("-", deck1) + "o" + string.Join("-", deck2);
        }
    }
}