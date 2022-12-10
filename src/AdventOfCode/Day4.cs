namespace AdventCalendar2022.AdventOfCode;
internal class Day4
{
    public static void Run()
    {
        var pairs = File.ReadLines("AdventOfCode/Data/Day4Input.txt")
            .Select(pair => pair.Split(','))
            .Select(pair => new[]
            {
                pair[0].Split('-').Select(int.Parse).ToArray(),
                pair[1].Split('-').Select(int.Parse).ToArray(),
            })
            .Select(pair => new[]
            {
                Enumerable.Range(pair[0][0], pair[0][1] - pair[0][0] + 1).ToList(),
                Enumerable.Range(pair[1][0], pair[1][1] - pair[1][0] + 1).ToList()
            }
            .OrderByDescending(range => range.Count)
            .ToArray());

        var fullyOverlappingPairs = pairs.Where(pair => pair[0].Intersect(pair[1]).Count() == pair[1].Count);
        Console.WriteLine($"AdventOfCode Day 4 Part 1 result: {fullyOverlappingPairs.Count()}");

        var overlappingPairs = pairs.Where(pair => pair[0].Intersect(pair[1]).Any());
        Console.WriteLine($"AdventOfCode Day 4 Part 2 result: {overlappingPairs.Count()}");
    }
}
