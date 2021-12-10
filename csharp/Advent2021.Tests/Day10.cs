using System.IO;
using Advent2021.Solutions.Day10;
using Xunit;

namespace Advent2021.Tests
{
    public class Day10Test
    {
        [Fact]
        public void TestExample()
        {
            const string example = @"
[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]
";
            var input = example.Trim().ReadLines();
            var result = Day10.Solve(input);
            Assert.Equal(26397, result.Part1);
            Assert.Equal(288957, result.Part2);
        }

        [Fact]
        public void TestRealInput()
        {
            var input = File.ReadLines("inputs/day10.txt");
            var result = Day10.Solve(input);
            Assert.Equal(278475, result.Part1);
            Assert.Equal(3015539998, result.Part2);
        }
    }
}

// NOT 22418549
