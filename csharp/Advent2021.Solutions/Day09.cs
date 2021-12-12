using System.Collections.Generic;
using System.Linq;

namespace Advent2021.Solutions.Day09
{
    public record Result(int Part1, int Part2) { }

    public static class Day09
    {
        public static Result Solve(IEnumerable<string> input)
        {
            int[][] heights = input
              .Select(line => line.Select(c => c - '0').ToArray())
              .ToArray();
            var heightMap = new Grid<int>(heights);

            var lowPoints = FindLowPoints(heightMap);
            var part1 = lowPoints.Select(p => heightMap[p]).Sum() + lowPoints.Count;

            var part2 = lowPoints
                .Select(p => FloodFill(heightMap, p))
                .OrderBy(basinSize => basinSize)
                .TakeLast(3)
                .Aggregate(1, (a, b) => a * b);

            return new Result(part1, part2);
        }

        public static List<Pos> FindLowPoints(Grid<int> heightMap)
        {
            return heightMap.Positions().Where(pos =>
            {
                var h = heightMap[pos];
                return heightMap.Adjacent(pos).All(a => h < heightMap[a]);
            })
            .ToList();
        }

        public static int FloodFill(Grid<int> heightMap, Pos startPos)
        {
            var basin = new List<Pos>();
            var seen = new HashSet<Pos>();
            var queue = new Queue<Pos>();

            queue.Enqueue(startPos);
            while (queue.Any())
            {
                var pos = queue.Dequeue();
                if (seen.Contains(pos)) continue;
                seen.Add(pos);
                if (heightMap[pos] < 9)
                {
                    basin.Add(pos);
                    foreach (var adj in heightMap.Adjacent(pos))
                    {
                        queue.Enqueue(adj);
                    }
                }
            }

            return basin.Count;
        }
    }
}
