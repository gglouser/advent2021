using System.Collections.Generic;
using System.Linq;

namespace Advent2021.Solutions.Day11
{
    public record Result(int Part1, int Part2) { }

    public static class Day11
    {
        public static Result Solve(IEnumerable<string> input)
        {
            var octopuses = new Grid<int>(
                input.Select(line => line.Select(c => c - '0').ToArray()).ToArray()
            );

            var part1 = 0;
            for (var i = 0; i < 100; i++)
            {
                var (octo2, flashes) = Step(octopuses);
                octopuses = octo2;
                part1 += flashes;
            }

            var part2 = 0;
            for (var i = 101; ; i++)
            {
                var (octo2, flashes) = Step(octopuses);
                if (flashes == 100)
                {
                    part2 = i;
                    break;
                }
                octopuses = octo2;
            }

            return new Result(part1, part2);
        }

        public static (Grid<int>, int) Step(Grid<int> octos)
        {
            var next = octos.Map(o => o + 1);
            var flashed = octos.Map(_ => false);

            int flashes = 0;
            var toFlash = new Queue<Pos>(next.Positions().Where(pos => next[pos] > 9));
            Pos flashPos;
            while (toFlash.TryDequeue(out flashPos))
            {
                if (flashed[flashPos]) continue;
                flashed[flashPos] = true;
                flashes++;
                foreach (var neighbor in next.Neighbors(flashPos))
                {
                    next[neighbor]++;
                    if (next[neighbor] > 9) toFlash.Enqueue(neighbor);
                }
            }

            foreach (var pos in octos.Positions().Where(p => flashed[p]))
            {
                next[pos] = 0;
            }

            return (next, flashes);
        }
    }
}
