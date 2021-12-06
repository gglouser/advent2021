using System.IO;
using Advent2021.Solutions.Day06;
using Xunit;

namespace Advent2021.Tests
{
    public class Day06Test
    {
        [Fact]
        public void TestExample()
        {
            const string example = @"
3,4,3,1,2
";
            var input = example.Trim().ReadLines();
            var result = Day06.Solve(input);
            Assert.Equal(5934, result.Part1);
            Assert.Equal(26984457539, result.Part2);
        }

        [Fact]
        public void TestRealInput()
        {
            var input = File.ReadLines("inputs/day06.txt");
            var result = Day06.Solve(input);
            Assert.Equal(345793, result.Part1);
            Assert.Equal(1572643095893, result.Part2);
        }
    }
}
