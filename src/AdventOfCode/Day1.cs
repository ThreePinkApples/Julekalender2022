using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCalendar2022.AdventOfCode
{
    internal class Day1
    {
        public static void Run()
        {
            var elfCalories = File.ReadAllText("AdventOfCode/Data/Day1Input.txt")
                .Split("\n\n")
                .Where(elfList => !string.IsNullOrWhiteSpace(elfList))
                .Select(elfList => 
                    elfList.Split("\n")
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(int.Parse)
                    .Sum()
                ).ToList();
            Console.WriteLine($"AdventOfCode Day 1 Part 1 result: {elfCalories.Max()}");

            Console.WriteLine($"AdventOfCode Day 1 Part 2 result: {elfCalories.OrderByDescending(c => c).Take(3).Sum()}");
        }
    }
}
