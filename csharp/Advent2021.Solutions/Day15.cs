using System.Collections.Generic;
using System.Linq;

namespace Advent2021.Solutions.Day15
{
    public record Result(int Part1, int Part2) { }

    public static class Day15
    {
        public static Result Solve(IEnumerable<string> input)
        {
            Grid<int> cave = new Grid<int>(
                input.Select(line => line.Select(c => c - '0').ToArray()).ToArray());

            var part1 = LowestRisk(cave);

            var bigCave = GrowMap(cave);
            var part2 = LowestRisk(bigCave);

            return new Result(part1, part2);
        }

        public static int LowestRisk(Grid<int> cave)
        {
            var bestRisks = Grid<int>.FromGenerator(cave.Width(), cave.Height(), _ => int.MaxValue);
            var frontiers = new Dictionary<int, List<Pos>>();
            var frontierQueue = new PriorityQueue<List<Pos>>();
            var goalPos = new Pos(cave.Width() - 1, cave.Height() - 1);

            var initFrontier = new List<Pos>();
            initFrontier.Add(new Pos());
            frontierQueue.Enqueue(0, initFrontier);

            while (frontierQueue.Any())
            {
                var (frontierRisk, frontier) = frontierQueue.Dequeue();
                foreach (var pos in frontier)
                {
                    if (frontierRisk >= bestRisks[pos]) continue;

                    bestRisks[pos] = frontierRisk;

                    foreach (var adjacent in cave.Adjacent(pos))
                    {
                        int risk = frontierRisk + cave[adjacent];
                        if (!frontiers.ContainsKey(risk))
                        {
                            frontiers.Add(risk, new List<Pos>());
                            frontierQueue.Enqueue(risk, frontiers[risk]);
                        }
                        frontiers[risk].Add(adjacent);
                    }
                }

                if (bestRisks[goalPos] < int.MaxValue) break;
            }

            return bestRisks[goalPos];
        }

        public static Grid<int> GrowMap(Grid<int> cave)
        {
            return Grid<int>.FromGenerator(5 * cave.Width(), 5 * cave.Height(), pos =>
            {
                var blockX = pos.X / cave.Width();
                var blockY = pos.Y / cave.Height();
                var subX = pos.X % cave.Width();
                var subY = pos.Y % cave.Height();
                var risk = cave[new Pos(subX, subY)] + blockX + blockY;
                return (risk - 1) % 9 + 1;
            });
        }
    }
}
