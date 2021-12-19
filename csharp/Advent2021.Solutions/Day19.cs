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

        public static Pos3 ChangeOrientation(Pos3 beacon, int which)
        {
            var x = beacon.X;
            var y = beacon.Y;
            var z = beacon.Z;
            switch (which)
            {
                case 0: return new Pos3(x, y, z);
                case 1: return new Pos3(x, z, -y);
                case 2: return new Pos3(x, -y, -z);
                case 3: return new Pos3(x, -z, y);
                case 4: return new Pos3(-x, -z, -y);
                case 5: return new Pos3(-x, -y, z);
                case 6: return new Pos3(-x, z, y);
                case 7: return new Pos3(-x, y, -z);
                case 8: return new Pos3(y, z, x);
                case 9: return new Pos3(y, x, -z);
                case 10: return new Pos3(y, -z, -x);
                case 11: return new Pos3(y, -x, z);
                case 12: return new Pos3(-y, -x, -z);
                case 13: return new Pos3(-y, -z, x);
                case 14: return new Pos3(-y, x, z);
                case 15: return new Pos3(-y, z, -x);
                case 16: return new Pos3(z, x, y);
                case 17: return new Pos3(z, y, -x);
                case 18: return new Pos3(z, -x, -y);
                case 19: return new Pos3(z, -y, x);
                case 20: return new Pos3(-z, -y, -x);
                case 21: return new Pos3(-z, -x, y);
                case 22: return new Pos3(-z, y, x);
                case 23: return new Pos3(-z, x, -y);
                default: throw new Exception("illegal orientation");
            }
        }

        public static (List<Pos3>, Pos3) MatchBeacons(List<Pos3> beacons1, List<Pos3> beacons2)
        {
            for (int i = 0; i < beacons1.Count; i++)
            {
                for (int orientation = 0; orientation < 24; orientation++)
                {
                    var b2 = beacons2.Select(pos => ChangeOrientation(pos, orientation)).ToList();
                    for (int j = 0; j < beacons2.Count; j++)
                    {
                        var delta = beacons1[i] - b2[j];
                        var rb2 = b2.Select(pos => pos + delta).ToList();
                        var common = beacons1.Intersect(rb2).Count();
                        if (common >= 12)
                        {
                            Console.WriteLine($"match at orientation {orientation}");
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
