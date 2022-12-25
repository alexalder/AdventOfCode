global using AdventOfCodeUtils;
using System;
using System.Reflection;

namespace AdventOfCode
{
    class MainClass
    {
        public static void Main()
        {
            var watch = new System.Diagnostics.Stopwatch();

            // Enter day.
            int day = 16;
            int year = 2022;

            bool live = true;

            if (live && DateTime.Now.Month == 12 && DateTime.Now.Day < 26)
            {
                day = DateTime.Now.Day;
                year = DateTime.Now.Year;
            }

            var assembly = typeof(MainClass).Assembly;
            var dayType = assembly.GetType("AdventOfCode" + year + ".Day" + day.ToString("D2"));
            var method = dayType!.GetMethod("Run");

            Console.WriteLine("Running Year " + year + ": Day " + day);

            watch.Start();
            method!.Invoke(null, Array.Empty<object>());
            watch.Stop();

            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }
    }
}
