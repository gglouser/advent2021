using System.Collections.Generic;
using System.Linq;

namespace Advent2021.Solutions.Day14
{
    public record Result(long Part1, long Part2) { }

    public static class Day14
    {
        public static Result Solve(IEnumerable<string> input)
        {
            var template = input.First();
            var rules = input.Skip(2).Select(ParseRule).ToDictionary();
            var state = new State(template, rules);

            var part1 = state.RunIterations(10);
            var part2 = state.RunIterations(30);

            return new Result(part1, part2);
        }

        public static ((char, char), char) ParseRule(string line)
        {
            var split = line.Split(" -> ");
            return ((split[0][0], split[0][1]), split[1][0]);
        }

        public class State
        {
            private char FirstElem;
            private char LastElem;
            private Dictionary<(char, char), char> Rules;
            private Counter<(char, char)> Counts;

            public State(string template, Dictionary<(char, char), char> rules)
            {
                FirstElem = template.First();
                LastElem = template.Last();
                Rules = rules;

                Counts = new Counter<(char, char)>();
                for (int i = 0; i < template.Length - 1; i++)
                {
                    var key = (template[i], template[i + 1]);
                    Counts[key]++;
                }
            }

            public long RunIterations(int iterations)
            {
                for (int i = 0; i < iterations; i++)
                {
                    InsertionStep();
                }
                var totals = GetTotals();
                var t = totals.Select(elem => elem.Item2).OrderBy(n => n);
                return t.Last() / 2 - t.First() / 2;
            }

            public void InsertionStep()
            {
                var next = new Counter<(char, char)>();
                foreach (var ((a, c), n) in Counts)
                {
                    var b = Rules[(a, c)];
                    next[(a, b)] += n;
                    next[(b, c)] += n;
                }
                Counts = next;
            }

            public Counter<char> GetTotals()
            {
                var totals = new Counter<char>();
                totals[FirstElem]++;
                totals[LastElem]++;
                foreach (var ((a, b), n) in Counts)
                {
                    totals[a] += n;
                    totals[b] += n;
                }
                return totals;
            }
        }
    }
}
