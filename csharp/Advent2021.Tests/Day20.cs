using System.IO;
using Advent2021.Solutions.Day20;
using Xunit;

namespace Advent2021.Tests
{
    public class Day20Test
    {
        [Fact]
        public void TestExample()
        {
            const string example = @"
..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..##
#..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###
.######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#.
.#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#.....
.#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#..
...####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.....
..##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#

#..#.
#....
##..#
..#..
..###
";
            var input = example.Trim().ReadLines();
            var result = Day20.Solve(input);
            Assert.Equal(35, result.Part1);
            Assert.Equal(3351, result.Part2);
        }

        [Fact]
        public void TestRealInput()
        {
            var input = File.ReadLines("inputs/day20.txt");
            var result = Day20.Solve(input);
            Assert.Equal(5786, result.Part1);
            Assert.Equal(16757, result.Part2);
        }
    }
}
