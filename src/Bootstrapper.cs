using System.Reflection;

namespace AdventCalendar2022;
public class Bootstrapper
{
    public static void Start()
    {
        Console.WriteLine("Pick a calendar and day");

        string? line;
        while ((line = Console.ReadLine()) != null)
        {
            try
            {
                ProcessLine(line);
            }
            catch (Exception ex)
            {
                LogError("Failure", ex);
            }
        }
    }

    private static void ProcessLine(string line)
    {
        var parts = line.Split(' ');
        var calendar = parts[0];
        int day;
        try
        {
            day = Int32.Parse(parts[1]);
        }
        catch (FormatException)
        {
            LogError($"{parts[1]} is not a number.");
            return;
        }
        if (day < 1 || day > 24)
        {
            LogError($"{day} is not a valid christmas calendar day!");
            return;
        }
        switch (calendar)
        {
            case "knowit":
                Run("Knowit", day);
                break;
            case "advent":
                Run("AdventOfCode", day);
                break;
            default:
                LogError("Unknown calendar");
                break;
        }
    }

    private static void Run(string calendar, int day)
    {
        var t = Type.GetType($"AdventCalendar2022.{calendar}.Day{day}");
        if (t == null)
        {
            LogError($"Day {day} is not implemeted yet for {calendar}");
            return;
        }
        var method = t.GetMethod("Run", BindingFlags.Static | BindingFlags.Public);
        method.Invoke(null, null);
    }

    private static void LogError(string error)
    {
        LogError(error, null);
    }

    private static void LogError(string error, Exception? ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"You've been naughty!\n{error}", ex);
        Console.ResetColor();
    }
}
