using System.Collections.Generic;
using System.Linq;

namespace Advent2021.Solutions.DayXX
{
    public record Result(int Part1, int Part2) { }

    public static class DayXX
    {
        public static Result Solve(IEnumerable<string> input)
        {
            List<int> nums = input
              .Select(int.Parse)
              .ToList();

            var part1 = 0;
            var part2 = 0;

            return new Result(part1, part2);
        }
    }
}
