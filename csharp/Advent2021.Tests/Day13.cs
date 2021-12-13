using System;
using System.IO;
using Advent2021.Solutions.Day13;
using Xunit;

namespace Advent2021.Tests
{
    public class Day13Test
    {
        [Fact]
        public void TestExample()
        {
            const string example = @"
6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0

fold along y=7
fold along x=5
";
            var input = example.Trim().ReadLines();
            var result = Day13.Solve(input);
            Assert.Equal(17, result.Part1);
            var expectedCode = string.Join('\n', new string[] {
                "#####",
                "#...#",
                "#...#",
                "#...#",
                "#####"
            });
            Assert.Equal(expectedCode, result.Part2);
        }

        [Fact]
        public void TestRealInput()
        {
            var input = File.ReadLines("inputs/day13.txt");
            var result = Day13.Solve(input);
            Assert.Equal(737, result.Part1);
            Console.WriteLine(result.Part2);
            var expectedCode = string.Join('\n', new string[] {
                "####.#..#...##.#..#..##..####.#..#.###.",
                "...#.#..#....#.#..#.#..#.#....#..#.#..#",
                "..#..#..#....#.#..#.#..#.###..####.#..#",
                ".#...#..#....#.#..#.####.#....#..#.###.",
                "#....#..#.#..#.#..#.#..#.#....#..#.#...",
                "####..##...##...##..#..#.#....#..#.#..."
            });
            Assert.Equal(expectedCode, result.Part2);
        }
    }
}
