using System.IO;
using Advent2021.Solutions.Day12;
using Xunit;

namespace Advent2021.Tests
{
    public class Day12Test
    {
        [Fact]
        public void TestExample1()
        {
            const string example = @"
start-A
start-b
A-c
A-b
b-d
A-end
b-end
";
            var input = example.Trim().ReadLines();
            var result = Day12.Solve(input);
            Assert.Equal(10, result.Part1);
            Assert.Equal(36, result.Part2);
        }

        [Fact]
        public void TestExample2()
        {
            const string example = @"
dc-end
HN-start
start-kj
dc-start
dc-HN
LN-dc
HN-end
kj-sa
kj-HN
kj-dc
";
            var input = example.Trim().ReadLines();
            var result = Day12.Solve(input);
            Assert.Equal(19, result.Part1);
            Assert.Equal(103, result.Part2);
        }

        [Fact]
        public void TestExample3()
        {
            const string example = @"
fs-end
he-DX
fs-he
start-DX
pj-DX
end-zg
zg-sl
zg-pj
pj-he
RW-he
fs-DX
pj-RW
zg-RW
start-pj
he-WI
zg-he
pj-fs
start-RW
";
            var input = example.Trim().ReadLines();
            var result = Day12.Solve(input);
            Assert.Equal(226, result.Part1);
            Assert.Equal(3509, result.Part2);
        }

        [Fact]
        public void TestRealInput()
        {
            var input = File.ReadLines("inputs/day12.txt");
            var result = Day12.Solve(input);
            Assert.Equal(4792, result.Part1);
            Assert.Equal(133360, result.Part2);
        }
    }
}
