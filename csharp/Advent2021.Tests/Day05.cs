using System.IO;
using Advent2021.Solutions.Day05;
using Xunit;

namespace Advent2021.Tests
{
    public class Day05Test
    {
        [Fact]
        public void TestExample()
        {
            const string example = @"
0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2
";
            var input = example.Trim().ReadLines();
            var result = Day05.Solve(input);
            Assert.Equal(5, result.Part1);
            Assert.Equal(12, result.Part2);
        }

        [Fact]
        public void TestRealInput()
        {
            var input = File.ReadLines("inputs/day05.txt");
            var result = Day05.Solve(input);
            Assert.Equal(5147, result.Part1);
            Assert.Equal(16925, result.Part2);
        }
    }
}
