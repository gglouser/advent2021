using System.IO;
using Advent2021.Solutions.Day03;
using Xunit;

namespace Advent2021.Tests
{
    public class Day03Test
    {
        [Fact]
        public void TestExample()
        {
            const string example = @"
00100
11110
10110
10111
10101
01111
00111
11100
10000
11001
00010
01010
";
            var input = example.Trim().ReadLines();
            var result = Day03.Solve(input);
            Assert.Equal(198, result.Part1);
            Assert.Equal(230, result.Part2);
        }

        [Fact]
        public void TestRealInput()
        {
            var input = File.ReadLines("inputs/day03.txt");
            var result = Day03.Solve(input);
            Assert.Equal(3242606, result.Part1);
            Assert.Equal(4856080, result.Part2);
        }
    }
}
