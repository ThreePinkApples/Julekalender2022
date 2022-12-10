namespace AdventCalendar2022.AdventOfCode;

internal class Day2
{
    internal enum Move
    {
        Rock,
        Paper,
        Scissor
    }
    internal enum Result
    {
        Lose,
        Draw,
        Win
    }

    public static Dictionary<string, Move> MoveMapPart1 = new()
    {
        { "A", Move.Rock },
        { "B", Move.Paper },
        { "C", Move.Scissor },
        { "X", Move.Rock },
        { "Y", Move.Paper },
        { "Z", Move.Scissor }
    };

    public static Dictionary<string, Result> MoveMapPart2 = new()
    {
        { "X", Result.Lose },
        { "Y", Result.Draw },
        { "Z", Result.Win }
    };

    public static Dictionary<Move, int> MoveScoreMapping = new()
    {
        { Move.Rock, 1 },
        { Move.Paper, 2 },
        { Move.Scissor, 3 }
    };

    public static Dictionary<Result, int> ResultMapping = new()
    {
        { Result.Lose, 0 },
        { Result.Draw, 3 },
        { Result.Win, 6 },
    };

    public static void Run()
    {
        var moves = File.ReadAllLines("AdventOfCode/Data/Day2Input.txt")
            .Select(m => m.Split(" ").ToArray());
        var part1FinalScore = moves.Select(m => PlayRoundPart1Rules(m[0], m[1])).Sum();
        Console.WriteLine($"AdventOfCode Day 2 Part 1 result: {part1FinalScore}");

        var part2FinalScore = moves.Select(m => PlayRoundPart2Rules(m[0], m[1])).Sum();
        Console.WriteLine($"AdventOfCode Day 2 Part 2 result: {part2FinalScore}");
    }

    public static int PlayRoundPart1Rules(string elfMoveCoded, string myMoveCoded)
    {
        var elfMove = MoveMapPart1[elfMoveCoded];
        var myMove = MoveMapPart1[myMoveCoded];
        if (elfMove == myMove)
            return ResultMapping[Result.Draw] + MoveScoreMapping[myMove];
        else if (elfMove is Move.Rock && myMove is Move.Paper)
            return ResultMapping[Result.Win] + MoveScoreMapping[myMove];
        else if (elfMove is Move.Rock)
            return ResultMapping[Result.Lose] + MoveScoreMapping[myMove];
        else if (elfMove is Move.Paper && myMove is Move.Scissor)
            return ResultMapping[Result.Win] + MoveScoreMapping[myMove];
        else if (elfMove is Move.Paper)
            return ResultMapping[Result.Lose] + MoveScoreMapping[myMove];
        else if (elfMove is Move.Scissor && myMove is Move.Rock)
            return ResultMapping[Result.Win] + MoveScoreMapping[myMove];
        return ResultMapping[Result.Lose] + MoveScoreMapping[myMove];
    }

    public static int PlayRoundPart2Rules(string elfMoveCoded, string myResultCoded)
    {
        var elfMove = MoveMapPart1[elfMoveCoded];
        var myResult = MoveMapPart2[myResultCoded];
        if (myResult is Result.Win)
        {
            if (elfMove is Move.Rock)
                return ResultMapping[Result.Win] + MoveScoreMapping[Move.Paper];
            else if (elfMove is Move.Paper)
                return ResultMapping[Result.Win] + MoveScoreMapping[Move.Scissor];
            return ResultMapping[Result.Win] + MoveScoreMapping[Move.Rock];
        }
        else if (myResult is Result.Lose)
        {
            if (elfMove is Move.Rock)
                return ResultMapping[Result.Lose] + MoveScoreMapping[Move.Scissor];
            else if (elfMove is Move.Paper)
                return ResultMapping[Result.Lose] + MoveScoreMapping[Move.Rock];
            return ResultMapping[Result.Lose] + MoveScoreMapping[Move.Paper];
        }
        return ResultMapping[Result.Draw] + MoveScoreMapping[elfMove];
    }
}
