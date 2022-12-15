namespace AdventCalendar2022.AdventOfCode;
internal class Day13
{
    public static void Run()
    {
        var packetPairStrings = File.ReadAllText("AdventOfCode/Data/Day13Input.txt")
            .Split("\n\n")
            .Where(line => !string.IsNullOrWhiteSpace(line));
        foreach (var packetPairString in packetPairStrings)
        {
            ComparePair(packetPairString.Trim());
        }
        //Console.WriteLine($"AdventOfCode Day 13 Part 1 result: {signalStrengthSum}");
        //Console.WriteLine($"AdventOfCode Day 13 Part 2 result:");
    }

    private static void ComparePair(string packetPairString)
    {
        var packets = packetPairString.Split("\n").Select(ps => Packet.FromPacketString(ps.Trim())).ToList();
        Console.WriteLine(packets);
    }
}

public class Packet
{
    public int? Item { get; set; } = null;
    public List<Packet> Children = new List<Packet>();

    public Packet() { }

    public Packet(int item)
    {
        Item = item;
    }

    public void AddItem(int item)
    {
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
                return Tuple.Create(packet, charIndex);
            if (currentChar == ',') continue;
            if (currentChar == '[')
            {
                var child = Packet.FromPacketString(packetString.Substring(charIndex));
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
