using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent2021.Solutions.Day17
{
    public record Result(int Part1, int Part2) { }

    public static class Day17
    {
        public static Regex RE_TARGET = new Regex(@"target area: x=(\d+)..(\d+), y=(-?\d+)..(-?\d+)");

        public static Result Solve(IEnumerable<string> input)
        {
            var match = RE_TARGET.Match(input.First());
            var targetXMin = int.Parse(match.Groups[1].Value);
            var targetXMax = int.Parse(match.Groups[2].Value);
            var targetYMin = int.Parse(match.Groups[3].Value);
            var targetYMax = int.Parse(match.Groups[4].Value);

            var (bestY, count) = Simulate(targetXMin, targetXMax, targetYMin, targetYMax);

            var part1 = bestY;
            var part2 = count;

            return new Result(part1, part2);
        }

        public static (int, int) Simulate(int xmin, int xmax, int ymin, int ymax)
        {
            var bestY = 0;
            var count = 0;
            for (int initVx = 1; initVx <= xmax; initVx++)
            {
                for (int initVy = ymin; initVy <= -ymin; initVy++)
                {
                    var x = 0;
                    var y = 0;
                    var vx = initVx;
                    var vy = initVy;
                    var maxy = 0;
                    for (var time = 0; x <= xmax && y >= ymin && time < 1000; time++)
                    {
                        x += vx;
                        y += vy;
                        if (vx > 0) vx--;
                        vy--;
                        if (y > maxy) maxy = y;
                        if (x >= xmin && x <= xmax && y >= ymin && y <= ymax)
                        {
                            if (maxy > bestY) bestY = maxy;
                            count++;
                            break;
                        }
                    }
                }
            }
            return (bestY, count);
        }
    }
}
