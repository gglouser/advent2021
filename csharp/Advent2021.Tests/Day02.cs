using System.IO;
using Advent2021.Solutions.Day02;
using Xunit;

namespace Advent2021.Tests
{
    public class Day02Test
    {
        [Fact]
        public void TestExample()
        {
            const string example = @"
forward 5
down 5
forward 8
up 3
down 8
forward 2
";
            var input = example.Trim().ReadLines();
            var result = Day02.Solve(input);
            Assert.Equal(150, result.Part1);
            Assert.Equal(900, result.Part2);
        }

        [Fact]
        public void TestRealInput()
        {
            var input = File.ReadLines("inputs/day02.txt");
            var result = Day02.Solve(input);
            Assert.Equal(1938402, result.Part1);
            Assert.Equal(1947878632, result.Part2);
        }
    }
}
