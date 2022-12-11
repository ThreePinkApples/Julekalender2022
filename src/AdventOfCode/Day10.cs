using System.Text;

namespace AdventCalendar2022.AdventOfCode;
internal class Day10
{
    public static void Run()
    {
        var commandQueue = new Queue<string>(File.ReadAllLines("AdventOfCode/Data/Day10Input.txt"));

        var targetCycles = new[] { 20, 60, 100, 140, 180, 220 };
        var screen = new int[40, 6];
        var totalPixels = screen.Length;
        var registerX = 1;
        var cmdExecuting = "";
        var numberToAdd = 0;
        var signalStrengthSum = 0;
        var cycle = 0;
        while (commandQueue.Count > 0)
        {
            cycle++;
            var currentPixel = (cycle - 1) % (totalPixels - 1);
            var currentScreenRow = (int)Math.Floor((decimal)currentPixel / screen.GetLength(0));
            var currentScreenColumn = currentPixel % screen.GetLength(0);
            if (registerX == currentScreenColumn || registerX == currentScreenColumn + 1 || registerX == currentScreenColumn - 1)
                screen[currentScreenColumn, currentScreenRow] = 1;

            if (targetCycles.Contains(cycle))
                signalStrengthSum += cycle * registerX;
            if (string.IsNullOrEmpty(cmdExecuting))
            {
                var cmd = commandQueue.Dequeue();
                if (cmd == "noop") continue;
                var cmdParts = cmd.Split(" ");
                cmdExecuting = cmdParts.First();
                numberToAdd = int.Parse(cmdParts.Last().Trim());
            }
            else
            {
                registerX += numberToAdd;
                cmdExecuting = "";
            }
        }
        Console.WriteLine($"AdventOfCode Day 10 Part 1 result: {signalStrengthSum}");
        Console.WriteLine($"AdventOfCode Day 10 Part 2 result:");
        RenderScreen(screen);
    }

    public static void RenderScreen(int[,] screen)
    {
        var builder = new StringBuilder();
        for (var rowIndex = 0; rowIndex < screen.GetLength(1); rowIndex++)
        {
            var rowBuilder = new StringBuilder();
            for (var columnIndex = 0; columnIndex < screen.GetLength(0); columnIndex++)
            {
                rowBuilder.Append(screen[columnIndex, rowIndex] == 1 ? "#" : ".");
            }
            builder.AppendLine(rowBuilder.ToString());
        }
        Console.WriteLine(builder.ToString());
    }
}
