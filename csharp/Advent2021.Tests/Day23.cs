using System.IO;
using Advent2021.Solutions.Day23;
using Xunit;

namespace Advent2021.Tests
{
    public class Day23Test
    {
        [Fact]
        public void TestExample()
        {
            const string example = @"
#############
#...........#
###B#C#B#D###
  #A#D#C#A#
  #########
";
            var input = example.Trim().ReadLines();
            var result = Day23.Solve(input);
            Assert.Equal(12521, result.Part1);
            Assert.Equal(44169, result.Part2);
        }

        [Fact]
        public void TestRealInput()
        {
            var input = File.ReadLines("inputs/day23.txt");
            var result = Day23.Solve(input);
            Assert.Equal(15237, result.Part1);
            Assert.Equal(47509, result.Part2);
        }
    }
}
