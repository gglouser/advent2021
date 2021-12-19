using System.IO;
using System.Linq;
using Advent2021.Solutions.Day18;
using Xunit;

namespace Advent2021.Tests
{
    public class Day18Test
    {
        [Fact]
        public void TestExplode()
        {
            var n = SFNum.Parse("[[[[[9,8],1],2],3],4]");
            Assert.Equal("[[[[[9,8],1],2],3],4]", n.ToString());
            Assert.True(n.TryExplode(0));
            Assert.Equal("[[[[0,9],2],3],4]", n.ToString());
        }

        [Fact]
        public void TestSplit()
        {
            var n = new SFNum(11);
            Assert.True(n.TrySplit());
            Assert.Equal("[5,6]", n.ToString());
        }

        [Fact]
        public void TestReduce()
        {
            var p = SFNum.Parse("[[[[4,3],4],4],[7,[[8,4],9]]]");
            var q = SFNum.Parse("[1,1]");
            var r = p + q;
            Assert.Equal("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]", r.ToString());
        }

        [Fact]
        public void TestSum()
        {
            const string example = @"
[1,1]
[2,2]
[3,3]
[4,4]
[5,5]
";
            var input = example.Trim().ReadLines();
            var sum = input.Select(SFNum.Parse).Aggregate((a, b) => a + b);
            Assert.Equal("[[[[3,0],[5,3]],[4,4]],[5,5]]", sum.ToString());
        }

        [Fact]
        public void TestExample()
        {
            const string example = @"
[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]
[[[5,[2,8]],4],[5,[[9,9],0]]]
[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]
[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]
[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]
[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]
[[[[5,4],[7,7]],8],[[8,3],8]]
[[9,3],[[9,9],[6,[4,9]]]]
[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]
[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]
";
            var input = example.Trim().ReadLines();
            var result = Day18.Solve(input);
            Assert.Equal(4140, result.Part1);
            Assert.Equal(3993, result.Part2);
        }

        [Fact]
        public void TestRealInput()
        {
            var input = File.ReadLines("inputs/day18.txt");
            var result = Day18.Solve(input);
            Assert.Equal(4202, result.Part1);
            Assert.Equal(4779, result.Part2);
        }
    }
}
