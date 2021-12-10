using System.IO;
using Advent2021.Solutions.Day09;
using Xunit;

namespace Advent2021.Tests
{
    public class Day09Test
    {
        [Fact]
        public void TestExample()
        {
            const string example = @"
2199943210
3987894921
9856789892
8767896789
9899965678
";
            var input = example.Trim().ReadLines();
            var result = Day09.Solve(input);
            Assert.Equal(15, result.Part1);
            Assert.Equal(1134, result.Part2);
        }

        [Fact]
        public void TestRealInput()
        {
            var input = File.ReadLines("inputs/day09.txt");
            var result = Day09.Solve(input);
            Assert.Equal(570, result.Part1);
            Assert.Equal(899392, result.Part2);
        }
    }
}
