using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day16
{
    static string startingValve = "AA";
    static readonly int totalMinutes = 30;
    static readonly int lessMinutes = 26;

    static List<Valve> valves = new();
    static Dictionary<string, Valve> valveDict = new();
    static Dictionary<(string, string), int> distances = new();

    public static void Run()
    {
        var input = Utils.ReadInputAsStrings(Utils.GetInputPath());

        Console.WriteLine(OpenValves(input));

        Console.WriteLine(OpenValvesDuo(input));
    }

    static List<Valve> ReadInput(string[] input)
    {

        foreach (var line in input)
        {
            var values = line.Split(new string[] { "Valve ", " has flow rate=", "; tunnels lead to valves ", "; tunnel leads to valve " }, StringSplitOptions.RemoveEmptyEntries);
            valves.Add(new Valve(values[0], int.Parse(values[1]), values[2].Split(", ")));
        }

        foreach (var val in valves)
            valveDict.Add(val.label, val);

        return valves;
    }

    private static int OpenValves(string[] input)
    {
        var valves = ReadInput(input);

        string curValve = startingValve;
        int currentPressure = 0;
        int totalPressure = 0;

        string[] importantValves = valves.Where(x => x.flowRate > 0).Select(x => x.label).ToArray();

        return SolveProblem(importantValves, currentPressure, totalPressure, curValve, totalMinutes);
    }

    private static int OpenValvesDuo(string[] input)
    {
        string curValve = startingValve;
        string elephantCurValve = startingValve;

        string[] importantValves = valves.Where(x => x.flowRate > 0).Select(x => x.label).ToArray();

        List<Problem> attempts = new();
        List<Problem> newAttempts = new();
        List<Problem> results = new();

        attempts.Add(new Problem(importantValves, curValve, elephantCurValve, lessMinutes));


        int cycles = 0;
        while (attempts.Any(x => x.minutesLeft > 0))
        {
            Console.WriteLine(cycles++);
            foreach (Problem attempt in attempts.Where(x => x.minutesLeft > 0))
            {
                // All moving
                if (attempt.waitTime > 0 && attempt.elephantWaitTime > 0)
                {
                    var waiting = Math.Min(attempt.waitTime, attempt.elephantWaitTime);
                    var newAttempt = attempt;
                    newAttempt.totalPressure += waiting * attempt.currentPressure;
                    newAttempt.waitTime -= waiting;
                    newAttempt.elephantWaitTime -= waiting;
                    newAttempt.minutesLeft -= waiting;

                    newAttempts.Add(newAttempt);
                }
                // Nobody moving
                else if (attempt.waitTime == 0 && attempt.elephantWaitTime == 0)
                {

                    var newAttempt = attempt;

                    if (attempt.openingValve)
                    {
                        newAttempt.closedValves = newAttempt.closedValves.Where(x => x != attempt.curPosition).ToArray();
                        newAttempt.openingValve = false;
                        newAttempt.currentPressure += valveDict[attempt.curPosition].flowRate;
                    }

                    if (attempt.elephantOpeningValve)
                    {
                        newAttempt.closedValves = newAttempt.closedValves.Where(x => x != attempt.elephantCurPosition).ToArray();
                        newAttempt.elephantOpeningValve = false;
                        newAttempt.currentPressure += valveDict[attempt.elephantCurPosition].flowRate;
                    }

                    // Valves all closed
                    if (newAttempt.closedValves.Length == 0)
                    {
                        // This won't come up
                        //throw new Exception();
                        newAttempt.totalPressure += newAttempt.currentPressure * newAttempt.minutesLeft;
                        newAttempt.minutesLeft = 0;
                        newAttempts.Add(newAttempt);
                    }

                    // 1 valve left
                    if (newAttempt.closedValves.Length == 1)
                    {
                        var distance = FindDistance(attempt.curPosition, newAttempt.closedValves[0]);
                        var elephantDistance = FindDistance(attempt.elephantCurPosition, newAttempt.closedValves[0]);

                        if (distance < elephantDistance && distance + 2 <= newAttempt.minutesLeft)
                        {
                            newAttempt.curPosition = newAttempt.closedValves[0];
                            newAttempt.waitTime = distance + 1;
                            newAttempt.openingValve = true;

                            newAttempts.Add(newAttempt);
                        }
                        else if (elephantDistance < distance && elephantDistance + 2 <= newAttempt.minutesLeft)
                        {
                            newAttempt.elephantCurPosition = newAttempt.closedValves[0];
                            newAttempt.elephantWaitTime = elephantDistance + 1;
                            newAttempt.elephantOpeningValve = true;

                            newAttempts.Add(newAttempt);
                        }
                    }
                    // More valves left
                    else
                    {
                        var possibleValves = newAttempt.closedValves.Where(x => FindDistance(newAttempt.curPosition, x) + 2 <= newAttempt.minutesLeft);

                        var elephantPossibleValves = newAttempt.closedValves.Where(x => FindDistance(newAttempt.elephantCurPosition, x) + 2 <= newAttempt.minutesLeft);

                        // All valves are too far (Non so se ha davvero senso? Fixato?)
                        if (possibleValves.Count() == 0 && elephantPossibleValves.Count() == 0)
                        {
                            newAttempt.totalPressure += newAttempt.minutesLeft * newAttempt.currentPressure;
                            newAttempt.minutesLeft -= newAttempt.minutesLeft;

                            newAttempts.Add(newAttempt);
                        }
                        // Valves within reach
                        else
                        {
                            foreach (var possibleValve in possibleValves)
                            {
                                var distance = FindDistance(attempt.curPosition, possibleValve);

                                foreach (var elephantPossibleValve in elephantPossibleValves)
                                {
                                    var elephantDistance = FindDistance(attempt.elephantCurPosition, elephantPossibleValve);

                                    if (possibleValve != elephantPossibleValve)
                                    {
                                        var newNewAttempt = newAttempt;

                                        newNewAttempt.curPosition = possibleValve;
                                        newNewAttempt.elephantCurPosition = elephantPossibleValve;
                                        newNewAttempt.waitTime = distance + 1;
                                        newNewAttempt.elephantWaitTime = elephantDistance + 1;
                                        newNewAttempt.openingValve = true;
                                        newNewAttempt.elephantOpeningValve = true;

                                        newAttempts.Add(newNewAttempt);
                                    }
                                }
                            }

                            foreach (var possibleValve in possibleValves)
                            {
                                var distance = FindDistance(attempt.curPosition, possibleValve);

                                var newNewAttempt = newAttempt;

                                newNewAttempt.curPosition = possibleValve;
                                newNewAttempt.waitTime = distance + 1;
                                newNewAttempt.openingValve = true;

                                newAttempts.Add(newNewAttempt);
                            }

                            foreach (var elephantPossibleValve in elephantPossibleValves)
                            {
                                var elephantDistance = FindDistance(attempt.elephantCurPosition, elephantPossibleValve);

                                var newNewAttempt = newAttempt;

                                newNewAttempt.elephantCurPosition = elephantPossibleValve;
                                newNewAttempt.elephantWaitTime = elephantDistance + 1;
                                newNewAttempt.elephantOpeningValve = true;

                                newAttempts.Add(newNewAttempt);
                            }
                        }

                    }
                }
                // Someone's moving
                // I am moving
                else if (attempt.waitTime == 0)
                {
                    var newAttempt = attempt;

                    if (attempt.openingValve)
                    {
                        newAttempt.closedValves = newAttempt.closedValves.Where(x => x != attempt.curPosition).ToArray();
                        newAttempt.openingValve = false;
                        newAttempt.currentPressure += valveDict[attempt.curPosition].flowRate;
                    }

                    var possibleValves = newAttempt.closedValves.Where(x => x != attempt.elephantCurPosition && FindDistance(attempt.curPosition, x) + 2 <= newAttempt.minutesLeft);

                    foreach (var possibleValve in possibleValves)
                    {
                        var distance = FindDistance(attempt.curPosition, possibleValve);

                        var newNewAttempt = newAttempt;

                        newNewAttempt.curPosition = possibleValve;
                        newNewAttempt.waitTime = distance + 1;
                        newNewAttempt.openingValve = true;

                        newAttempts.Add(newNewAttempt);
                    }

                    var waiting = attempt.elephantWaitTime;

                    newAttempt.totalPressure += waiting * newAttempt.currentPressure;
                    newAttempt.elephantWaitTime -= waiting;
                    newAttempt.minutesLeft -= waiting;

                    newAttempts.Add(newAttempt);
                }
                // Elephant is moving
                else if (attempt.elephantWaitTime == 0)
                {
                    var newAttempt = attempt;

                    if (attempt.elephantOpeningValve)
                    {
                        newAttempt.closedValves = newAttempt.closedValves.Where(x => x != attempt.elephantCurPosition).ToArray();
                        newAttempt.elephantOpeningValve = false;
                        newAttempt.currentPressure += valveDict[attempt.elephantCurPosition].flowRate;
                    }

                    var elephantPossibleValves = newAttempt.closedValves.Where(x => x != attempt.curPosition && FindDistance(attempt.elephantCurPosition, x) + 2 <= newAttempt.minutesLeft);

                    foreach (var elephantPossibleValve in elephantPossibleValves)
                    {
                        var elephantDistance = FindDistance(attempt.elephantCurPosition, elephantPossibleValve);

                        var newNewAttempt = newAttempt;

                        newNewAttempt.elephantCurPosition = elephantPossibleValve;
                        newNewAttempt.elephantWaitTime = elephantDistance + 1;
                        newNewAttempt.elephantOpeningValve = true;

                        newAttempts.Add(newNewAttempt);
                    }

                    var waiting = attempt.waitTime;

                    newAttempt.totalPressure += waiting * newAttempt.currentPressure;
                    newAttempt.waitTime -= waiting;
                    newAttempt.minutesLeft -= waiting;

                    newAttempts.Add(newAttempt);
                }
            }
            attempts.RemoveAll(x => x.minutesLeft > 0);
            results.AddRange(attempts);
            attempts.Clear();

            attempts.AddRange(newAttempts);
            int cutoff = 1000000;
            if (attempts.Count >= cutoff)
            {
                attempts = attempts.OrderByDescending(x => x.currentPressure / (lessMinutes - x.minutesLeft)).ToList();
                attempts.RemoveRange(cutoff, attempts.Count - cutoff);
            }

            newAttempts = new();
        }

        var best = results.MaxBy(attempt => attempt.totalPressure);

        return best.totalPressure;
    }

    static int SolveProblem(string[] closedValves, int currentPressure, int totalPressure, string curPosition, int minutesLeft)
    {
        string[] newClosedValves = { };

        void MoveToValve(string valve)
        {
            minutesLeft -= FindDistance(curPosition, closedValves[0]);
            totalPressure += FindDistance(curPosition, closedValves[0]) * currentPressure;
        }

        void OpenValve(string valve)
        {
            minutesLeft -= 1;
            totalPressure += currentPressure;

            currentPressure += valves.Where(x => x.label == closedValves[0]).First().flowRate;

            var temp = closedValves.ToList();
            temp.Remove(closedValves[0]);
            newClosedValves = temp.ToArray();
        }

        if (closedValves.Length == 1)
        {
            var distance = FindDistance(curPosition, closedValves[0]);
            if (minutesLeft > distance + 2)
            {
                MoveToValve(closedValves[0]);

                OpenValve(closedValves[0]);
            }
            while (minutesLeft > 0)
            {
                totalPressure += currentPressure;
                minutesLeft -= 1;
            }
            return totalPressure;
        }
        else
        {
            List<int> results = new();
            for (int i = 0; i < closedValves.Length; i++)
            {
                var targetValve = closedValves[i];

                var distance = FindDistance(curPosition, targetValve);

                if (minutesLeft > distance + 2)
                {
                    results.Add(SolveProblem(closedValves.Where(x=> x != targetValve).ToArray(), currentPressure + valves.Where(x => x.label == closedValves[i]).First().flowRate, totalPressure + (FindDistance(curPosition, closedValves[i]) + 1) * currentPressure, targetValve, minutesLeft - 1 - FindDistance(curPosition, closedValves[i])));
                }
            }
            if (results.Count == 0)
            {
                while (minutesLeft > 0)
                {
                    totalPressure += currentPressure;
                    minutesLeft -= 1;
                }
                return totalPressure;
            }
            return results.Max();
        }
        
    }

    static int FindDistance(string current, string target)
    {
        if (distances.ContainsKey((current, target)))
            return distances[(current, target)];

        foreach (var item in FindDistanceInside(valves, current))
            if (!distances.ContainsKey((current, item.Key)))
                distances[(current, item.Key)] = item.Value;

        return distances[(current, target)];
    }

    static Dictionary<string, int> FindDistanceInside(List<Valve> valves, string current)
    {
        Dictionary<string, int> result = new();

        result.Add(current, 0);

        while (result.Count < valves.Count)
        {
            List<(string, int)> addPair = new();
            foreach (var valve in result)
            {
                foreach (var nearValve in valves.Where(x => x.label == valve.Key).First().tunnels)
                {
                    addPair.Add((nearValve, valve.Value + 1));
                }
            }
            foreach (var pair in addPair)
                if (!result.ContainsKey(pair.Item1))
                    result.Add(pair.Item1, pair.Item2);
            addPair.Clear();
        }

        return result;
    }

    class Valve
    {
        internal string label;
        internal int flowRate;
        internal string[] tunnels;

        internal Valve(string label, int flowRate, string[] tunnels)
        {
            this.label = label;
            this.flowRate = flowRate;
            this.tunnels = tunnels;
        }
    }

    struct Problem
    {
        internal string[] closedValves;
        internal string curPosition;
        internal string elephantCurPosition;
        internal int minutesLeft;
        internal int currentPressure = 0;
        internal int totalPressure = 0;
        internal int waitTime = 0;
        internal int elephantWaitTime = 0;
        internal bool openingValve = false;
        internal bool elephantOpeningValve = false;
        public Problem(string[] closedValves, string curPosition, string elephantCurPosition, int minutesLeft)
        {
            this.closedValves = closedValves;
            this.curPosition = curPosition;
            this.elephantCurPosition = elephantCurPosition;
            this.minutesLeft = minutesLeft;
        }
    }
}
