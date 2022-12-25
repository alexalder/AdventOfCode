using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day19
{
    static readonly int maxTime = 24;
    static readonly int moreTime = 32;

    static readonly int desperation = 20;

    static readonly string[] delimiters = new string[] { "Blueprint ", ": Each ore robot costs ", " ore. Each clay robot costs ", " ore. Each obsidian robot costs ", " ore and ", " clay. Each geode robot costs ", " obsidian." };

    public static void Run()
    {
        var input = Utils.ReadInputAsStrings(Utils.GetInputPath());

        //Console.WriteLine(PartOne(input));

        Console.WriteLine(PartTwo(input));
    }

    static int PartOne(string[] input)
    {
        var blueprints = input
            .Select(x => x.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => int.Parse(x)))
            .Select(x => new RobotFactoryBlueprint(x.ToArray()));

        int sumQualityLevels = 0;

        List<Action> bluePrintReadings = new();
        foreach (var blueprint in blueprints)
        {
            bluePrintReadings.Add(() =>
            {
                var geodes = FindMostGeodes(blueprint, maxTime);
                Console.WriteLine("ID " + blueprint.id);
                Console.WriteLine(blueprint.oreRobotOreCost + " " + blueprint.clayRobotOreCost + " " + blueprint.obsidianRobotOreCost + " " + blueprint.obsidianRobotClayCost + " " + blueprint.geodeRobotOreCost + " " + blueprint.geodeRobotObsidianCost);
                Console.WriteLine("Geodes: " + geodes);
                sumQualityLevels += geodes * blueprint.id;
            });
        }
        Parallel.Invoke(bluePrintReadings.ToArray());

        return sumQualityLevels;
    }

    static int PartTwo(string[] input)
    {
        var blueprints = input
                    .Select(x => x.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => int.Parse(x)))
                    .Select(x => new RobotFactoryBlueprint(x.ToArray()))
                    .Take(3);

        int geodesProduct = 1;

        List<Action> bluePrintReadings = new();
        foreach (var blueprint in blueprints)
        {
            bluePrintReadings.Add(() =>
            {
                var geodes = SolveGPT(blueprint, new RobotFactory(), 1);
                Console.WriteLine("ID " + blueprint.id);
                Console.WriteLine(blueprint.oreRobotOreCost + " " + blueprint.clayRobotOreCost + " " + blueprint.obsidianRobotOreCost + " " + blueprint.obsidianRobotClayCost + " " + blueprint.geodeRobotOreCost + " " + blueprint.geodeRobotObsidianCost);
                Console.WriteLine("Geodes: " + geodes);
                geodesProduct *= geodes;
            });
        }
        Parallel.Invoke(bluePrintReadings.ToArray());

        //return SolveGPT(blueprints.First(), new RobotFactory(), 1);

        return geodesProduct;
    }

    static int SolveGPT(RobotFactoryBlueprint blueprint, RobotFactory factory, int timePassed)
    {
        List<int> results = new();
        if (timePassed == moreTime)
            return factory.geodes + factory.geodeRobots;
        else if (timePassed == moreTime - 1)
        {
            var newFactory = factory;
            var mostRobots = Math.Min(factory.ore / blueprint.geodeRobotOreCost, factory.obsidian / blueprint.geodeRobotObsidianCost);
            newFactory.geodeRobots += mostRobots;
            newFactory.geodes += factory.geodeRobots;

            return SolveGPT(blueprint, newFactory, timePassed + 1);
        }
        else
        {
            (int ore, int clay, int obsidian, int geode) harvest = (factory.oreRobots, factory.clayRobots, factory.obsidianRobots, factory.geodeRobots);
            
            for (int i = 0; i < 5; i++)
            {
                if (i == 0 && factory.ore >= blueprint.oreRobotOreCost && timePassed < desperation)
                {
                    var robotFactory = AddHarvest(factory, harvest);
                    robotFactory.oreRobots += 1;
                    robotFactory.ore -= blueprint.oreRobotOreCost;
                    results.Add(SolveGPT(blueprint, robotFactory, timePassed + 1));
                }
                else if (i == 1 && factory.ore >= blueprint.clayRobotOreCost && timePassed < desperation)
                {
                    var robotFactory = AddHarvest(factory, harvest);
                    robotFactory.clayRobots += 1;
                    robotFactory.ore -= blueprint.clayRobotOreCost;
                    results.Add(SolveGPT(blueprint, robotFactory, timePassed + 1));
                }
                else if (i == 2 && factory.ore >= blueprint.obsidianRobotOreCost && factory.clay >= blueprint.obsidianRobotClayCost/* && timePassed < desperation*/)
                {
                    var robotFactory = AddHarvest(factory, harvest);
                    robotFactory.obsidianRobots += 1;
                    robotFactory.ore -= blueprint.obsidianRobotOreCost;
                    robotFactory.clay -= blueprint.obsidianRobotClayCost;
                    results.Add(SolveGPT(blueprint, robotFactory, timePassed + 1));
                }
                else if (i == 3 && factory.ore >= blueprint.geodeRobotOreCost && factory.obsidian >= blueprint.geodeRobotObsidianCost)
                {
                    var robotFactory = AddHarvest(factory, harvest);
                    robotFactory.geodeRobots += 1;
                    robotFactory.ore -= blueprint.geodeRobotOreCost;
                    robotFactory.obsidian -= blueprint.geodeRobotObsidianCost;
                    results.Add(SolveGPT(blueprint, robotFactory, timePassed + 1));
                }
                else if (i == 4 && (timePassed < desperation || factory.ore <= blueprint.geodeRobotOreCost || factory.ore <= blueprint.obsidianRobotOreCost || factory.obsidian <= blueprint.geodeRobotObsidianCost || factory.clay <= blueprint.obsidianRobotClayCost))
                {
                    var robotFactory = AddHarvest(factory, harvest);
                    results.Add(SolveGPT(blueprint, robotFactory, timePassed + 1));
                }
            }

            return results.Max();
        }
    }

    static RobotFactory AddHarvest(RobotFactory factory, (int ore, int clay, int obsidian, int geode) harvest)
    {
        var returnFactory = factory;
        returnFactory.ore += harvest.ore;
        returnFactory.clay += harvest.clay;
        returnFactory.obsidian += harvest.obsidian;
        returnFactory.geodes += harvest.geode;
        return returnFactory;
    }


    static int FindMostGeodes(RobotFactoryBlueprint blueprint, int timeLimit)
    {
        return Solve(blueprint, new RobotFactory(), 1, timeLimit);
    }

    static int Solve2(RobotFactoryBlueprint blueprint, RobotFactory factory, int timePassed, int timeLimit)
    {
        if (timePassed == timeLimit)
        {
            return factory.geodes + factory.geodeRobots;
        }
        else if (timePassed > timeLimit - 7)
        {
            var newFactory = factory;
            var mostRobots = Math.Min(factory.ore / blueprint.geodeRobotOreCost, factory.obsidian / blueprint.geodeRobotObsidianCost);
            newFactory.geodeRobots += mostRobots;
            newFactory.geodes += factory.geodeRobots;

            return Solve2(blueprint, newFactory, timePassed + 1, timeLimit);
        }
        else
        {
            List<int> results = new();

            (int ore, int clay, int obsidian, int geode) harvest = (factory.oreRobots, factory.clayRobots, factory.obsidianRobots, factory.geodeRobots);

            var buildOptions = GetBuildOptions(blueprint, factory);

            foreach (var buildOption in buildOptions)
            {
                var newFactory = buildOption.factory;
                newFactory.oreRobots += buildOption.oreRobots;
                newFactory.clayRobots += buildOption.clayRobots;
                newFactory.obsidianRobots += buildOption.obsidianRobots;
                newFactory.geodeRobots += buildOption.geodeRobots;
                newFactory.ore += harvest.ore;
                newFactory.clay += harvest.clay;
                newFactory.obsidian += harvest.obsidian;
                newFactory.geodes += harvest.geode;
                results.Add(Solve2(blueprint, newFactory, timePassed + 1, timeLimit));
            }

            return results.Max();
        }
    }


    static int Solve(RobotFactoryBlueprint blueprint, RobotFactory factory, int timePassed, int timeLimit)
    {
        if (timePassed == timeLimit)
        {
            return factory.geodes + factory.geodeRobots;
        }
        else if (timePassed == timeLimit - 1)
        {
            var newFactory = factory;
            var mostRobots = Math.Min(factory.ore / blueprint.geodeRobotOreCost, factory.obsidian / blueprint.geodeRobotObsidianCost);
            newFactory.geodeRobots += mostRobots;
            newFactory.geodes += factory.geodeRobots;

            return Solve(blueprint, newFactory, timePassed + 1, timeLimit);
        }
        else
        {
            List<int> results = new();

            (int ore, int clay, int obsidian, int geode) harvest = (factory.oreRobots, factory.clayRobots, factory.obsidianRobots, factory.geodeRobots);

            var buildOptions = GetBuildOptions(blueprint, factory);

            foreach (var buildOption in buildOptions)
            {
                var newFactory = buildOption.factory;
                newFactory.oreRobots += buildOption.oreRobots;
                newFactory.clayRobots += buildOption.clayRobots;
                newFactory.obsidianRobots += buildOption.obsidianRobots;
                newFactory.geodeRobots += buildOption.geodeRobots;
                newFactory.ore += harvest.ore;
                newFactory.clay += harvest.clay;
                newFactory.obsidian += harvest.obsidian;
                newFactory.geodes += harvest.geode;
                results.Add(Solve(blueprint, newFactory, timePassed + 1, timeLimit));
            }

            return results.Max();
        }
    }

    static HashSet<(int oreRobots, int clayRobots, int obsidianRobots, int geodeRobots, RobotFactory factory)> GetBuildOptions(RobotFactoryBlueprint blueprint, RobotFactory factory)
    {
        HashSet<(int oreRobots, int clayRobots, int obsidianRobots, int geodeRobots, RobotFactory factory)> results = new();
        bool built = false;

        results = new();
        if (factory.ore >= blueprint.oreRobotOreCost)
        {
            var newFactory = factory;
            newFactory.ore -= blueprint.oreRobotOreCost;
            results.Add((1, 0, 0, 0, newFactory));
            built = true;
        }
        if (factory.ore >= blueprint.clayRobotOreCost)
        {
            var newFactory = factory;
            newFactory.ore -= blueprint.clayRobotOreCost;
            results.Add((0, 1, 0, 0, newFactory));
            built = true;
        }
        if (factory.ore >= blueprint.obsidianRobotOreCost && factory.clay >= blueprint.obsidianRobotClayCost)
        {
            var newFactory = factory;
            newFactory.ore -= blueprint.obsidianRobotOreCost;
            newFactory.clay -= blueprint.obsidianRobotClayCost;
            results.Add((0, 0, 1, 0, newFactory));
            built = true;
        }
        if (factory.ore >= blueprint.geodeRobotOreCost && factory.obsidian >= blueprint.geodeRobotObsidianCost)
        {
            var newFactory = factory;
            newFactory.ore -= blueprint.geodeRobotOreCost;
            newFactory.obsidian -= blueprint.geodeRobotObsidianCost;
            results.Add((0, 0, 0, 1, newFactory));
            built = true;
        }

        if (built == false)
        {
            results.Add((0, 0, 0, 0, factory));

            return results;
        }
        else
        {
            var furtherResults =  FurtherBuild(blueprint, results);

            furtherResults.Add((0, 0, 0, 0, factory));

            return furtherResults;
        }
    }

    static HashSet<(int oreRobots, int clayRobots, int obsidianRobots, int geodeRobots, RobotFactory factory)> FurtherBuild(RobotFactoryBlueprint blueprint, HashSet<(int oreRobots, int clayRobots, int obsidianRobots, int geodeRobots, RobotFactory factory)> results)
    {
        int cur = results.Count;
        HashSet<(int oreRobots, int clayRobots, int obsidianRobots, int geodeRobots, RobotFactory factory)> addendum = new();
        foreach (var entry in results)
        {
            if (entry.factory.ore >= blueprint.oreRobotOreCost)
            {
                var newFactory = entry.factory;
                newFactory.ore -= blueprint.oreRobotOreCost;
                var newEntry = (entry.oreRobots + 1, entry.clayRobots, entry.obsidianRobots, entry.geodeRobots, newFactory);
                if (!results.Contains(newEntry) && !addendum.Contains(newEntry))
                    addendum.Add(newEntry);
            }
            if (entry.factory.ore >= blueprint.clayRobotOreCost)
            {
                var newFactory = entry.factory;
                newFactory.ore -= blueprint.clayRobotOreCost;
                var newEntry = (entry.oreRobots, entry.clayRobots + 1, entry.obsidianRobots, entry.geodeRobots, newFactory);
                if (!results.Contains(newEntry) && !addendum.Contains(newEntry))
                    addendum.Add(newEntry);
            }
            if (entry.factory.ore >= blueprint.obsidianRobotOreCost && entry.factory.clay >= blueprint.obsidianRobotClayCost)
            {
                var newFactory = entry.factory;
                newFactory.ore -= blueprint.obsidianRobotOreCost;
                newFactory.clay -= blueprint.obsidianRobotClayCost;
                var newEntry = (entry.oreRobots, entry.clayRobots, entry.obsidianRobots + 1, entry.geodeRobots, newFactory);
                if (!results.Contains(newEntry) && !addendum.Contains(newEntry))
                    addendum.Add(newEntry);
            }
            if (entry.factory.ore >= blueprint.geodeRobotOreCost && entry.factory.obsidian >= blueprint.geodeRobotObsidianCost)
            {
                var newFactory = entry.factory;
                newFactory.ore -= blueprint.geodeRobotOreCost;
                newFactory.obsidian -= blueprint.geodeRobotObsidianCost;
                var newEntry = (entry.oreRobots, entry.clayRobots, entry.obsidianRobots, entry.geodeRobots + 1, newFactory);
                if (!results.Contains(newEntry) && !addendum.Contains(newEntry))
                    addendum.Add(newEntry);
            }
        }
        if (results.Count == cur)
        {
            return results;
        }
            
        else
        {
            foreach (var entry in addendum)
                results.Add(entry);
            return FurtherBuild(blueprint, results);
        }
            
    }
    internal struct RobotFactoryBlueprint
    {
        internal int id;
        internal int oreRobotOreCost;
        internal int clayRobotOreCost;
        internal int obsidianRobotOreCost;
        internal int obsidianRobotClayCost;
        internal int geodeRobotOreCost;
        internal int geodeRobotObsidianCost;

        public RobotFactoryBlueprint(int[] input)
        {
            this.id = input[0];
            this.oreRobotOreCost = input[1];
            this.clayRobotOreCost = input[2];
            this.obsidianRobotOreCost = input[3];
            this.obsidianRobotClayCost = input[4];
            this.geodeRobotOreCost = input[5];
            this.geodeRobotObsidianCost = input[6];
        }
    }

    internal struct RobotFactory
    {
        internal int oreRobots;
        internal int clayRobots;
        internal int obsidianRobots;
        internal int geodeRobots;
        internal int ore;
        internal int clay;
        internal int obsidian;
        internal int geodes;

        public RobotFactory(int oreRobots, int clayRobots, int obsidianRobots, int geodeRobots, int ore, int clay, int obsidian, int geodes)
        {
            this.oreRobots = oreRobots;
            this.clayRobots = clayRobots;
            this.obsidianRobots = obsidianRobots;
            this.geodeRobots = geodeRobots;
            this.ore = ore;
            this.clay = clay;
            this.obsidian = obsidian;
            this.geodes = geodes;
        }

        public RobotFactory()
        {
            this.oreRobots = 1;
            this.clayRobots = 0;
            this.obsidianRobots = 0;
            this.geodeRobots = 0;
            this.ore = 0;
            this.clay = 0;
            this.obsidian = 0;
            this.geodes = 0;
        }


        internal RobotFactory Clone()
        {
            return new RobotFactory(oreRobots, clayRobots, obsidianRobots, geodeRobots, ore, clay, obsidian, geodes);
        }
    }
}
