using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent2021.Solutions.Day22
{
    public record Result(long Part1, long Part2) { }

    public static class Day22
    {
        public static Result Solve(IEnumerable<string> input)
        {
            var cuboids = input.Select(Cuboid.Parse).ToArray();

            var initRegion = new Cuboid
            {
                X = new Range { Min = -50, Max = 50 },
                Y = new Range { Min = -50, Max = 50 },
                Z = new Range { Min = -50, Max = 50 },
            };
            var nearCuboids = cuboids.Where(c => initRegion.Contains(c)).ToArray();

            var part1 = BootReactor(nearCuboids);
            var part2 = BootReactor(cuboids);

            return new Result(part1, part2);
        }

        public static long BootReactor(Cuboid[] cuboids)
        {
            var slices1 = GetSliceRanges(cuboids.Select(c => c.X))
                .Select(range => Slice(cuboids, range, c => c.X, Cuboid.UpdateX))
                .Where(slice => slice.Any())
                .ToArray();

            var slices2 = slices1.SelectMany(slice =>
                GetSliceRanges(slice.Select(c => c.Y))
                    .Select(range => Slice(slice, range, c => c.Y, Cuboid.UpdateY))
                    .Where(slice => slice.Any())
                    .ToArray()).ToArray();

            var slices3 = slices2.SelectMany(slice =>
                GetSliceRanges(slice.Select(c => c.Z))
                    .Select(range => Slice(slice, range, c => c.Z, Cuboid.UpdateZ))
                    .Where(slice => slice.Any())
                    .ToArray()).ToArray();

            var cubes = slices3
                .Select(slice => slice.Last())
                .Where(cuboid => cuboid.On)
                .Select(cuboid => cuboid.Volume)
                .Sum();
            return cubes;
        }

        public static Range[] GetSliceRanges(IEnumerable<Range> ranges)
        {
            var points = ranges
                .Select(r => r.Min)
                .Concat(ranges.Select(r => r.Max + 1))
                .OrderBy(x => x)
                .Distinct()
                .ToArray();
            var slices = new Range[points.Length - 1];
            for (int i = 0; i < points.Length - 1; i++)
            {
                slices[i] = new Range { Min = points[i], Max = points[i + 1] - 1 };
            }
            return slices;
        }

        public static Cuboid[] Slice(
            IEnumerable<Cuboid> cuboids,
            Range range,
            Func<Cuboid, Range> select,
            Cuboid.Updater update)
        {
            return cuboids
                .Where(c => range.Intersects(select(c)))
                .Select(c =>
                {
                    update(ref c, range);
                    return c;
                })
                .ToArray();
        }
    }

    public struct Range
    {
        public int Min;
        public int Max;

        public bool Intersects(Range other) => Min <= other.Max && other.Min <= Max;
        public bool Contains(Range other) => Min <= other.Min && Max >= other.Max;
    }

    public struct Cuboid
    {
        public Range X;
        public Range Y;
        public Range Z;
        public bool On;

        public static Regex RE_CUBOID = new Regex(@"(on|off) x=(-?\d+)..(-?\d+),y=(-?\d+)..(-?\d+),z=(-?\d+)..(-?\d+)");

        public static Cuboid Parse(string line)
        {
            var match = RE_CUBOID.Match(line);
            return new Cuboid
            {
                X = new Range
                {
                    Min = int.Parse(match.Groups[2].Value),
                    Max = int.Parse(match.Groups[3].Value),
                },
                Y = new Range
                {
                    Min = int.Parse(match.Groups[4].Value),
                    Max = int.Parse(match.Groups[5].Value),
                },
                Z = new Range
                {
                    Min = int.Parse(match.Groups[6].Value),
                    Max = int.Parse(match.Groups[7].Value),
                },
                On = match.Groups[1].Value == "on",
            };
        }

        public long Volume
        {
            get => (long)(X.Max - X.Min + 1) * (long)(Y.Max - Y.Min + 1) * (long)(Z.Max - Z.Min + 1);
        }

        public bool Contains(Cuboid other) =>
            X.Contains(other.X) && Y.Contains(other.Y) && Z.Contains(other.Z);

        public delegate void Updater(ref Cuboid c, Range r);

        public static void UpdateX(ref Cuboid c, Range r) => c.X = r;
        public static void UpdateY(ref Cuboid c, Range r) => c.Y = r;
        public static void UpdateZ(ref Cuboid c, Range r) => c.Z = r;
    }
}
