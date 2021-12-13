using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent2021.Solutions.Day13
{
    public record Result(int Part1, string Part2) { }

    public static class Day13
    {
        public static Regex RE_FOLD = new Regex(@"fold along (x|y)=(\d+)");

        public static Result Solve(IEnumerable<string> input)
        {
            List<Pos> dots = input.TakeWhile(line => line != "")
                .Select(ParsePos)
                .ToList();
            List<(string, int)> folds = input.SkipWhile(line => line != "")
                .Skip(1)
                .Select(ParseFold)
                .ToList();

            var nextDots = FoldAlong(dots, folds[0].Item1, folds[0].Item2);
            var part1 = nextDots.Count;

            var page = dots;
            foreach (var (axis, where) in folds)
            {
                page = FoldAlong(page, axis, where);
            }
            var part2 = Render(page);

            return new Result(part1, part2);
        }

        public static Pos ParsePos(string line)
        {
            var coords = line.Split(',');
            return new Pos(int.Parse(coords[0]), int.Parse(coords[1]));
        }

        public static (string, int) ParseFold(string line)
        {
            var match = RE_FOLD.Match(line);
            return (match.Groups[1].Value, int.Parse(match.Groups[2].Value));
        }

        public static List<Pos> FoldAlong(List<Pos> dots, string axis, int where)
        {
            return dots.Select(pos =>
            {
                if (axis == "x")
                {
                    if (pos.X > where)
                        return new Pos(2 * where - pos.X, pos.Y);
                    else
                        return pos;
                }
                else
                {
                    if (pos.Y > where)
                        return new Pos(pos.X, 2 * where - pos.Y);
                    else
                        return pos;

                }
            })
            .Distinct()
            .ToList();
        }

        public static string Render(List<Pos> dots)
        {
            var width = dots.Select(pos => pos.X).Max() + 1;
            var height = dots.Select(pos => pos.Y).Max() + 1;
            string[,] grid = new string[height, width];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    grid[i, j] = ".";

            foreach (var dot in dots)
                grid[dot.Y, dot.X] = "#";

            var s = string.Join('\n', Enumerable.Range(0, height)
                .Select(row => string.Join("",
                    Enumerable.Range(0, width).Select(col => grid[row, col]))));

            return s;
        }
    }
}
