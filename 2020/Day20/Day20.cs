using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    public class Day20
    {
        public static void Run()
        {
            var input = Utils.ReadInputAsStrings(Utils.GetInputPath(2020, 20));

            Console.WriteLine(CornerIDs(input));

            Console.WriteLine(SearchMonsters(input));
        }

        private static int SearchMonsters(string[] input)
        {
            List<Photo> photos = ParsePhotos(input);

            string[] grid = PlacePhotos(photos).GetBigPictureCutBorders();

            Photo bigPicture = new Photo(0, grid);

            int monsters = 0;

            while (monsters == 0)
            {
                foreach (var rotation in bigPicture.GetAllOrientations())
                {
                    for (int line = 1; line < rotation.content.Length - 1; line++)
                    {
                        for (int character = 0; character < rotation.content[line].Length - "#    ##    ##    ###".Length; character++)
                        {
                            // Indentation goes brrrr
                            if (rotation.content[line][character] == '#')
                                if (rotation.content[line + 1][character + 1] == '#')
                                    if (rotation.content[line + 1][character + 4] == '#')
                                        if (rotation.content[line][character + 5] == '#')
                                            if (rotation.content[line][character + 6] == '#')
                                                if (rotation.content[line + 1][character + 7] == '#')
                                                    if (rotation.content[line + 1][character + 10] == '#')
                                                        if (rotation.content[line][character + 11] == '#')
                                                            if (rotation.content[line][character + 12] == '#')
                                                                if (rotation.content[line + 1][character + 13] == '#')
                                                                    if (rotation.content[line + 1][character + 16] == '#')
                                                                        if (rotation.content[line][character + 17] == '#')
                                                                            if (rotation.content[line][character + 18] == '#')
                                                                                if (rotation.content[line - 1][character + 18] == '#')
                                                                                    if (rotation.content[line][character + 19] == '#')
                                                                                        monsters++;
                        }
                    }
                }
                break;
            }

            int waters = 0;

            for (int line = 0; line < grid.Length; line++)
                for (int character = 0; character < grid[line].Length; character++)
                    if (grid[line][character] == '#')
                        waters++;

            return waters - monsters * 15;
        }

        private static double CornerIDs(string[] input)
        {
            List<Photo> photos = ParsePhotos(input);

            PhotoGrid ordered = PlacePhotos(photos);

            double res = 1;
            foreach (var photo in ordered.Values)
                if (ordered.GetAdjacentValues(photo.Key).Length == 2)
                    res *= photo.Value.id;

            return res;
        }

        private static PhotoGrid PlacePhotos(List<Photo> photos)
        {
            PhotoGrid grid = new PhotoGrid();

            while (photos.Count > 0)
            {

                foreach (var coordinate in grid.GetEmptySpaces())
                {
                    bool added = false;
                    foreach (var newPhoto in photos)
                    {
                        foreach (var orientation in newPhoto.GetAllOrientations())
                        {
                            bool correct = true;
                            foreach (var nearbyPhotoData in grid.GetAdjacentValuesAndCoordinates(coordinate))
                            {
                                int x = nearbyPhotoData.Item1;
                                int y = nearbyPhotoData.Item2;
                                Photo nearbyPhoto = nearbyPhotoData.Item3;
                                if (!orientation.Borders(nearbyPhoto, (x, y)))
                                    correct = false;
                            }
                            if (correct)
                            {
                                grid.Add(coordinate, orientation);
                                photos.Remove(newPhoto);
                                added = true;
                                break;
                            }
                        }
                        if (added)
                            break;
                    }
                }
            }

            return grid;
        }

        public static List<Photo> ParsePhotos(string[] input)
        {
            List<Photo> photos = new List<Photo>();
            var photosInput = Utils.SplitInput(input, "");

            foreach (string[] photoStrings in photosInput)
            {
                List<string> borders = new List<string>();

                int id = int.Parse(photoStrings[0].Split(new char[] { ' ', ':' }, StringSplitOptions.None)[1]);

                photos.Add(new Photo(id, photoStrings.Skip(1).ToArray()));
            }

            return photos;
        }
    }

    public class Photo
    {
        public int id;
        public string[] content;
        string[] borders = new string[4];

        public string Up
        {
            get => borders[0];
            set => borders[0] = value;
        }
        public string Right
        {
            get => borders[1];
            set => borders[1] = value;
        }
        public string Down
        {
            get => borders[2];
            set => borders[2] = value;
        }
        public string Left
        {
            get => borders[3];
            set => borders[3] = value;
        }

        public Photo(int id, string[] input)
        {
            this.id = id;
            content = input;

            Up = input[0];

            List<char> right = new List<char>();
            foreach (string line in input)
                right.Add(line.Last());
            Right = new string(right.ToArray());

            Down = input.Last();

            List<char> left = new List<char>();
            foreach (string line in input)
                left.Add(line.First());
            Left = new string(left.ToArray());
        }

        public void RotateRight()
        {
            string oldUp = Up;
            string oldRight = Right;
            string oldDown = Down;
            string oldLeft = Left;

            string[] oldContent = content;

            Up = new string(oldLeft.Reverse().ToArray());
            Right = oldUp;
            Down = new string(oldRight.Reverse().ToArray());
            Left = oldDown;

            content = new string[oldContent.Length];
            for (int i = 0; i < content.Length; i++)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string oldString in oldContent)
                    sb.Append(oldString[i]);
                content[i] = new string(sb.ToString().Reverse().ToArray());
            }
        }

        public void FlipHorizontal()
        {
            string oldUp = Up;
            string oldRight = Right;
            string oldDown = Down;
            string oldLeft = Left;

            string[] oldContent = content;

            Up = new string(oldUp.Reverse().ToArray());
            Right = oldLeft;
            Down = new string(oldDown.Reverse().ToArray());
            Left = oldRight;

            content = new string[oldContent.Length];
            for (int i = 0; i < content.Length; i++)
            {
                content[i] = new string(oldContent[i].Reverse().ToArray());
            }
        }

        public bool Borders(Photo otherPhoto)
        {
            if (this.Up == otherPhoto.Down)
                return true;
            else if (this.Down == otherPhoto.Up)
                return true;
            else if (this.Left == otherPhoto.Right)
                return true;
            else if (this.Right == otherPhoto.Left)
                return true;
            return false;
        }

        public bool Borders(Photo otherPhoto, (int, int) coordinateOtherCentric)
        {
            if (coordinateOtherCentric == (0, -1))
            {
                if (this.Up == otherPhoto.Down)
                    return true;
            }

            else if (coordinateOtherCentric == (0, 1))
            {
                if (this.Down == otherPhoto.Up)
                    return true;
            }

            else if (coordinateOtherCentric == (1, 0))
            {
                if (this.Left == otherPhoto.Right)
                    return true;
            }

            else if (coordinateOtherCentric == (-1, 0))
            {
                if (this.Right == otherPhoto.Left)
                    return true;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            if (this.id == ((Photo)obj).id)
            {
                return borders.SequenceEqual(((Photo)obj).borders);

            }
            return false;
        }

        public Photo Clone()
        {
            return new Photo(this.id, this.content);
        }

        public IEnumerable<Photo> GetAllOrientations()
        {
            yield return this;
            RotateRight();
            yield return this;
            RotateRight();
            yield return this;
            RotateRight();
            yield return this;
            RotateRight();

            FlipHorizontal();

            yield return this;
            RotateRight();
            yield return this;
            RotateRight();
            yield return this;
            RotateRight();
            yield return this;

            RotateRight();
            FlipHorizontal();
        }
    }

    public class PhotoGrid
    {
        Dictionary<(int, int), Photo> internalGrid = new Dictionary<(int, int), Photo>();

        public int XMin
        {
            get => internalGrid.Min(x => x.Key.Item1);
        }
        public int YMin
        {
            get => internalGrid.Min(x => x.Key.Item2);
        }
        public int XMax
        {
            get => internalGrid.Max(x => x.Key.Item1);
        }
        public int YMax
        {
            get => internalGrid.Max(x => x.Key.Item2);
        }

        public string[] GetBigPicture()
        {
            List<string> res = new List<string>();

            for (int y = YMin; y <= YMax; y++)
            {
                string[] bigContent = new string[internalGrid.First().Value.content.Length];
                for (int x = XMin; x <= XMax; x++)
                {
                    for (int i = 0; i < bigContent.Length; i++)
                    {
                        bigContent[i] += internalGrid[(x, y)].content[bigContent.Length - i - 1];
                    }

                }
                Array.ForEach(bigContent, x => res.Add(x));
            }

            return res.ToArray();
        }

        public string[] GetBigPictureCutBorders()
        {
            List<string> res = new List<string>();

            for (int y = YMin; y <= YMax; y++)
            {
                string[] bigContent = new string[internalGrid.First().Value.content.Length - 2];
                for (int x = XMin; x <= XMax; x++)
                {
                    for (int i = 0; i < bigContent.Length; i++)
                    {
                        bigContent[i] += internalGrid[(x, y)].content[bigContent.Length - i].Substring(1, internalGrid[(x, y)].content[bigContent.Length - i].Length - 2);
                    }

                }
                Array.ForEach(bigContent, x => res.Add(x));
            }

            return res.ToArray();
        }

        public Dictionary<(int, int), Photo> Values
        {
            get => internalGrid;
        }

        public PhotoGrid()
        {
        }

        public PhotoGrid(PhotoGrid otherGrid)
        {
            internalGrid = new Dictionary<(int, int), Photo>();
            foreach (var element in otherGrid.internalGrid)
                internalGrid.Add(element.Key, element.Value);
        }

        public void Add((int, int) coordinate, Photo photo)
        {
            internalGrid.Add(coordinate, photo);
        }

        public Photo[] GetAdjacentValues((int, int) coordinates)
        {
            int x = coordinates.Item1;
            int y = coordinates.Item2;
            List<Photo> res = new List<Photo>();

            if (internalGrid.ContainsKey((x, y + 1)))
                res.Add(internalGrid[(x, y + 1)]);
            if (internalGrid.ContainsKey((x + 1, y)))
                res.Add(internalGrid[(x + 1, y)]);
            if (internalGrid.ContainsKey((x, y - 1)))
                res.Add(internalGrid[(x, y - 1)]);
            if (internalGrid.ContainsKey((x - 1, y)))
                res.Add(internalGrid[(x - 1, y)]);

            return res.ToArray();
        }

        public (int, int, Photo)[] GetAdjacentValuesAndCoordinates((int, int) coordinates)
        {
            int x = coordinates.Item1;
            int y = coordinates.Item2;
            List<(int, int, Photo)> res = new List<(int, int, Photo)>();

            if (internalGrid.ContainsKey((x, y + 1)))
                res.Add((0, -1, internalGrid[(x, y + 1)]));
            if (internalGrid.ContainsKey((x + 1, y)))
                res.Add((-1, 0, internalGrid[(x + 1, y)]));
            if (internalGrid.ContainsKey((x, y - 1)))
                res.Add((0, 1, internalGrid[(x, y - 1)]));
            if (internalGrid.ContainsKey((x - 1, y)))
                res.Add((1, 0, internalGrid[(x - 1, y)]));

            return res.ToArray();
        }

        public List<(int, int)> GetEmptySpaces()
        {
            List<(int, int)> res = new List<(int, int)>();

            if (internalGrid.Count == 0)
            {
                res.Add((0, 0));
                return res;
            }

            foreach (var value in internalGrid)
            {
                int x = value.Key.Item1;
                int y = value.Key.Item2;
                if (!internalGrid.ContainsKey((x, y + 1)))
                    res.Add((x, y + 1));
                if (!internalGrid.ContainsKey((x + 1, y)))
                    res.Add((x + 1, y));
                if (!internalGrid.ContainsKey((x, y - 1)))
                    res.Add((x, y - 1));
                if (!internalGrid.ContainsKey((x - 1, y)))
                    res.Add((x - 1, y));
            }
            return res;
        }

        public PhotoGrid Clone()
        {
            return new PhotoGrid(this);
        }

        public void Clone(PhotoGrid destination)
        {
            destination.internalGrid = new Dictionary<(int, int), Photo>();

            foreach (var value in internalGrid)
                destination.internalGrid.Add(value.Key, value.Value);
        }
    }
}