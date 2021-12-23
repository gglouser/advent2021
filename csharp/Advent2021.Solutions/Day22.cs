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
            var nearCuboids = cuboids.Where(c => c.IsWithin(initRegion)).ToArray();

            var part1 = BootReactor(nearCuboids);
            var part2 = BootReactor(cuboids);

            return new Result(part1, part2);
        }

        public static long BootReactor(Cuboid[] cuboids)
        {
            var slices1 = GetSliceRanges(cuboids.Select(c => c.X))
                .Select(range => SliceX(cuboids, range))
                .Where(slice => slice.Any())
                .ToArray();

            var slices2 = slices1.Select(xslice =>
                GetSliceRanges(xslice.Select(c => c.Y))
                    .Select(range => SliceY(xslice, range))
                    .Where(slice => slice.Any())
                    .ToArray()).ToArray();

            var slices3 = slices2.Select(xslice => xslice.Select(yslice =>
                GetSliceRanges(yslice.Select(c => c.Z))
                    .Select(range => SliceZ(yslice, range))
                    .Where(slice => slice.Any())
                    .ToArray()).ToArray()).ToArray();

            var cubes = slices3.Select(zslice =>
                zslice.Select(yslice =>
                    yslice.Select(xslice =>
                        xslice.Last().On ? xslice[0].Volume : 0
                    ).Sum()
                ).Sum()
            ).Sum();
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

        public static Cuboid[] SliceX(IEnumerable<Cuboid> cuboids, Range slice)
        {
            return cuboids
                .Where(c => slice.Min <= c.X.Max && c.X.Min <= slice.Max)
                .Select(c => new Cuboid
                {
                    X = new Range { Min = slice.Min, Max = slice.Max },
                    Y = c.Y,
                    Z = c.Z,
                    On = c.On,
                })
                .ToArray();
        }

        public static Cuboid[] SliceY(IEnumerable<Cuboid> cuboids, Range slice)
        {
            return cuboids
                .Where(c => slice.Min <= c.Y.Max && c.Y.Min <= slice.Max)
                .Select(c => new Cuboid
                {
                    X = c.X,
                    Y = new Range { Min = slice.Min, Max = slice.Max },
                    Z = c.Z,
                    On = c.On,
                })
                .ToArray();
        }

        public static Cuboid[] SliceZ(IEnumerable<Cuboid> cuboids, Range slice)
        {
            return cuboids
                .Where(c => slice.Min <= c.Z.Max && c.Z.Min <= slice.Max)
                .Select(c => new Cuboid
                {
                    X = c.X,
                    Y = c.Y,
                    Z = new Range { Min = slice.Min, Max = slice.Max },
                    On = c.On,
                })
                .ToArray();
        }
    }

    public struct Range
    {
        public int Min;
        public int Max;
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

        public bool IsWithin(Cuboid region) =>
            X.Min >= region.X.Min && X.Max <= region.X.Max
            && Y.Min >= region.Y.Min && Y.Max <= region.Y.Max
            && Z.Min >= region.Z.Min && Z.Max <= region.Z.Max;
    }
}
