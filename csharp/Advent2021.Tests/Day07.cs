using System.IO;
using Advent2021.Solutions.Day07;
using Xunit;

namespace Advent2021.Tests
{
    public class Day07Test
    {
        [Fact]
        public void TestExample()
        {
            const string example = @"
16,1,2,0,4,2,7,1,2,14
";
            var input = example.Trim().ReadLines();
            var result = Day07.Solve(input);
            Assert.Equal(37, result.Part1);
            Assert.Equal(168, result.Part2);
        }

        [Fact]
        public void TestRealInput()
        {
            var input = File.ReadLines("inputs/day07.txt");
            var result = Day07.Solve(input);
            Assert.Equal(344297, result.Part1);
            Assert.Equal(97164301, result.Part2);
        }
    }
}
