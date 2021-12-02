using System.Collections.Generic;
using System.Linq;

namespace Advent2021.Solutions.Day01
{
    public record Result(int Part1, int Part2) { }

    public static class Day01
    {
        public static Result Solve(IEnumerable<string> input)
        {
            List<int> nums = input
              .Select(int.Parse)
              .ToList();

            var part1 = CountIncreases(nums, 1);
            var part2 = CountIncreases(nums, 3);

            return new Result(part1, part2);
        }

        public static int CountIncreases(List<int> nums, int window)
        {
            var numIncreases = 0;
            for (int i = window; i < nums.Count; i++)
            {
                if (nums[i] > nums[i - window]) numIncreases += 1;
            }
            return numIncreases;
        }
    }
}
