using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent2021.Solutions.Day19
{
    public record Result(int Part1, int Part2) { }

    public static class Day19
    {
        public static Result Solve(IEnumerable<string> input)
        {
            var scanners = Parse(input);

            var corrected = new bool[scanners.Count];
            corrected[0] = true;

            Queue<int> toCheck = new Queue<int>();
            toCheck.Enqueue(0);

            List<Pos3> scannerPos = new List<Pos3>();
            scannerPos.Add(new Pos3());

            int key;
            while (toCheck.TryDequeue(out key))
            {
                for (int i = 1; i < scanners.Count; i++)
                {
                    if (corrected[i]) continue;
                    Console.WriteLine($"matching scanners {key} and {i}");
                    var (beacons, delta) = MatchBeacons(scanners[key], scanners[i]);
                    if (beacons != null)
                    {
                        scanners[i] = beacons;
                        corrected[i] = true;
                        toCheck.Enqueue(i);
                        scannerPos.Add(delta);
                    }
                }
            }

            var part1 = scanners.SelectMany(b => b).Distinct().Count();

            int maxDist = 0;
            for (int i = 0; i < scannerPos.Count; i++)
            {
                for (int j = i + 1; j < scannerPos.Count; j++)
                {
                    var dist = (scannerPos.ElementAt(i) - scannerPos.ElementAt(j)).Abs();
                    if (dist > maxDist) maxDist = dist;
                }
            }
            var part2 = maxDist;

            return new Result(part1, part2);
        }

        public static List<List<Pos3>> Parse(IEnumerable<string> input)
        {
            List<List<Pos3>> scanners = new List<List<Pos3>>();
            List<Pos3> beacons = new List<Pos3>();
            var lines = input.GetEnumerator();
            lines.MoveNext();
            while (lines.MoveNext())
            {
                var line = lines.Current;
                if (line == "")
                {
                    scanners.Add(beacons);
                    beacons = new List<Pos3>();
                    lines.MoveNext();
                    continue;
                }

                var coords = line.Split(",").Select(int.Parse).ToArray();
                beacons.Add(new Pos3(coords[0], coords[1], coords[2]));
            }
            scanners.Add(beacons);
            return scanners;
        }

        private static readonly Func<Pos3, Pos3>[] Rotations;

        static Day19()
        {
            var refl1 = new Func<Pos3, Pos3>[] {
                p => p.ReflectY(),
                p => p.ReflectZ(),
                p => p.ReflectYZ(),
                p => p.ReflectYZNeg(),
            };
            var refl2 = new Func<Pos3, Pos3>[] {
                p => p.ReflectY(),
                p => p.ReflectX(),
                p => p.ReflectXY(),
                p => p.ReflectXYNeg(),
                p => p.ReflectXZ(),
                p => p.ReflectXZNeg(),
            };
            Rotations = refl1.SelectMany(r1 => refl2.Select(r2 => r1.Compose(r2))).ToArray();
        }

        public static (List<Pos3>, Pos3) MatchBeacons(List<Pos3> beacons1, List<Pos3> beacons2)
        {
            for (int i = 0; i < beacons1.Count; i++)
            {
                foreach (var rot in Rotations)
                {
                    var b2 = beacons2.Select(rot).ToList();
                    for (int j = 0; j < beacons2.Count; j++)
                    {
                        var delta = beacons1[i] - b2[j];
                        var rb2 = b2.Select(pos => pos + delta).ToList();
                        var common = beacons1.Intersect(rb2).Count();
                        if (common >= 12)
                        {
                            Console.WriteLine($"match at orientation {rot(new Pos3(1, 2, 3))}");
                            Console.WriteLine($"delta relative to scanner0 is {delta}");
                            return (rb2, delta);
                        }
                    }
                }
            }
            return (null, new Pos3());
        }
    }
}
