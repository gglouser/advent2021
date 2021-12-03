using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent2021.Solutions.Day03
{
    public record Result(int Part1, int Part2) { }

    public static class Day03
    {
        public static Result Solve(IEnumerable<string> input)
        {
            var binary = input.ToList();

            var (gamma, epsilon) = FindGammaEpsilon(binary);
            var part1 = gamma * epsilon;

            var o2 = FindVoteWinner(binary, true);
            var co2 = FindVoteWinner(binary, false);
            var part2 = o2 * co2;

            return new Result(part1, part2);
        }

        public static (int, int) FindGammaEpsilon(List<string> binary)
        {
            var numBits = binary[0].Length;
            string gammaString = "";
            for (int position = 0; position < numBits; position++)
            {
                gammaString += MostCommonBit(binary, position);
            }

            var gamma = Convert.ToInt32(gammaString, 2);
            var epsilon = ~gamma & ((1 << numBits) - 1);
            return (gamma, epsilon);
        }

        public static int FindVoteWinner(List<string> binary, bool most)
        {
            var numBits = binary[0].Length;
            for (int position = 0; position < numBits; position++)
            {
                var x = MostCommonBit(binary, position);
                binary = binary.Where(b => (b[position] == x) == most).ToList();
                if (binary.Count == 1)
                {
                    return Convert.ToInt32(binary[0], 2);
                }
            }
            Console.WriteLine("not found");
            return 0;
        }

        public static char MostCommonBit(List<string> binary, int position)
        {
            int ones = binary.Where(b => b[position] == '1').Count();
            return ones * 2 >= binary.Count ? '1' : '0';
        }
    }
}
