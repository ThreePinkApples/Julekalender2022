namespace AdventCalendar2022.AdventOfCode;
internal class Day7
{
    public static void Run()
    {
        var totalDiskSize = 70000000;
        var requiredSpace = 30000000;
        var input = File.ReadAllLines("AdventOfCode/Data/Day7Input.txt");
        var directoryStack = new Stack<string>();
        var directories = new Dictionary<string, Dictionary<string, int>>();
        foreach (var line in input)
        {
            if (line.StartsWith("$ cd"))
            {
                if (line.EndsWith("/"))
                    directoryStack.Clear();
                else if (line.EndsWith(".."))
                    directoryStack.Pop();
                else
                    directoryStack.Push(line.Split(" ").Last());
            }
            else if (int.TryParse(line.Split(" ").First(), out var size))
            {
                var fullPath = "/" + string.Join("/", directoryStack.Reverse());
                var path = "/";
                var fileName = line.Split(" ").Last();
                Day7.AddFile(directories, path, fullPath + fileName, size);
                foreach (var directory in directoryStack.Reverse())
                {
                    path = $"{path}/{directory}";
                    Day7.AddFile(directories, path, fullPath + fileName, size);
                }
            }
        }
        var smallDirectories = directories.Where(d => d.Value.Values.Sum() <= 100000);
        Console.WriteLine($"AdventOfCode Day 7 Part 1 result: {smallDirectories.Sum(sd => sd.Value.Values.Sum())}");

        var freeSpace = totalDiskSize - directories["/"].Sum(d => d.Value);
        var spaceToDelete = requiredSpace - freeSpace;
        var sizesOfDeletableDirectories = directories
            .Where(d => d.Value.Values.Sum() >= spaceToDelete)
            .OrderBy(d => d.Value.Values.Sum())
            .Select(d => d.Value.Values.Sum());
        Console.WriteLine($"AdventOfCode Day 7 Part 2 result: {sizesOfDeletableDirectories.First()}");
    }

    public static void AddFile(Dictionary<string, Dictionary<string, int>> directories, string path, string fileName, int size)
    {
        if (!directories.ContainsKey(path))
            directories.Add(path, new Dictionary<string, int>());
        directories[path].Add(fileName, size);
    }
}
