using System.Drawing;

namespace AdventCalendar2022.AdventOfCode;
internal class Day9
{
    internal record class Point
    {
        public int X { get; set; }
        public int Y { get; set; }


    }

    public static void Run()
    {
        var moves = File.ReadAllLines("AdventOfCode/Data/Day9Input.txt")
            .Select(line => line.Split(" "))
            .Select(parts => Tuple.Create(parts.First(), int.Parse(parts.Last())))
            .ToList();
        var testMoves = new List<Tuple<string, int>>()
        {
            new("R", 5 ),
            new("U", 8 ),
            new("L", 8 ),
            new("D", 3 ),
            new("R", 17 ),
            new("D", 10 ),
            new("L", 25 ),
            new("U", 20 )
        };
        var knots = new Point[]
        {
            new Point { X = 0, Y = 0 },
            new Point { X = 0, Y = 0 },
            new Point { X = 0, Y = 0 },
            new Point { X = 0, Y = 0 },
            new Point { X = 0, Y = 0 },
            new Point { X = 0, Y = 0 },
            new Point { X = 0, Y = 0 },
            new Point { X = 0, Y = 0 },
            new Point { X = 0, Y = 0 },
            new Point { X = 0, Y = 0 }
        };
        var visitedPart1 = new HashSet<Point>()
        {
            new Point { X = 0, Y = 0 }
        };
        var visitedPart2 = new HashSet<Point>()
        {
            new Point { X = 0, Y = 0 }
        };
        foreach (var move in moves)
        {
            for (var step = 0; step < move.Item2; step++)
            {
                switch (move.Item1)
                {
                    case "U":
                        MoveUp(knots);
                        break;
                    case "D":
                        MoveDown(knots);
                        break;
                    case "R":
                        MoveRight(knots);
                        break;
                    case "L":
                        MoveLeft(knots);
                        break;
                }
                Follow(knots);
                visitedPart1.Add(knots[1]);
                visitedPart2.Add(knots[9]);
            }
        }
        Console.WriteLine($"AdventOfCode Day 9 Part 1 result: {visitedPart1.Count()}");
        // Cheated a bit on Part 2. I re-ran my logic from Part 1 and got the wrong result and was unable to understand why it would be wrong given that the task description said
        // "Each knot further down the rope follows the knot in front of it using the same rules as before.".
        // But then I Google the solution/discussions and see that the movement does change to also being diagonal? I found no explanation for why this is though.
        // The task description tries to hint at this with "However, be careful: more types of motion are possible than before, so you might want to visually compare your simulated rope to the one above."
        // and I could see in the examples that they did move diagonally, but I was unable to understand the reason behind this.
        Console.WriteLine($"AdventOfCode Day 9 Part 2 result: {visitedPart2.Count()}");
    }

    private static void MoveUp(Point[] knots)
    {
        knots[0].Y += 1;
    }

    private static void MoveDown(Point[] knots)
    {
        knots[0].Y -= 1;
    }

    private static void MoveRight(Point[] knots)
    {
        knots[0].X += 1;
    }

    private static void MoveLeft(Point[] knots)
    {
        knots[0].X -= 1;
    }

    private static void Follow(Point[] knots)
    {
        for (var knotIndex = 1; knotIndex < knots.Length; knotIndex++)
        {
            var diff = new Point
            {
                X = Math.Abs(knots[knotIndex - 1].X - knots[knotIndex].X),
                Y = Math.Abs(knots[knotIndex - 1].Y - knots[knotIndex].Y)
            };
            if (diff.Y >= 2)
            {
                knots[knotIndex].Y += (knots[knotIndex - 1].Y - knots[knotIndex].Y) / 2;
                if (diff.X >= 2)
                    knots[knotIndex].X += (knots[knotIndex - 1].X - knots[knotIndex].X) / 2;
                else
                    knots[knotIndex].X += knots[knotIndex - 1].X - knots[knotIndex].X;
            }
            else if (diff.X >= 2)
            {
                knots[knotIndex].X += (knots[knotIndex - 1].X - knots[knotIndex].X) / 2;
                knots[knotIndex].Y += knots[knotIndex - 1].Y - knots[knotIndex].Y;
            }
        }
    }
}
