using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent2021.Solutions.Day05
{
    public record Result(int Part1, int Part2) { }

    public static class Day05
    {
        public static Regex RE_LINE = new Regex(@"(\d+),(\d+) -> (\d+),(\d+)");

        public struct Line
        {
            public Pos Start;
            public Pos End;

            public static Line Parse(string s)
            {
                var m = RE_LINE.Match(s);
                var x1 = int.Parse(m.Groups[1].Value);
                var y1 = int.Parse(m.Groups[2].Value);
                var x2 = int.Parse(m.Groups[3].Value);
                var y2 = int.Parse(m.Groups[4].Value);
                return new Line { Start = new Pos(x1, y1), End = new Pos(x2, y2) };
            }

            public bool IsHOrV()
            {
                return Start.X == End.X || Start.Y == End.Y;
            }

            public Pos Delta()
            {
                return new Pos(Math.Sign(End.X - Start.X), Math.Sign(End.Y - Start.Y));
            }

            public IEnumerable<Pos> Points()
            {
                var delta = Delta();
                var stop = End + delta;
                for (var p = Start; !p.Equals(stop); p += delta)
                {
                    yield return p;
                }
            }
        }

        public static Result Solve(IEnumerable<string> input)
        {
            List<Line> ventLines = input.Select(Line.Parse).ToList();

            var part1 = CountIntersections(ventLines.Where(line => line.IsHOrV()).ToList());
            var part2 = CountIntersections(ventLines);

            return new Result(part1, part2);
        }

        private static int CountIntersections(List<Line> ventLines)
        {
            Dictionary<Pos, int> points = new Dictionary<Pos, int>();
            foreach (var ventLine in ventLines)
            {
                foreach (var p in ventLine.Points())
                {
                    if (points.ContainsKey(p))
                    {
                        points[p] += 1;
                    }
                    else
                    {
                        points.Add(p, 1);
                    }
                }
            }

            return points.Values.Where(n => n > 1).Count();
        }
    }
}
