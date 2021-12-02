using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent2021.Solutions.Day02
{
    public record Result(int Part1, int Part2) { }

    public static class Day02
    {
        public static Result Solve(IEnumerable<string> input)
        {
            var hpos = 0;
            var vpos = 0;
            var vpos2 = 0;

            foreach (var (command, arg) in input.Select(ParseCommand))
            {
                switch (command)
                {
                    case "forward":
                        hpos += arg;
                        vpos2 += vpos * arg;
                        break;
                    case "down":
                        vpos += arg;
                        break;
                    case "up":
                        vpos -= arg;
                        break;
                    default:
                        break;
                }
            }

            var part1 = hpos * vpos;
            var part2 = hpos * vpos2;

            return new Result(part1, part2);
        }

        public static (string, int) ParseCommand(string command)
        {
            var parts = command.Split(' ');
            return (parts[0], int.Parse(parts[1]));
        }
    }
}
