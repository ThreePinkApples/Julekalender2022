namespace AdventCalendar2022.AdventOfCode;
internal class Day10
{
    public static void Run()
    {
        var commands = File.ReadAllLines("AdventOfCode/Data/Day10Input.txt").ToList();
        var commandQueue = new Queue<string>(commands);

        var targetCycles = new[] { 20, 60, 100, 140, 180, 220 };
        var registerX = 1;
        var isExecuting = "";
        var numberToAdd = 0;
        var signalStrengthSum = 0;
        for (var cycle = 1; cycle < targetCycles.Last() + 1; cycle++)
        {
            if (targetCycles.Contains(cycle))
                signalStrengthSum += cycle * registerX;
            if (string.IsNullOrEmpty(isExecuting))
            {
                var cmd = commandQueue.Dequeue();
                if (cmd == "noop") continue;
                var cmdParts = cmd.Split(" ");
                isExecuting = cmdParts.First();
                numberToAdd = int.Parse(cmdParts.Last().Trim());
            }
            else
            {
                registerX += numberToAdd;
                isExecuting = "";
            }
        }
        Console.WriteLine($"AdventOfCode Day 10 Part 1 result: {signalStrengthSum}");
        //Console.WriteLine($"AdventOfCode Day 10 Part 2 result: {visitedPart2.Count()}");
    }
}
