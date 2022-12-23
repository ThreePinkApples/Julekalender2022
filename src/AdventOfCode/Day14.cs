namespace AdventCalendar2022.AdventOfCode;
internal class Day14
{
    public static void Run()
    {
        var sample = @"498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9
";
        var paths = File.ReadAllText("AdventOfCode/Data/Day14Input.txt")
            .Split("\n")
            .Where(path => !string.IsNullOrWhiteSpace(path))
            .Select(path => path.Trim().Split("->").Select(position =>
            {
                var parts = position.Trim().Split(",");
                return new Coordinate(int.Parse(parts[0].Trim()), int.Parse(parts[1].Trim()));
            }).ToArray()).ToList();
        //paths = sample
        //    .Split("\r\n")
        //    .Where(path => !string.IsNullOrWhiteSpace(path))
        //    .Select(path => path.Trim().Split("->").Select(position =>
        //    {
        //        var parts = position.Trim().Split(",");
        //        return new Coordinate(int.Parse(parts[0].Trim()), int.Parse(parts[1].Trim()));
        //    }).ToArray()).ToList();

        var sandStartCoords = new Coordinate(500, 0);
        var maxX = Math.Max(sandStartCoords.X, paths.Select(path => path.Max(coordinate => coordinate.X)).Max());
        var maxY = paths.Select(path => path.Max(coordinate => coordinate.Y)).Max();
        var matrix = new int[maxX + 1, maxY + 1];
        foreach (var path in paths)
        {
            for (var positionIndex = 0; positionIndex < path.Length - 1; positionIndex++)
            {
                var from = path[positionIndex];
                var to = path[positionIndex + 1];
                if (from.Y != to.Y)
                {
                    var highest = Math.Min(from.Y, to.Y);
                    var lowest = Math.Max(from.Y, to.Y);
                    for (var lineIndex = highest; lineIndex <= lowest; lineIndex++)
                        matrix[from.X, lineIndex] = 1;
                }
                else if (from.X != to.X)
                {
                    var leftMost = Math.Min(from.X, to.X);
                    var rightMost = Math.Max(from.X, to.X);
                    for (var lineIndex = leftMost; lineIndex <= rightMost; lineIndex++)
                        matrix[lineIndex, from.Y] = 1;
                }
            }
        }
        var sand = new Coordinate(sandStartCoords.X, sandStartCoords.Y);
        var sandAtRest = 0;
        while (true)
        {
            if (sand.Y >= maxY)
                break;
            if (matrix[sand.X, sand.Y + 1] == 0)
                sand.Y++;
            else if (matrix[sand.X - 1, sand.Y + 1] == 0)
            {
                sand.Y++;
                sand.X--;
            }
            else if (matrix[sand.X + 1, sand.Y + 1] == 0)
            {
                sand.Y++;
                sand.X++;
            }
            else
            {
                sandAtRest++;
                matrix[sand.X, sand.Y] = -1;
                sand = new Coordinate(sandStartCoords.X, sandStartCoords.Y);
            }
        }
        Console.WriteLine($"AdventOfCode Day 14 Part 1 result: {sandAtRest}");
        Console.WriteLine($"AdventOfCode Day 14 Part 2 result: ");
    }

    public static void PrintMatrix(int[,] matrix)
    {
        for (var y = 0; y < matrix.GetLength(1); y++)
        {
            for (var x = 0; x < matrix.GetLength(0); x++)
            {
                Console.Write(matrix[x, y] == 1 ? "#" : ".");
            }
            Console.WriteLine();
        }
    }
}

public class Coordinate
{
    public int X { get; set; }
    public int Y { get; set; }
    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }
}
