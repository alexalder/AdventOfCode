using System;
using System.Reflection;

namespace AdventOfCode
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var watch = new System.Diagnostics.Stopwatch();

            // Enter day.
            int day = 1;
            int year = 2021;
            bool live = true;
            if (live && DateTime.Now.Month == 12 && DateTime.Now.Day < 26)
            {
                day = DateTime.Now.Day;
                year = DateTime.Now.Year;
            }

            var assembly = typeof(MainClass).Assembly;
            var dayType = assembly.GetType("AdventOfCode" + year + ".Day" + day.ToString("D2"));
            var method = dayType.GetMethod("Run");

            watch.Start();
            method.Invoke(null, new object[] { });
            watch.Stop();

            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }
    }
}
