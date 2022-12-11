namespace AdventCalendar2022.AdventOfCode;
internal class Day11
{
    public static void Run()
    {
        var input = File.ReadAllText("AdventOfCode/Data/Day11Input.txt").Split("\n\n");
        Part1(input);
        Part2(input);
    }

    private static List<Monkey> ParseInput(string[] input)
    {
        return input
            .Select(monkeyString => monkeyString.Trim())
            .Select(Monkey.FromInput)
            .OrderBy(m => m.Id)
            .ToList();
    }

    public static void Part1(string[] input)
    {
        var monkeys = ParseInput(input);
        var numberOfRounds = 20;
        for (var round = 0; round < numberOfRounds; round++)
        {
            foreach (var monkey in monkeys)
            {
                monkey.RunInspection(monkeys);
            }
        }
        var mostActiveMonkeys = monkeys.OrderByDescending(m => m.NumberOfInspectedItems).Take(2).ToList();
        Console.WriteLine($"AdventOfCode Day 11 Part 1 result: {mostActiveMonkeys[0].NumberOfInspectedItems * mostActiveMonkeys[1].NumberOfInspectedItems}");
    }

    public static void Part2(string[] input)
    {
        // I do not in anyway have the required math skills to understand this part
        var monkeys = ParseInput(input);
        var exampleMonkeys = new List<Monkey>()
        {
            new Monkey
            {
                Id = 0,
                Items = new(new long[] { 79, 89 }),
                Operation = Tuple.Create("old", '*', "19"),
                Test = new MonkeyTest
                {
                    Condition = Tuple.Create("divisible", 23),
                    TrueMonkeyId = 2,
                    FalseMonkeyId = 3,
                }
            },
            new Monkey
            {
                Id = 1,
                Items = new(new long[] { 54, 65, 75, 74 }),
                Operation = Tuple.Create("old", '+', "6"),
                Test = new MonkeyTest
                {
                    Condition = Tuple.Create("divisible", 19),
                    TrueMonkeyId = 2,
                    FalseMonkeyId = 0,
                }
            },
            new Monkey
            {
                Id = 2,
                Items = new(new long[] { 79, 60, 97 }),
                Operation = Tuple.Create("old", '*', "old"),
                Test = new MonkeyTest
                {
                    Condition = Tuple.Create("divisible", 13),
                    TrueMonkeyId = 1,
                    FalseMonkeyId = 3,
                }
            },
            new Monkey
            {
                Id = 3,
                Items = new(new long[] { 74 }),
                Operation = Tuple.Create("old", '+', "3"),
                Test = new MonkeyTest
                {
                    Condition = Tuple.Create("divisible", 17),
                    TrueMonkeyId = 0,
                    FalseMonkeyId = 1,
                }
            },
        };
        var numberOfRounds = 10_000;
        for (var round = 0; round < numberOfRounds; round++)
        {
            if (round == 20)
                Console.WriteLine("Debugging");
            foreach (var monkey in exampleMonkeys)
            {
                monkey.RunInspection(exampleMonkeys, part2Rules: true);
            }
        }
        var mostActiveMonkeys = exampleMonkeys.OrderByDescending(m => m.NumberOfInspectedItems).Take(2).ToList();
        Console.WriteLine($"AdventOfCode Day 11 Part 2 result: {mostActiveMonkeys[0].NumberOfInspectedItems * mostActiveMonkeys[1].NumberOfInspectedItems}");
    }
}

public class Monkey
{
    public int Id { get; set; }
    public Queue<long> Items { get; set; }
    public Tuple<string, char, string> Operation { get; set; }
    public MonkeyTest Test { get; set; }
    public long NumberOfInspectedItems { get; set; }

    public void RunInspection(List<Monkey> monkeys, bool part2Rules = false)
    {
        while (Items.Count > 0)
        {
            NumberOfInspectedItems++;
            var item = Items.Dequeue();
            item = InspectItem(item);
            item = ApplyRelief(item, part2Rules);
            var monkeyId = Test.Test(item);
            ThrowItem(item, monkeys[monkeyId]);
        }
    }

    private long InspectItem(long item)
    {
        if (Operation.Item1 != "old")
            throw new Exception($"Unknown operation {Operation}");
        switch (Operation.Item2)
        {
            case '+':
                if (Operation.Item3 == "old")
                    item += item;
                else
                    item += int.Parse(Operation.Item3);
                break;
            case '-':
                if (Operation.Item3 == "old")
                    item -= item;
                else
                    item -= int.Parse(Operation.Item3);
                break;
            case '*':
                if (Operation.Item3 == "old")
                    item *= item;
                else
                    item *= int.Parse(Operation.Item3);
                break;
            case '/':
                if (Operation.Item3 == "old")
                    item /= item;
                else
                    item /= int.Parse(Operation.Item3);
                break;
        }
        return item;
    }

    private long ApplyRelief(long item, bool part2Rules)
    {
        if (!part2Rules)
            return (long)Math.Floor(item / 3m);
        return item;
    }

    public void ThrowItem(long item, Monkey receiver)
    {
        receiver.ReceiveItem(item);
    }

    public void ReceiveItem(long item)
    {
        Items.Enqueue(item);
    }

    public static Monkey FromInput(string input)
    {
        var monkey = new Monkey();
        var parts = input.Split("\n").Select(part => part.Trim()).ToArray();
        monkey.Id = int.Parse(parts[0].Replace("Monkey ", "").Replace(":", ""));
        monkey.Items = new Queue<long>(
            parts[1]
                .Replace("Starting items: ", "")
                .Split(",")
                .Select(item => long.Parse(item.Trim()))
        );
        var operationParts = parts[2].Replace("Operation: new = ", "").Split(" ");
        monkey.Operation = Tuple.Create(operationParts[0], operationParts[1][0], operationParts[2]);
        monkey.Test = MonkeyTest.FromInput(monkey, parts);
        return monkey;
    }
}

public class MonkeyTest
{
    public Monkey Monkey { get; set; }
    public Tuple<string, int> Condition { get; set; }
    public int TrueMonkeyId { get; set; }
    public int FalseMonkeyId { get; set; }

    public int Test(long item)
    {
        if (Condition.Item1 == "divisible")
        {
            if (item % Condition.Item2 == 0)
                return TrueMonkeyId;
            return FalseMonkeyId;
        }
        throw new Exception($"Unknown condition {Condition}");
    }

    public static MonkeyTest FromInput(Monkey monkey, string[] inputParts)
    {
        var test = new MonkeyTest() { Monkey = monkey };
        var conditionParts = inputParts[3].Replace("Test: ", "").Split(" by ");
        test.Condition = Tuple.Create(conditionParts[0], int.Parse(conditionParts[1]));
        var trueParts = inputParts[4].Replace("If true: ", "").Split(" to monkey ");
        test.TrueMonkeyId = int.Parse(trueParts[1]);
        var falseParts = inputParts[5].Replace("If false: ", "").Split(" to monkey ");
        test.FalseMonkeyId = int.Parse(falseParts[1]);
        return test;
    }
}
