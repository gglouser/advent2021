using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent2021.Solutions.Day08
{
    public record Result(int Part1, int Part2) { }

    public record Display(string[] signals, string[] output) { }

    public static class Day08
    {
        public static Result Solve(IEnumerable<string> input)
        {
            List<Display> displays = input.Select(line =>
                {
                    var x = line.Split(" | ");
                    var signals = x[0].Split(" ").Select(SortStr).ToArray();
                    var output = x[1].Split(" ").Select(SortStr).ToArray();
                    return new Display(signals, output);
                })
                .ToList();

            var part1 = displays
                .Select(digs =>
                    digs.output
                        .Where(d => d.Length == 2 || d.Length == 3 || d.Length == 4 || d.Length == 7)
                        .Count())
                .Sum();
            var part2 = displays.Select(Decode).Sum();

            return new Result(part1, part2);
        }

        public static string SortStr(string s)
        {
            var cs = s.ToCharArray();
            Array.Sort(cs);
            return new string(cs);
        }

        public static int Decode(Display display)
        {
            var signals = display.signals;
            var one = signals.Where(s => s.Length == 2).First();
            var seven = signals.Where(s => s.Length == 3).First();
            var four = signals.Where(s => s.Length == 4).First();
            var eight = signals.Where(s => s.Length == 7).First();

            var is235 = signals.Where(s => s.Length == 5);
            var is069 = signals.Where(s => s.Length == 6);

            //  a
            // b c
            //  d
            // e f
            //  g
            var isA = seven.Where(c => !one.Contains(c)).First();
            var isC = one.Where(c => is069.Any(s => !s.Contains(c))).First();
            var isBorD = four.Where(c => !one.Contains(c)).ToArray();
            
            var six = is069.Where(s => !s.Contains(isC)).First();
            var five = is235.Where(s => !s.Contains(isC)).First();

            var isE = six.Where(c => !five.Contains(c)).First();

            var three = is235.Where(s => s != five && !s.Contains(isE)).First();
            var two = is235.Where(s => s != three && s != five).First();

            var nine = is069.Where(s => !s.Contains(isE)).First();
            var zero = is069.Where(s => s != six && s != nine).First();

            var q = display.output.Select(o => {
                if (o == zero)
                    return 0;
                else if (o == one)
                    return 1;
                else if (o == two)
                    return 2;
                else if (o == three)
                    return 3;
                else if (o == four)
                    return 4;
                else if (o == five)
                    return 5;
                else if (o == six)
                    return 6;
                else if (o == seven)
                    return 7;
                else if (o == eight)
                    return 8;
                else
                    return 9;
            }).ToList();

            return q.Aggregate(0, (a,b) => 10*a + b);
        }
    }
}
