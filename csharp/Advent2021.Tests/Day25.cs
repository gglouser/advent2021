using System.IO;
using Advent2021.Solutions.Day25;
using Xunit;

namespace Advent2021.Tests
{
    public class Day25Test
    {
        [Fact]
        public void TestExample()
        {
            const string example = @"
v...>>.vv>
.vv>>.vv..
>>.>v>...v
>>v>>.>.v.
v>v.vv.v..
>.>>..v...
.vv..>.>v.
v.v..>>v.v
....v..v.>
";
            var input = example.Trim().ReadLines();
            var result = Day25.Solve(input);
            Assert.Equal(58, result.Part1);
            // Assert.Equal(-1, result.Part2);
        }

        [Fact]
        public void TestRealInput()
        {
            var input = File.ReadLines("inputs/day25.txt");
            var result = Day25.Solve(input);
            Assert.Equal(471, result.Part1);
            // Assert.Equal(-1, result.Part2);
        }
    }
}
