using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Advent2021.Solutions.Day25
{
    public record Result(int Part1, int Part2) { }

    public static class Day25
    {
        public static Result Solve(IEnumerable<string> input)
        {
            var map = input.ToArray();
            var initState = Grid<char>.FromGenerator(map[0].Length, map.Length,
                pos => map[pos.Y][pos.X]);

            var part1 = RunToStop(initState);
            var part2 = 0;

            return new Result(part1, part2);
        }

        public static int RunToStop(Grid<char> initState)
        {
            var n = 0;
            var state = initState;
            while (true)
            {
                var changedE = false;
                (state, changedE) = StepE(state);
                var changedS = false;
                (state, changedS) = StepS(state);
                n++;
                if (!(changedE || changedS)) break;
            }
            return n;
        }

        public static (Grid<char>, bool) StepE(Grid<char> grid)
        {
            var changed = false;
            var newGrid = Grid<char>.FromGenerator(grid.Width(), grid.Height(), pos =>
            {
                var east = pos + new Pos(1, 0);
                east.X %= grid.Width();

                var west = pos + new Pos(-1, 0);
                if (west.X < 0) west.X += grid.Width();

                if (grid[pos] == '>' && grid[east] == '.')
                {
                    changed = true;
                    return '.';
                }
                if (grid[pos] == '.' && grid[west] == '>')
                {
                    changed = true;
                    return '>';
                }
                return grid[pos];
            });
            return (newGrid, changed);
        }

        public static (Grid<char>, bool) StepS(Grid<char> grid)
        {
            var changed = false;
            var newGrid = Grid<char>.FromGenerator(grid.Width(), grid.Height(), pos =>
            {
                var south = pos + new Pos(0, 1);
                south.Y %= grid.Height();

                var north = pos + new Pos(0, -1);
                if (north.Y < 0) north.Y += grid.Height();

                if (grid[pos] == 'v' && grid[south] == '.')
                {
                    changed = true;
                    return '.';
                }
                if (grid[pos] == '.' && grid[north] == 'v')
                {
                    changed = true;
                    return 'v';
                }
                return grid[pos];
            });
            return (newGrid, changed);
        }
    }
}
