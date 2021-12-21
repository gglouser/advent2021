using System.Collections.Generic;
using System.Linq;

namespace Advent2021.Solutions.Day20
{
    public record Result(int Part1, int Part2) { }

    public static class Day20
    {
        public static Result Solve(IEnumerable<string> input)
        {
            string imageEnhancer = string.Concat(input.TakeWhile(line => line != ""));
            var initState = new Grid<bool>(input.SkipWhile(line => line != "").Skip(1)
                .Select(line => line.Select(c => c == '#').ToArray())
                .ToArray());

            var part1 = RunSimulation(initState, imageEnhancer, 2);
            var part2 = RunSimulation(initState, imageEnhancer, 50);

            return new Result(part1, part2);
        }

        public static int RunSimulation(Grid<bool> initState, string rule, int steps)
        {
            var state = initState;
            var voidState = false;
            for (int i = 0; i < steps; i++)
            {
                var (nextState, nextVoid) = Step(state, rule, voidState);
                state = nextState;
                voidState = nextVoid;
            }
            return state.Values().Count(x => x);
        }

        public static (Grid<bool>, bool) Step(Grid<bool> state, string rule, bool voidState)
        {
            var next = Grid<bool>.FromGenerator(state.Width() + 2, state.Height() + 2,
                pos =>
                {
                    var key = 0;
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            key = 2 * key;
                            var p = pos + new Pos(j - 1, i - 1);
                            if (state.InBounds(p))
                            {
                                if (state[p]) key++;
                            }
                            else
                            {
                                if (voidState) key++;
                            }
                        }
                    }
                    return rule[key] == '#';
                });

            var nextVoid = (voidState ? rule[511] : rule[0]) == '#';
            return (next, nextVoid);
        }
    }
}
