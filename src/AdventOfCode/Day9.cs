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
        var startX = 10000;
        var startY = 10000;
        var knots = new Point[]
        {
            new Point { X = startX, Y = startY },
            new Point { X = startX, Y = startY },
            new Point { X = startX, Y = startY },
            new Point { X = startX, Y = startY },
            new Point { X = startX, Y = startY },
            new Point { X = startX, Y = startY },
            new Point { X = startX, Y = startY },
            new Point { X = startX, Y = startY },
            new Point { X = startX, Y = startY },
            new Point { X = startX, Y = startY }
        };
        var visitedPart1 = new HashSet<Point>()
        {
            new Point { X = startX, Y = startY }
        };
        var visitedPart2 = new HashSet<Point>()
        {
            new Point { X = startX, Y = startY }
        };
        foreach (var move in moves)
        {
            for (var step = 0; step < move.Item2; step++)
            {
                switch (move.Item1)
                {
                    case "U":
                        knots[0].Y += 1;
                        break;
                    case "D":
                        knots[0].Y -= 1;
                        break;
                    case "R":
                        knots[0].X += 1;
                        break;
                    case "L":
                        knots[0].X -= 1;
                        break;
                }
                Follow(knots);
                visitedPart1.Add(knots[1]);
                visitedPart2.Add(knots[9]);
            }
        }
        Console.WriteLine($"AdventOfCode Day 9 Part 1 result: {visitedPart1.Count()}");
        Console.WriteLine($"AdventOfCode Day 9 Part 2 result: {visitedPart2.Count()}");
    }

    private static void Follow(Point[] knots)
    {
        for (var knotIndex = 1; knotIndex < knots.Length; knotIndex++)
        {
            var diff = new Point
            {
                X = knots[knotIndex - 1].X - knots[knotIndex].X,
                Y = knots[knotIndex - 1].Y - knots[knotIndex].Y
            };
            if (Math.Abs(diff.Y) == 2)
            {
                // Previous knot moved UP or DOWN
                knots[knotIndex].Y += diff.Y / 2;
                if (Math.Abs(diff.X) == 2)
                    // Incase knot also moved RIGHT or LEFT
                    knots[knotIndex].X += diff.X / 2;
                else
                    // Allign on X-axis
                    knots[knotIndex].X += diff.X;
            }
            else if (Math.Abs(diff.X) == 2)
            {
                // Previous knot moved RIGHT or LEFT
                knots[knotIndex].X += diff.X / 2;
                // Allign on Y-axis
                knots[knotIndex].Y += diff.Y;
            }
        }
    }
}
