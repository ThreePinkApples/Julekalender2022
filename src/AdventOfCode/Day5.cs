using System.Text.RegularExpressions;

namespace AdventCalendar2022.AdventOfCode;
internal class Day5
{
    public static void Run()
    {
        var input = File.ReadAllText("AdventOfCode/Data/Day5Input.txt").Split("\n\n").Take(2);
        var rows = input.First()
            .Split("\n")
            .Where(line => line.Contains("["))
            .Select(line => line.Chunk(4).Select(c => string.Join("", c).Trim().Replace("[", "").Replace("]", "")).ToArray())
            .Reverse()
            .ToArray();
        var createMover9000Stacks = new List<Stack<char>>();
        var createMover9001Stacks = new List<Stack<char>>();
        for (var rowIndex = 0; rowIndex < rows.Length; rowIndex++)
        {
            var row = rows[rowIndex];
            for (var columnIndex = 0; columnIndex < row.Length; columnIndex++)
            {
                var column = row[columnIndex];
                if (rowIndex == 0)
                {
                    createMover9000Stacks.Add(new Stack<char>());
                    createMover9001Stacks.Add(new Stack<char>());
                }
                if (!string.IsNullOrEmpty(column))
                {
                    createMover9000Stacks[columnIndex].Push(column[0]);
                    createMover9001Stacks[columnIndex].Push(column[0]);
                }
            }
        }

        var moves = input.Last()
            .Split("\n")
            .Where(move => !string.IsNullOrWhiteSpace(move))
            .Select(move => move
                .Split(" ")
                .Where(part => int.TryParse(part, out var _))
                .Select(int.Parse)
                .ToArray()
            ).ToArray();

        Day5.CrateMover9000(createMover9000Stacks, moves);
        Console.WriteLine($"AdventOfCode Day 4 Part 1 result: {string.Join("", createMover9000Stacks.Select(s => s.Peek()))}");
        Day5.CrateMover9001(createMover9001Stacks, moves);
        Console.WriteLine($"AdventOfCode Day 4 Part 2 result: {string.Join("", createMover9001Stacks.Select(s => s.Peek()))}");
    }

    public static void CrateMover9000(List<Stack<char>> stacks, int[][] moves)
    {
        foreach (var move in moves)
        {
            for (var counter = 0; counter < move[0]; counter++)
            {
                stacks[move[2] - 1].Push(stacks[move[1] - 1].Pop());
            }
        }
    }

    public static void CrateMover9001(List<Stack<char>> stacks, int[][] moves)
    {
        foreach (var move in moves)
        {
            if (move[0] == 1)
                stacks[move[2] - 1].Push(stacks[move[1] - 1].Pop());
            else
            {
                var tempStack = new Stack<char>();
                for (var counter = 0; counter < move[0]; counter++)
                {
                    tempStack.Push(stacks[move[1] - 1].Pop());
                }
                foreach (var box in tempStack)
                    stacks[move[2] - 1].Push(box);
            }
        }
    }
}
