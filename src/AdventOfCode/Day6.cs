namespace AdventCalendar2022.AdventOfCode;
internal class Day6
{
    public static void Run()
    {
        var datastream = File.ReadAllText("AdventOfCode/Data/Day6Input.txt");
        var startOfPacketCharsProcessed = 0;
        for (var scanIndex = 0; scanIndex < datastream.Length; scanIndex++)
        {
            startOfPacketCharsProcessed++;
            if (datastream.Substring(scanIndex, 4).ToHashSet().Count == 4)
            {
                startOfPacketCharsProcessed += 3;
                break;
            }
        }
        Console.WriteLine($"AdventOfCode Day 6 Part 1 result: {startOfPacketCharsProcessed}");

        var startOfMessageCharsProcessed = 0;
        for (var scanIndex = 0; scanIndex < datastream.Length; scanIndex++)
        {
            startOfMessageCharsProcessed++;
            if (datastream.Substring(scanIndex, 14).ToHashSet().Count == 14)
            {
                startOfMessageCharsProcessed += 13;
                break;
            }
        }
        Console.WriteLine($"AdventOfCode Day 6 Part 2 result: {startOfMessageCharsProcessed}");
    }
}
