using System.IO;
using Advent2021.Solutions.Day17;
using Xunit;

namespace Advent2021.Tests
{
    public class Day17Test
    {
        [Fact]
        public void TestExample()
        {
            const string example = "target area: x=20..30, y=-10..-5";
            var input = example.Trim().ReadLines();
            var result = Day17.Solve(input);
            Assert.Equal(45, result.Part1);
            Assert.Equal(112, result.Part2);
        }

        [Fact]
        public void TestRealInput()
        {
            var input = File.ReadLines("inputs/day17.txt");
            var result = Day17.Solve(input);
            Assert.Equal(5778, result.Part1);
            Assert.Equal(2576, result.Part2);
        }
    }
}
