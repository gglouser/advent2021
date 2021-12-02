using System.IO;
using Advent2021.Solutions.Day01;
using Xunit;

namespace Advent2021.Tests
{
    public class Day01Test
    {
        [Fact]
        public void TestExample()
        {
            const string example = @"
199
200
208
210
200
207
240
269
260
263
";
            var input = example.Trim().ReadLines();
            var result = Day01.Solve(input);
            Assert.Equal(7, result.Part1);
            Assert.Equal(5, result.Part2);
        }

        [Fact]
        public void TestRealInput()
        {
            var input = File.ReadLines("inputs/day01.txt");
            var result = Day01.Solve(input);
            Assert.Equal(1288, result.Part1);
            Assert.Equal(1311, result.Part2);
        }
    }
}
