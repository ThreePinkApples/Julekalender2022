namespace AdventCalendar2022.AdventOfCode;
internal class Day3
{
    public static void Run()
    {
        var rucksacks = File.ReadLines("AdventOfCode/Data/Day3Input.txt");
        var prioritySumPerRucksack = rucksacks
            .Select(rucksack => new[]
            {
                rucksack.Substring(0, rucksack.Length / 2),
                rucksack.Substring(rucksack.Length / 2)
            })
            .Select(rucksack =>
                rucksack[0]
                    .Where(item => rucksack[1].Contains(item))
                    .Distinct()
                    .Select(Priority)
                    .Sum()
            ).Sum();
        Console.WriteLine($"AdventOfCode Day 3 Part 1 result: {prioritySumPerRucksack}");

        var prioritySumPerGroup = rucksacks
            .Chunk(3)
            .Select(group =>
                group[0]
                    .Where(item => group[1].Contains(item) && group[2].Contains(item))
                    .Distinct()
                    .Select(Priority)
                    .Sum()
            ).Sum();
        Console.WriteLine($"AdventOfCode Day 3 Part 2 result: {prioritySumPerGroup}");
    }

    // A = 65 in ASCII, a = 97 in ASCII
    public static int Priority(char item) => item < 97 ? item - (65 - 27) : item - 96;
}
