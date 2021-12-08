using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent2021.Solutions.Day07
{
    public record Result(int Part1, int Part2) { }

    public static class Day07
    {
        public static Result Solve(IEnumerable<string> input)
        {
            var crabs = input.First().Split(',').Select(int.Parse).ToArray();

            var min = crabs.Min();
            var max = crabs.Max();

            int best = int.MaxValue;
            for (int i = min; i < max; i++)
            {
                var cost = Cost(crabs, i);
                if (cost < best)
                {
                    best = cost;
                }
            }
            var part1 = best;

            best = int.MaxValue;
            for (int i = min; i < max; i++)
            {
                var cost = Cost2(crabs, i);
                if (cost < best)
                {
                    best = cost;
                }
            }
            var part2 = best;

            return new Result(part1, part2);
        }

        public static int Cost(int[] crabs, int pos)
        {
            return crabs.Select(crab => Math.Abs(crab - pos)).Sum();
        }

        public static int Cost2(int[] crabs, int pos)
        {
            return crabs.Select(crab => {
                var n = Math.Abs(crab - pos);
                return n * (n + 1) / 2;
            }).Sum();
        }
    }
}
