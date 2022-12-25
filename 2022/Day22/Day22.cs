using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day22
{
    public static readonly (int x, int y) Right = (1, 0);
    public static readonly (int x, int y) Down = (0, 1);
    public static readonly (int x, int y) Left = (-1, 0);
    public static readonly (int x, int y) Up = (0, -1);

    public static void Run()
    {
        var input = Utils.SplitInput(Utils.ReadInputAsStrings(Utils.GetInputPath()));

        Console.WriteLine(NavigateMap(input));

        Console.WriteLine(NavigateMap3D(input));
    }

    static int NavigateMap(string[][] input)
    {
        Grid<bool?> grid = ParseInput(input);

        (int x, int y) startPosition = (0, 0);
        (int x, int y) startRotation = (1, 0);

        for (int x = 0; x < grid.xMax; x++)
            if (grid.GridValues[x, 0] == true)
            {
                startPosition = (x, 0);
                break;
            }

        var curPosition = startPosition;
        var curRotation = startRotation;

        string path = input[1][0];

        var directions = path.Split(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }, StringSplitOptions.RemoveEmptyEntries);
        var distances = path.Split(new char[] { 'L', 'R' }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x));

        int distancesIndex = 0;
        int directionsIndex = 0;

        while (distancesIndex < distances.Count())
        {
            var distance = distances.ElementAt(distancesIndex++);
            for (int travelled = 0; travelled < distance; travelled++)
            {
                bool breaking = false;

                (int x, int y) tentative = (curPosition.x + curRotation.x, curPosition.y + curRotation.y);

                if (tentative.x >= grid.xMax)
                    tentative.x -= grid.xMax;
                if (tentative.y >= grid.yMax)
                    tentative.y -= grid.yMax;
                if (tentative.x < 0)
                    tentative.x += grid.xMax;
                if (tentative.y < 0)
                    tentative.y += grid.yMax;

                if (grid.GridValues[tentative.x, tentative.y] == true)
                    curPosition = (tentative.x, tentative.y);

                else if (grid.GridValues[tentative.x, tentative.y] == false)
                    breaking = true;

                else if (grid.GridValues[tentative.x, tentative.y] == null)
                {
                    while (true)
                    {
                        tentative = (tentative.x + curRotation.x, tentative.y + curRotation.y);

                        if (tentative.x >= grid.xMax)
                            tentative.x -= grid.xMax;
                        if (tentative.y >= grid.yMax)
                            tentative.y -= grid.yMax;
                        if (tentative.x < 0)
                            tentative.x += grid.xMax;
                        if (tentative.y < 0)
                            tentative.y += grid.yMax;

                        if (grid.GridValues[tentative.x, tentative.y] == null)
                            continue;
                        if (grid.GridValues[tentative.x, tentative.y] == true)
                        {
                            curPosition = tentative;
                            break;
                        }
                        if (grid.GridValues[tentative.x, tentative.y] == false)
                        {
                            breaking = true;
                            break;
                        }
                    }
                }

                if (breaking)
                    break;
            }

            if (directionsIndex < directions.Count())
                curRotation = GetRotation(curRotation, directions.ElementAt(directionsIndex++));
        }

        return (curPosition.y + 1) * 1000 + (curPosition.x + 1) * 4 + GetFacingValue(curRotation);
    }

    static int NavigateMap3D(string[][] input)
    {
        Grid<bool?> grid = ParseInput(input);

        (int x, int y) startPosition = (0, 0);
        (int x, int y) startRotation = Right;

        for (int x = 0; x < grid.xMax; x++)
            if (grid.GridValues[x, 0] == true)
            {
                startPosition = (x, 0);
                break;
            }

        var curPosition = startPosition;
        var curRotation = startRotation;

        string path = input[1][0];

        var directions = path.Split(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }, StringSplitOptions.RemoveEmptyEntries);
        var distances = path.Split(new char[] { 'L', 'R' }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x));

        int distancesIndex = 0;
        int directionsIndex = 0;

        while (distancesIndex < distances.Count())
        {
            var distance = distances.ElementAt(distancesIndex++);
            for (int travelled = 0; travelled < distance; travelled++)
            {
                (int x, int y) tentativePosition = (curPosition.x + curRotation.x, curPosition.y + curRotation.y);
                (int x, int y) tentativeRotation = curRotation;

                if (tentativePosition.x >= grid.xMax)
                    (tentativePosition, tentativeRotation) = GetNewFace(grid, curPosition, curRotation);
                if (tentativePosition.y >= grid.yMax)
                    (tentativePosition, tentativeRotation) = GetNewFace(grid, curPosition, curRotation);
                if (tentativePosition.x < 0)
                    (tentativePosition, tentativeRotation) = GetNewFace(grid, curPosition, curRotation);
                if (tentativePosition.y < 0)
                    (tentativePosition, tentativeRotation) = GetNewFace(grid, curPosition, curRotation);

                if (grid.GridValues[tentativePosition.x, tentativePosition.y] == true)
                {
                    curPosition = tentativePosition;
                    curRotation = tentativeRotation;
                }

                else if (grid.GridValues[tentativePosition.x, tentativePosition.y] == false)
                    break;

                else if (grid.GridValues[tentativePosition.x, tentativePosition.y] == null)
                {
                    (tentativePosition, tentativeRotation) = GetNewFace(grid, curPosition, curRotation);

                    //if (tentativePosition.x >= grid.xMax)
                    //    GetNewFace(grid, curPosition, curRotation);
                    //if (tentativePosition.y >= grid.yMax)
                    //    GetNewFace(grid, curPosition, curRotation);
                    //if (tentativePosition.x < 0)
                    //    GetNewFace(grid, curPosition, curRotation);
                    //if (tentativePosition.y < 0)
                    //    GetNewFace(grid, curPosition, curRotation);

                    if (grid.GridValues[tentativePosition.x, tentativePosition.y] == null)
                        throw new Exception();
                    if (grid.GridValues[tentativePosition.x, tentativePosition.y] == true)
                    {
                        curPosition = tentativePosition;
                        curRotation = tentativeRotation;
                    }
                    if (grid.GridValues[tentativePosition.x, tentativePosition.y] == false)
                    {
                        break;
                    }
                }
            }

            if (directionsIndex < directions.Count())
                curRotation = GetRotation(curRotation, directions.ElementAt(directionsIndex++));
        }

        return (curPosition.y + 1) * 1000 + (curPosition.x + 1) * 4 + GetFacingValue(curRotation);
    }

    static ((int x, int y) position, (int x, int y) rotation) GetNewFace(Grid<bool?> grid, (int x, int y) oldPosition, (int x, int y) rotation)
    {
        int xOffsets = grid.xMax / 3;
        int yOffsets = grid.yMax / 4;

        (int x, int y) facePosition = oldPosition;

        int faceX = 0;
        int faceY = 0;

        while (facePosition.x >= xOffsets)
        {
            facePosition.x -= xOffsets;
            faceX += 1;
        }

        while (facePosition.y >= yOffsets)
        {
            facePosition.y -= yOffsets;
            faceY += 1;
        }

        if (facePosition.x == xOffsets - 1 && rotation == Right)
        {
            if (faceY == 0)
            {
                return (
                    (xOffsets * 2 - 1, 3* yOffsets - 1 - facePosition.y),
                    Left
                );
            }
            else if (faceY == 1)
            {
                return (
                    (2 * xOffsets + facePosition.y, yOffsets - 1),
                    Up
                );
            }
            else if (faceY == 2)
            {
                return (
                    (xOffsets * 3 - 1, yOffsets - 1 - facePosition.y),
                    Left
                );
            }
            else if (faceY == 3)
            {
                return (
                    (xOffsets + facePosition.y, yOffsets * 3 - 1),
                    Up
                );
            }
        }
        else if (facePosition.y == yOffsets - 1 && rotation == Down)
        {
            if (faceX == 0)
            {
                return (
                    (2 * xOffsets + facePosition.x, yOffsets * 0),
                    Down
                );
            }
            else if (faceX == 1)
            {
                return (
                    (xOffsets - 1, 3 * yOffsets + facePosition.x),
                    Left
                );
            }
            else if (faceX == 2)
            {
                return (
                    (xOffsets * 2 - 1, yOffsets + facePosition.x),
                    Left
                );
            }
        }
        // Going left
        else if (facePosition.x == 0 && rotation == Left)
        {
            if (faceY == 0)
            {
                return (
                    (xOffsets * 0, yOffsets * 3 - 1 - facePosition.y),
                    Right
                );
            }
            else if (faceY == 1)
            {
                return (
                    (facePosition.y, yOffsets * 2),
                    Down
                );
            }
            else if (faceY == 2)
            {
                return (
                    (xOffsets, yOffsets - 1 - facePosition.y),
                    Right
                );
            }
            else if (faceY == 3)
            {
                return (
                    (xOffsets + facePosition.y, yOffsets * 0),
                    Down
                );
            }
        }
        else if (facePosition.y == 0 && rotation == Up)
        {
            if (faceX == 0)
            {
                return (
                    (xOffsets, yOffsets + facePosition.x),
                    Right
                );
            }
            else if (faceX == 1)
            {
                return (
                    (xOffsets * 0, yOffsets * 3 + facePosition.x),
                    Right
                );
            }
            else if (faceX == 2)
            {
                return (
                    (facePosition.x, yOffsets * 4 - 1),
                    Up
                );
            }
        }

        throw new Exception();
    }

    static Grid<bool?> ParseInput(string[][] input)
    {
        int maxLen = input[0].Max(x => x.Length);

        var padded = input[0].Select(x => x.PadRight(maxLen)).ToArray();

        Grid<bool?> grid = new(padded, x =>
        {
            if (x == ' ')
                return null;
            else if (x == '.')
                return true;
            else if (x == '#')
                return false;
            else
                throw new Exception();
        });

        return grid;
    }

    static int GetFacingValue((int, int) curRotation)
    {
        if (curRotation == Right)
        {
            return 0;
        }
        if (curRotation == Down)
        {
            return 1;
        }
        if (curRotation == Left)
        {
            return 2;
        }
        if (curRotation == Up)
        {
            return 3;
        }
        throw new Exception();
    }

    static (int, int) GetRotation((int, int) curRotation, string rotate)
    {
        if (curRotation == Right)
        {
            if (rotate == "R")
                return Down;
            else if (rotate == "L")
                return Up;
        }
        if (curRotation == Down)
        {
            if (rotate == "R")
                return Left;
            else if (rotate == "L")
                return Right;
        }
        if (curRotation == Left)
        {
            if (rotate == "R")
                return Up;
            else if (rotate == "L")
                return Down;
        }
        if (curRotation == Up)
        {
            if (rotate == "R")
                return Right;
            else if (rotate == "L")
                return Left;
        }
        throw new Exception();
    }

}
