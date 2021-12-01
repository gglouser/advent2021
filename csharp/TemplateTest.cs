using System.IO;
using Advent2021.Solutions.DayXX;
using Xunit;

namespace Advent2021.Tests
{
    public class DayXXTest
    {
        [Fact]
        public void TestExample()
        {
            const string example = @"
1721
";
            var input = example.Trim().ReadLines();
            var result = DayXX.Solve(input);
            Assert.Equal(-1, result.Part1);
            Assert.Equal(-1, result.Part2);
        }

        [Fact]
        public void TestRealInput()
        {
            var input = File.ReadLines("inputs/dayXX.txt");
            var result = DayXX.Solve(input);
            Assert.Equal(-1, result.Part1);
            Assert.Equal(-1, result.Part2);
        }
    }
}
