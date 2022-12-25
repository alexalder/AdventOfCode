using System;
using AdventOfCodeUtils;

namespace AdventOfCode2022;

public static class Day07
{
    static int smallFolderSize = 100000;
    static int diskSpace = 70000000;
    static int requiredSpace = 30000000;

    #region ElfFileSystem
    class ElfFileSystemEntry
    {
        internal string name;
    }

    class ElfFile : ElfFileSystemEntry
    {
        internal int size;

        internal ElfFile(string name, int size)
        {
            this.name = name;
            this.size = size;
        }

        internal ElfFile(string[] command)
        {
            this.name = command[1];
            this.size = int.Parse(command[0]);
        }
    }

    class ElfDirectory : ElfFileSystemEntry
    {
        internal ElfDirectory(string name)
        {
            this.name = name;
        }
    }
    #endregion

    public static void Run()
    {
        var input = Utils.ReadInputAsStrings(Utils.GetInputPath());

        Console.WriteLine(CountSmallFolders(input));

        Console.WriteLine(FindDirectoryToDelete(input));
    }

    private static int CountSmallFolders(string[] input)
    {
        TreeNode <ElfFileSystemEntry> fileSystem = ReadFileSystem(input);

        int smallFoldersSize = 0;

        GetFolderSize(fileSystem, (x) =>
        {
            if (x <= smallFolderSize)
                smallFoldersSize += x;
        });

        return smallFoldersSize;
    }

    private static int FindDirectoryToDelete(string[] input)
    {
        TreeNode<ElfFileSystemEntry> fileSystem = ReadFileSystem(input);
        List<int> directorySizes = new();

        int occupiedSpace = GetFolderSize(fileSystem, (x) => directorySizes.Add(x));

        int freeSpace = diskSpace - occupiedSpace;
        int neededSpace = requiredSpace - freeSpace;

        directorySizes.Sort();

        return directorySizes.Where(x => x > neededSpace).First();
    }

    private static int GetFolderSize(TreeNode<ElfFileSystemEntry> target, Action<int> action)
    {
        if (target.value.name == "lpv")
        { }
        int folderSize = 0;
        foreach (var entry in target.children)
        {
            if (entry.value is ElfFile)
                folderSize += ((ElfFile)entry.value).size;
            else if (entry.value is ElfDirectory)
                folderSize += GetFolderSize(entry, action);
        }

        if (action != null)
            action(folderSize);

        return folderSize;
    }

    private static TreeNode<ElfFileSystemEntry> ReadFileSystem(string[] input)
    {
        TreeNode<ElfFileSystemEntry> root = new(new ElfDirectory("/"));
        TreeNode<ElfFileSystemEntry> curFolder = root;

        foreach (string line in input.Skip(2))
        {
            if (line.StartsWith("$")) // Command
            {
                if (line.StartsWith("$ cd"))
                {
                    string folder = line.Substring(5);
                    if (folder == "..")
                    {
                        curFolder = curFolder.parent;
                        continue;
                    }
                    foreach (TreeNode<ElfFileSystemEntry> entry in curFolder.children)
                    {
                        if (entry.value.name == folder)
                        {
                            curFolder = entry;
                            break;
                        }
                            
                    }
                }
            }
            else if (line.StartsWith("dir")) // Directory
            {
                string folder = line.Substring(4);
                curFolder.AddNode(new ElfDirectory(folder));
            }
            else // File
            {
                curFolder.AddNode(new ElfFile(line.Split()));
            }
        }

        return root;
    }
}
