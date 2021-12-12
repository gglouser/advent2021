using System.IO;
using Advent2021.Solutions.Day11;
using Xunit;

namespace Advent2021.Tests
{
    public class Day11Test
    {
        [Fact]
        public void TestExample()
        {
            const string example = @"
5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526
";
            var input = example.Trim().ReadLines();
            var result = Day11.Solve(input);
            Assert.Equal(1656, result.Part1);
            Assert.Equal(195, result.Part2);
        }

        [Fact]
        public void TestRealInput()
        {
            var input = File.ReadLines("inputs/day11.txt");
            var result = Day11.Solve(input);
            Assert.Equal(1562, result.Part1);
            Assert.Equal(268, result.Part2);
        }
    }
}
