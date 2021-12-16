using System.IO;
using Advent2021.Solutions.Day15;
using Xunit;

namespace Advent2021.Tests
{
    public class Day15Test
    {
        [Fact]
        public void TestExample()
        {
            const string example = @"
1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581
";
            var input = example.Trim().ReadLines();
            var result = Day15.Solve(input);
            Assert.Equal(40, result.Part1);
            Assert.Equal(315, result.Part2);
        }

        [Fact]
        public void TestRealInput()
        {
            var input = File.ReadLines("inputs/day15.txt");
            // var stopwatch = new System.Diagnostics.Stopwatch();
            // stopwatch.Start();
            var result = Day15.Solve(input);
            // stopwatch.Stop();
            Assert.Equal(619, result.Part1);
            Assert.Equal(2922, result.Part2);
            // Assert.Equal(0, stopwatch.ElapsedMilliseconds);
        }
    }
}
