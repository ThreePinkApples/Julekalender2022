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
                Items = new(new ulong[] { 79, 89 }),
                Operation = Tuple.Create("old", '*', "19"),
                Test = new MonkeyTest
                {
                    Condition = Tuple.Create("divisible", 23UL),
                    TrueMonkeyId = 2,
                    FalseMonkeyId = 3,
                }
            },
            new Monkey
            {
                Id = 1,
                Items = new(new ulong[] { 54, 65, 75, 74 }),
                Operation = Tuple.Create("old", '+', "6"),
                Test = new MonkeyTest
                {
                    Condition = Tuple.Create("divisible", 19UL),
                    TrueMonkeyId = 2,
                    FalseMonkeyId = 0,
                }
            },
            new Monkey
            {
                Id = 2,
                Items = new(new ulong[] { 79, 60, 97 }),
                Operation = Tuple.Create("old", '*', "old"),
                Test = new MonkeyTest
                {
                    Condition = Tuple.Create("divisible", 23UL),
                    TrueMonkeyId = 1,
                    FalseMonkeyId = 3,
                }
            },
            new Monkey
            {
                Id = 3,
                Items = new(new ulong[] { 74 }),
                Operation = Tuple.Create("old", '+', "3"),
                Test = new MonkeyTest
                {
                    Condition = Tuple.Create("divisible", 23UL),
                    TrueMonkeyId = 0,
                    FalseMonkeyId = 1,
                }
            },
        };
        var modularThingy = exampleMonkeys.Aggregate(1UL, (agg, b) => agg * (ulong)b.Test.Condition.Item2);
        var numberOfRounds = 10_000;
        for (var round = 0; round < numberOfRounds; round++)
        {
            if (round == 20)
                Console.WriteLine("Debugging");
            foreach (var monkey in exampleMonkeys)
            {
                monkey.RunInspection(exampleMonkeys, part2WorryReducer: modularThingy);
            }
        }
        var mostActiveMonkeys = exampleMonkeys.OrderByDescending(m => m.NumberOfInspectedItems).Take(2).ToList();
        Console.WriteLine($"AdventOfCode Day 11 Part 2 result: {mostActiveMonkeys[0].NumberOfInspectedItems * mostActiveMonkeys[1].NumberOfInspectedItems}");
    }
}

public class Monkey
{
    public int Id { get; set; }
    public Queue<ulong> Items { get; set; }
    public Tuple<string, char, string> Operation { get; set; }
    public MonkeyTest Test { get; set; }
    public ulong NumberOfInspectedItems { get; set; }

    public void RunInspection(List<Monkey> monkeys, ulong part2WorryReducer = 0)
    {
        while (Items.Count > 0)
        {
            NumberOfInspectedItems++;
            var item = Items.Dequeue();
            item = InspectItem(item);
            item = ApplyRelief(item, part2WorryReducer);
            var monkeyId = Test.Test(item);
            ThrowItem(item, monkeys[monkeyId]);
        }
    }

    private ulong InspectItem(ulong item)
    {
        if (Operation.Item1 != "old")
            throw new Exception($"Unknown operation {Operation}");
        switch (Operation.Item2)
        {
            case '+':
                if (Operation.Item3 == "old")
                    item += item;
                else
                    item += ulong.Parse(Operation.Item3);
                break;
            case '-':
                if (Operation.Item3 == "old")
                    item -= item;
                else
                    item -= ulong.Parse(Operation.Item3);
                break;
            case '*':
                if (Operation.Item3 == "old")
                    item *= item;
                else
                    item *= ulong.Parse(Operation.Item3);
                break;
            case '/':
                if (Operation.Item3 == "old")
                    item /= item;
                else
                    item /= ulong.Parse(Operation.Item3);
                break;
        }
        return item;
    }

    private ulong ApplyRelief(ulong item, ulong part2WorryReducer)
    {
        if (part2WorryReducer == 0)
            return (ulong)Math.Floor(item / 3m);
        return item % part2WorryReducer;
    }

    public void ThrowItem(ulong item, Monkey receiver)
    {
        receiver.ReceiveItem(item);
    }

    public void ReceiveItem(ulong item)
    {
        Items.Enqueue(item);
    }

    public static Monkey FromInput(string input)
    {
        var monkey = new Monkey();
        var parts = input.Split("\n").Select(part => part.Trim()).ToArray();
        monkey.Id = int.Parse(parts[0].Replace("Monkey ", "").Replace(":", ""));
        monkey.Items = new Queue<ulong>(
            parts[1]
                .Replace("Starting items: ", "")
                .Split(",")
                .Select(item => ulong.Parse(item.Trim()))
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
    public Tuple<string, ulong> Condition { get; set; }
    public int TrueMonkeyId { get; set; }
    public int FalseMonkeyId { get; set; }

    public int Test(ulong item)
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
        test.Condition = Tuple.Create(conditionParts[0], ulong.Parse(conditionParts[1]));
        var trueParts = inputParts[4].Replace("If true: ", "").Split(" to monkey ");
        test.TrueMonkeyId = int.Parse(trueParts[1]);
        var falseParts = inputParts[5].Replace("If false: ", "").Split(" to monkey ");
        test.FalseMonkeyId = int.Parse(falseParts[1]);
        return test;
    }
}
