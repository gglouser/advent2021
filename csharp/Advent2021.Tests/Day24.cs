using System.IO;
using Advent2021.Solutions.Day24;
using Xunit;

namespace Advent2021.Tests
{
    public class Day24Test
    {
        [Fact]
        public void TestALU()
        {
            const string example = @"
inp w
add z w
mod z 2
div w 2
add y w
mod y 2
div w 2
add x w
mod x 2
div w 2
mod w 2
";
            var program = example.Trim().ReadLines();
            var result = Day24.RunProgram(program, new int[]{ 13 });
            Assert.Equal(1, result.W);
            Assert.Equal(1, result.X);
            Assert.Equal(0, result.Y);
            Assert.Equal(1, result.Z);
        }

        [Fact]
        public void TestRealInput()
        {
            var input = File.ReadLines("inputs/day24.txt");
            var result = Day24.Solve(input);
            Assert.Equal("95299897999897", result.Part1);
            Assert.Equal("31111121382151", result.Part2);
        }
    }
}
