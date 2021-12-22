using System.IO;
using Advent2021.Solutions.Day21;
using Xunit;

namespace Advent2021.Tests
{
    public class Day21Test
    {
        [Fact]
        public void TestExample()
        {
            const string example = @"
Player 1 starting position: 4
Player 2 starting position: 8
";
            var input = example.Trim().ReadLines();
            var result = Day21.Solve(input);
            Assert.Equal(739785, result.Part1);
            Assert.Equal(444356092776315, result.Part2);
        }

        [Fact]
        public void TestRealInput()
        {
            var input = File.ReadLines("inputs/day21.txt");
            var result = Day21.Solve(input);
            Assert.Equal(855624, result.Part1);
            Assert.Equal(187451244607486, result.Part2);
        }
    }
}
