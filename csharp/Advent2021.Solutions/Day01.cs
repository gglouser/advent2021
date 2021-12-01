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

            var part1 = CountIncrements(nums, 1);
            var part2 = CountIncrements(nums, 3);

            return new Result(part1, part2);
        }

        public static int CountIncrements(List<int> nums, int step)
        {
            var numIncrements = 0;
            for (int i = step; i < nums.Count; i++)
            {
                if (nums[i] > nums[i - step]) numIncrements += 1;
            }
            return numIncrements;
        }
    }
}
