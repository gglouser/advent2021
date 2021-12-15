using System.IO;
using Advent2021.Solutions.Day14;
using Xunit;

namespace Advent2021.Tests
{
    public class Day14Test
    {
        [Fact]
        public void TestExample()
        {
            const string example = @"
NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C
";
            var input = example.Trim().ReadLines();
            var result = Day14.Solve(input);
            Assert.Equal(1588, result.Part1);
            Assert.Equal(2188189693529, result.Part2);
        }

        [Fact]
        public void TestRealInput()
        {
            var input = File.ReadLines("inputs/day14.txt");
            var result = Day14.Solve(input);
            Assert.Equal(3406, result.Part1);
            Assert.Equal(3941782230241, result.Part2);
        }
    }
}
