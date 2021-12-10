using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent2021.Solutions.Day10
{
    public record Result(long Part1, long Part2) { }

    public static class Day10
    {
        public static Result Solve(IEnumerable<string> input)
        {
            var chunks = input.ToArray();

            var part1 = chunks.Select(c => SyntaxCheck(c).Item1).Sum();

            long[] scores = chunks
                .Select(SyntaxCheck)
                .Where(r => r.Item1 == 0)
                .Select(r => r.Item2.Aggregate(0L, (acc, c) => 5*acc + c))
                .OrderBy(s => s)
                .ToArray();

            var part2 = scores[scores.Length / 2];

            return new Result(part1, part2);
        }

        public static (long, Stack<long>) SyntaxCheck(string chunk)
        {
            var stack = new Stack<long>();
            foreach (var c in chunk)
            {
                switch (c)
                {
                    case '(': stack.Push(1); break;
                    case '[': stack.Push(2); break;
                    case '{': stack.Push(3); break;
                    case '<': stack.Push(4); break;
                    case ')':
                        if (stack.Pop() != 1) return (3, stack);
                        break;
                    case ']':
                        if (stack.Pop() != 2) return (57, stack);
                        break;
                    case '}':
                        if (stack.Pop() != 3) return (1197, stack);
                        break;
                    case '>':
                        if (stack.Pop() != 4) return (25137, stack);
                        break;
                    default: break;
                }
            }
            return (0, stack);
        }
    }
}
