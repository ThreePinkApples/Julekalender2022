namespace AdventCalendar2022.AdventOfCode;
internal class Day13
{
    public static void Run()
    {
        var sample = @"[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]
";
        var packetPairStrings = File.ReadAllText("AdventOfCode/Data/Day13Input.txt")
            .Split("\n\n")
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line.Trim().Split("\n").Select(ps => Packet.FromPacketString(ps.Trim()).Item1))
            .SelectMany(p => p)
            .ToArray();
        //packetPairStrings = sample
        //    .Split("\r\n\r\n")
        //    .Where(line => !string.IsNullOrWhiteSpace(line))
        //    .Select(line => line.Trim().Split("\n").Select(ps => Packet.FromPacketString(ps.Trim()).Item1))
        //    .SelectMany(p => p)
        //    .ToArray();
        var indexesInRightOrder = new List<int>();
        var index = 0;
        foreach (var packetPair in packetPairStrings.Chunk(2))
        {
            index++;
            if (ComparePair(packetPair))
                indexesInRightOrder.Add(index);
        }
        Console.WriteLine($"AdventOfCode Day 13 Part 1 result: {indexesInRightOrder.Sum()}");
        //Console.WriteLine($"AdventOfCode Day 13 Part 2 result:");
    }

    private static bool ComparePair(Packet[] packetPair)
    {
        var leftPacket = packetPair.First();
        var rightPacket = packetPair.Last();
        var result = ComparePair(leftPacket, rightPacket);
        Console.WriteLine(result);
        return result.Value;
    }

    private static bool? ComparePair(Packet left, Packet right, string indent = "")
    {
        var largestPacket = left.Children.Count < right.Children.Count ? right.Children.Count : left.Children.Count;
        if (indent == "")
            Console.WriteLine($"{indent}- Compare {left} vs {right}");
        indent += "  ";
        for (var childIndex = 0; childIndex < largestPacket; childIndex++)
        {
            if (childIndex == left.Children.Count)
            {
                // Left has run out of items, in the right order
                Console.WriteLine($"{indent}- Left side ran out of items");
                return true;
            }
            else if (childIndex == right.Children.Count)
            {
                // Right has run out of items, not in the right order
                Console.WriteLine($"{indent}- Right side ran out of items");
                return false;
            }
            var childLeft = left.Children[childIndex];
            var childRight = right.Children[childIndex];
            Console.WriteLine($"{indent}- Compare {childLeft} vs {childRight}");
            if (childLeft.IsItem && childRight.IsItem)
            {
                if (childLeft.Item == null && childRight.Item == null)
                    return null;
                if (childLeft.Item == null)
                    // Empty
                    return true;
                if (childRight.Item == null)
                    // Empty
                    return false;
                // Both are integer
                if (childLeft.Item != childRight.Item)
                    return childLeft.Item < childRight.Item;
                continue;
            }
            if (childLeft.IsItem)
            {
                childLeft = new Packet(childLeft);
                Console.WriteLine($"{indent}  - Mixed types; convert left side to {childLeft} and retry comparison");
            }
            else if (childRight.IsItem)
            {
                childRight = new Packet(childRight);
                Console.WriteLine($"{indent}  - Mixed types; convert right side to {childRight} and retry comparison");
            }
            var childCompare = ComparePair(childLeft, childRight, indent + "  ");
            if (childCompare != null) return childCompare;
        }
        return null;
    }
}

public class Packet
{
    public int? Item { get; set; } = null;
    public bool IsItem { get { return Item != null; } }
    public List<Packet> Children = new List<Packet>();

    public Packet() { }

    public Packet(Packet child)
    {
        Children.Add(child);
    }

    public Packet(int? item)
    {
        Item = item;
    }

    public void AddItem(int? item)
    {
        AddChild(new Packet(item));
    }

    public void AddChild(Packet child)
    {
        Children.Add(child);
    }

    public override string ToString()
    {
        if (IsItem)
            return Item?.ToString() ?? string.Empty;
        return $"[{string.Join(',', Children)}]";
    }

    public static Tuple<Packet, int> FromPacketString(string packetString)
    {
        if (packetString[0] != '[')
            throw new ArgumentException();
        var packet = new Packet();
        var numberBuilder = "";
        var specialChars = new char[] { '[', ',', ']' };
        for (var charIndex = 1; charIndex < packetString.Length; charIndex++)
        {
            var currentChar = packetString[charIndex];
            if (specialChars.Contains(currentChar) && numberBuilder.Length > 0)
            {
                packet.AddItem(int.Parse(numberBuilder));
                numberBuilder = "";
            }
            if (currentChar == ']')
                return Tuple.Create(packet, charIndex);
            if (currentChar == ',') continue;
            if (currentChar == '[')
            {
                var child = Packet.FromPacketString(packetString.Substring(charIndex));
                packet.AddChild(child.Item1);
                charIndex += child.Item2;
            }
            else
                numberBuilder += currentChar;
        }
        throw new Exception();
    }
}
