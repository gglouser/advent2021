using System.Collections.Generic;
using System.Linq;

namespace Advent2021.Solutions.Day06
{
    public record Result(long Part1, long Part2) { }

    public static class Day06
    {
        public static Result Solve(IEnumerable<string> input)
        {
            var fishCount = CountFish(input.First());

            var part1 = RunGenerations(fishCount, 80);
            var part2 = RunGenerations(fishCount, 256);

            return new Result(part1, part2);
        }

        public static long[] CountFish(string input)
        {
            long[] fishCount = new long[9];
            foreach (var fish in input.Split(',').Select(int.Parse))
            {
                fishCount[fish]++;
            }
            return fishCount;
        }

        public static long[] StepFishPopulation(long[] fishCount)
        {
            long[] newPopulation = new long[9];
            for (int i = 1; i < fishCount.Length; i++)
            {
                newPopulation[i - 1] = fishCount[i];
            }
            newPopulation[6] += fishCount[0];
            newPopulation[8] += fishCount[0];
            return newPopulation;
        }

        public static long RunGenerations(long[] fishCount, int generations)
        {
            for (long i = 0; i < generations; i++)
            {
                fishCount = StepFishPopulation(fishCount);
            }
            return fishCount.Sum();
        }
    }
}
