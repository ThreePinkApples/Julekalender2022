namespace AdventCalendar2022.AdventOfCode;
internal class Day8
{
    internal record Point(int X, int Y);

    public static void Run()
    {
        var trees = File.ReadAllLines("AdventOfCode/Data/Day8Input.txt")
            .Select(row => row.Select(char.ToString).Select(int.Parse).ToArray())
            .ToArray();
        var treesVisibleFromOutside = (trees.Length * 2) + ((trees[0].Length - 2) * 2);
        var sceneicScore = new Dictionary<Point, int>();
        for (var rowIndex = 1; rowIndex < trees.Length - 1; rowIndex++)
        {
            for (var columnIndex = 1; columnIndex < trees[rowIndex].Length - 1; columnIndex++)
            {
                var treeHeight = trees[rowIndex][columnIndex];
                var sceneicScoreFactors = new List<int>();

                var row = trees[rowIndex];
                var left = row.Take(columnIndex).Reverse();
                var right = row.Skip(columnIndex + 1);

                var column = trees.Select(row => row[columnIndex]);
                var up = column.Take(rowIndex).Reverse();
                var down = column.Skip(rowIndex + 1);

                if (!left.Any(otherThreeHeight => otherThreeHeight >= treeHeight))
                    treesVisibleFromOutside++;
                else if (!right.Any(otherThreeHeight => otherThreeHeight >= treeHeight))
                    treesVisibleFromOutside++;
                else if (!up.Any(otherThreeHeight => otherThreeHeight >= treeHeight))
                    treesVisibleFromOutside++;
                else if (!down.Any(otherThreeHeight => otherThreeHeight >= treeHeight))
                    treesVisibleFromOutside++;

                var leftVisible = left.TakeWhile(height => height < treeHeight);
                var rightVisible = right.TakeWhile(height => height < treeHeight);
                var upVisible = up.TakeWhile(height => height < treeHeight);
                var downVisible = down.TakeWhile(height => height < treeHeight);

                // We must count the first tree we encounter that is equally tall or taller, but also need to account
                // for this tree beign in the outer ring.
                sceneicScoreFactors.Add(leftVisible.Count() + (leftVisible.Count() != left.Count() || left.Last() >= treeHeight ? 1 : 0));
                sceneicScoreFactors.Add(rightVisible.Count() + (rightVisible.Count() != right.Count() || right.Last() >= treeHeight ? 1 : 0));
                sceneicScoreFactors.Add(upVisible.Count() + (upVisible.Count() != up.Count() || up.Last() >= treeHeight ? 1 : 0));
                sceneicScoreFactors.Add(downVisible.Count() + (downVisible.Count() != down.Count() || down.Last() >= treeHeight ? 1 : 0));
                sceneicScore.Add(new Point(rowIndex, columnIndex), sceneicScoreFactors.Aggregate((a, b) => a * b));
            }
        }
        Console.WriteLine($"AdventOfCode Day 8 Part 1 result: {treesVisibleFromOutside}");
        Console.WriteLine($"AdventOfCode Day 8 Part 2 result: {sceneicScore.OrderByDescending(ss => ss.Value).First().Value}");
    }
}
