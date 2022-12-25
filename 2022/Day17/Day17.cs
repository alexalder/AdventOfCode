using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day17
{
    static readonly int shapeNumber = 2022;
    static readonly long moreNumber = 1000000000000;

    public static void Run()
    {
        var input = File.ReadAllText(Utils.GetInputPath());

        Console.WriteLine(SimulateRocks(input));

        Console.WriteLine(SimulateMore(input));
    }

    static int SimulateRocks(string input)
    {
        int time = 0;
        int wind = 0;
        int maxHeight = 0;
        (int x, int y) shapePosition = (0, 0);
        HashSet<(int x, int y)> rocks = new();

        for (int curShapeIndex = 0; curShapeIndex < shapeNumber; curShapeIndex++)
        {
            shapePosition = (2, maxHeight + 4);
            var curShape = GetShape(curShapeIndex);

            while (true)
            {
                // Wind effects
                wind = GetWind(input, time++);
                (int x, int y) newPosition = (shapePosition.x + wind, shapePosition.y);
                if (CheckVolume(curShape, newPosition, rocks))
                    shapePosition = newPosition;
                newPosition = (shapePosition.x, shapePosition.y - 1);

                // Check rocks
                if (CheckVolume(curShape, newPosition, rocks))
                    shapePosition = newPosition;
                else
                {
                    foreach (var rock in curShape.GetVolume(shapePosition))
                    {
                        rocks.Add(rock);
                        if (rock.y > maxHeight)
                            maxHeight = rock.y;
                    }
                    break;
                }
            }
        }


        return maxHeight;
    }

    static long SimulateMore(string input)
    {
        long time = 0;
        int wind = 0;
        long maxHeight = 0;
        long prevHeight = 0;
        int shapesPerCycle = 0;
        int prevShapes;
        long offset = 0;
        (int x, long y) shapePosition = (0, 0);
        HashSet<(int x, long y)> rocks = new();

        for (long curShapeIndex = 0; curShapeIndex < moreNumber; curShapeIndex++)
        {
            shapePosition = (2, maxHeight + 4);
            var curShape = GetShape(curShapeIndex);

            while (true)
            {
                if (time % input.Length == 0 && time / input.Length < 3)
                {
                    prevHeight = maxHeight;
                    prevShapes = shapesPerCycle;

                    shapesPerCycle = (int)curShapeIndex - prevShapes;
                }
                if (time % input.Length == 0 && time / input.Length == 3)
                {
                    long cycles = moreNumber / shapesPerCycle;
                    while (curShapeIndex + cycles * shapesPerCycle > moreNumber)
                        cycles -= 1;
                    time += cycles * input.Length;
                    offset = (maxHeight - prevHeight) * cycles;
                    curShapeIndex += cycles * shapesPerCycle;
                }
                // Wind effects
                wind = GetWind(input, time++);
                (int x, long y) newPosition = (shapePosition.x + wind, shapePosition.y);
                if (CheckVolume(curShape, newPosition, rocks))
                    shapePosition = newPosition;
                newPosition = (shapePosition.x, shapePosition.y - 1);

                // Check rocks
                if (CheckVolume(curShape, newPosition, rocks))
                    shapePosition = newPosition;
                else
                {
                    foreach (var rock in curShape.GetVolume(shapePosition))
                    {
                        rocks.Add(rock);
                        if (rock.y > maxHeight)
                            maxHeight = rock.y;
                        if (rocks.Contains((0, rock.y)) && rocks.Contains((1, rock.y)) && rocks.Contains((2, rock.y)) && rocks.Contains((3, rock.y)) && rocks.Contains((4, rock.y)) && rocks.Contains((5, rock.y)) && rocks.Contains((6, rock.y)))
                            rocks.RemoveWhere(x => x.y < rock.y);
                    }
                    break;
                }
            }
        }

        return maxHeight + offset;
    }

    static bool CheckVolume(Tetramin tetramin, (int x, int y) position, HashSet<(int x, int y)> rocks)
    {
        var volume = tetramin.GetVolume(position);
        if (volume.Any(x => x.x < 0 || x.x > 6 || x.y < 1))
            return false;
        if (volume.Any(x => rocks.Contains(x)))
            return false;
        return true;
    }

    static bool CheckVolume(Tetramin tetramin, (int x, long y) position, HashSet<(int x, long y)> rocks)
    {
        var volume = tetramin.GetVolume(position);
        if (volume.Any(x => x.x < 0 || x.x > 6 || x.y < 1))
            return false;
        if (volume.Any(x => rocks.Contains(x)))
            return false;
        return true;
    }

    static int GetWind(string input, long time)
    {
        if (time >= input.Length)
            time = time % input.Length;
        if (input[(int)time] == '<')
            return -1;
        else 
            return 1;
    }

    static Tetramin GetShape(long shapeNumber)
    {
        shapeNumber = shapeNumber % 5;
        if (shapeNumber == 0)
            return new LinHoz();
        else if (shapeNumber == 1)
            return new Plus();
        else if (shapeNumber == 2)
            return new L();
        else if (shapeNumber == 3)
            return new LinVer();
        else if (shapeNumber == 4)
            return new Square();
        else throw new IOException();
    }

    class LinHoz : Tetramin
    {
        internal LinHoz()
        {
            length = 4;
            shape = new (int, int)[] { (0, 0), (1, 0), (2, 0), (3, 0) };
        }
    }

    class Plus : Tetramin
    {
        internal Plus()
        {
            length = 3;
            shape = new (int, int)[] { (1, 0), (0, 1), (1, 1), (2, 1), (1, 2) };
        }
    }
    class L : Tetramin
    {
        internal L()
        {
            length = 3;
            shape = new (int, int)[] { (0, 0), (1, 0), (2, 0), (2, 1), (2, 2) };
        }
    }
    class LinVer : Tetramin
    {
        internal LinVer()
        {
            length = 1;
            shape = new (int, int)[] { (0, 0), (0, 1), (0, 2), (0, 3) };
        }
    }
    class Square : Tetramin
    {
        internal Square()
        {
            length = 2;
            shape = new (int, int)[] { (0, 0), (1, 0), (0, 1), (1, 1) };
        }
    }

    abstract class Tetramin
    {
        internal int length;
        internal (int x, int y)[] shape;
        internal (int x, int y)[] GetVolume((int x, int y) position)
        {
            return shape.Select(x => (x.x + position.x, x.y + position.y)).ToArray();
        }

        internal (int x, long y)[] GetVolume((int x, long y) position)
        {
            return shape.Select(x => (x.x + position.x, x.y + position.y)).ToArray();
        }
    }
}
