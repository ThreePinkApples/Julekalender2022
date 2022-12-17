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
        //var packetPairStrings = File.ReadAllText("AdventOfCode/Data/Day13Input.txt")
        //    .Split("\n\n")
        //    .Where(line => !string.IsNullOrWhiteSpace(line));
        var packetPairStrings = sample
            .Split("\r\n\r\n")
            .Where(line => !string.IsNullOrWhiteSpace(line));
        foreach (var packetPairString in packetPairStrings)
        {
            ComparePair(packetPairString.Trim());
        }
        //Console.WriteLine($"AdventOfCode Day 13 Part 1 result: {signalStrengthSum}");
        //Console.WriteLine($"AdventOfCode Day 13 Part 2 result:");
    }

    private static bool? ComparePair(string packetPairString)
    {
        var packets = packetPairString.Split("\n").Select(ps => Packet.FromPacketString(ps.Trim()).Item1).ToList();
        var leftPacket = packets.First();
        var rightPacket = packets.Last();
        Packet currentLeftPacket = leftPacket;
        Packet currentRightPacket = rightPacket;
        var result = ComparePair(leftPacket, rightPacket);
        Console.WriteLine(result?.ToString() ?? "Oh no");
        return result;
    }

    private static bool? ComparePair(Packet left, Packet right)
    {
        //var leftChildIndex = 0;
        //var rightChildIndex = 0;
        var largestPacket = left.Children.Count < right.Children.Count ? right.Children.Count : left.Children.Count;
        for (var childIndex = 0; childIndex < largestPacket; childIndex++)
        {
            if (childIndex == left.Children.Count)
                // Left has run out of items, in the right order
                return true;
            else if (childIndex == right.Children.Count)
                // Right has run out of items, not in the right order
                return false;
            var childLeft = left.Children[childIndex];
            var childRight = right.Children[childIndex];
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
            var childCompare = ComparePair(childLeft, childRight);
            if (childCompare != null) return childCompare;
        }
        return null;
    }
}

public class Packet
{
    public int? Item { get; set; } = null;
    public bool IsItem { get { return Children.Count == 0; } }
    public List<Packet> Children = new List<Packet>();

    public Packet() { }

    public Packet(int item)
    {
        Item = item;
    }

    public void AddItem(int item)
    {
        // All values are wrapped in a child object so that everything ends up being a list with a single value in the end.
        Children.Add(new Packet(item));
    }

    public void AddChild(Packet child)
    {
        Children.Add(child);
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
            {
                if (packet.IsItem && packet.Item == null)
                    // Empty packet, still need child to wrap the empty value
                    packet.AddChild(new Packet());
                return Tuple.Create(packet, charIndex);
            }
            if (currentChar == ',') continue;
            if (currentChar == '[')
            {
                var child = Packet.FromPacketString(packetString.Substring(charIndex));
                // All values are wrapped in a child object so that everything ends up being a list with a single value in the end.
                packet.AddChild(child.Item1);
                charIndex += child.Item2;
            }
            else
            {
                numberBuilder += currentChar;
            }
        }
        return null;
    }
}
