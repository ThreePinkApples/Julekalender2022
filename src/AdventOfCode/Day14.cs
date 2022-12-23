namespace AdventCalendar2022.AdventOfCode;
internal class Day14
{
    public static void Run()
    {
        var sample = @"498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9
";
        var packets = File.ReadAllText("AdventOfCode/Data/Day14Input.txt")
            .Split("\n")
            .ToList();
        //packets = sample
        //    .Split("\r\n")
        //    .ToList();
        Console.WriteLine($"AdventOfCode Day 14 Part 1 result: ");
        Console.WriteLine($"AdventOfCode Day 14 Part 2 result: ");
    }
}
