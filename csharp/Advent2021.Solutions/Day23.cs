using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Advent2021.Solutions.Day23
{
    public record Result(int Part1, int Part2) { }

    public static class Day23
    {
        public static Result Solve(IEnumerable<string> input)
        {
            var initMap = input.ToArray();

            var initState = State.FromMap(initMap);
            var part1 = Search(initState);

            var extendedState = Extend(initState);
            var part2 = Search(extendedState);

            return new Result(part1, part2);
        }

        public static int Search(State initState)
        {
            var queue = new PriorityQueue<State>();
            queue.Enqueue(0, initState);
            var visited = new HashSet<string>();

            while (queue.Any())
            {
                var state = queue.Dequeue();
                if (state.IsGoal())
                {
                    // Console.WriteLine("found: " + state.Cost + " :: " + state.Token);
                    // state.PrintSequence();
                    return state.Cost;
                }
                var smap = state.Token;
                if (visited.Contains(smap)) continue;
                visited.Add(smap);

                foreach (var nextState in state.Branch())
                {
                    queue.Enqueue(nextState.Cost, nextState);
                }
            }

            return -1;
        }

        public static State Extend(State state)
        {
            var newMap = (char[])state.Map.Clone();
            Array.Resize<char>(ref newMap, state.Map.Length + 8);
            newMap[19] = newMap[11];
            newMap[20] = newMap[12];
            newMap[21] = newMap[13];
            newMap[22] = newMap[14];

            newMap[11] = 'D';
            newMap[12] = 'C';
            newMap[13] = 'B';
            newMap[14] = 'A';
            newMap[15] = 'D';
            newMap[16] = 'B';
            newMap[17] = 'A';
            newMap[18] = 'C';

            return new State
            {
                Map = newMap,
                Moved = new bool[16],
                Links = Links2
            };
        }

        public static readonly Dictionary<int, List<(int, int)>> Links1;
        public static readonly Dictionary<int, List<(int, int)>> Links2;

        static Day23()
        {
            (int, int, int)[] link = {
                (0, 1, 1),
                (1, 2, 2),
                (1, 7, 2),
                (2, 3, 2),
                (2, 7, 2),
                (2, 8, 2),
                (3, 4, 2),
                (3, 8, 2),
                (3, 9, 2),
                (4, 5, 2),
                (4, 9, 2),
                (4, 10, 2),
                (5, 6, 1),
                (5, 10, 2),
                (7, 11, 1),
                (8, 12, 1),
                (9, 13, 1),
                (10, 14, 1),
            };
            Links1 = new Dictionary<int, List<(int, int)>>();
            Links2 = new Dictionary<int, List<(int, int)>>();
            foreach (var (a, b, step) in link)
            {
                if (!Links1.ContainsKey(a)) Links1.Add(a, new());
                Links1[a].Add((b, step));
                if (!Links1.ContainsKey(b)) Links1.Add(b, new());
                Links1[b].Add((a, step));

                if (!Links2.ContainsKey(a)) Links2.Add(a, new());
                Links2[a].Add((b, step));
                if (!Links2.ContainsKey(b)) Links2.Add(b, new());
                Links2[b].Add((a, step));
            }

            (int, int, int)[] extraLink = {
                (11, 15, 1),
                (12, 16, 1),
                (13, 17, 1),
                (14, 18, 1),
                (15, 19, 1),
                (16, 20, 1),
                (17, 21, 1),
                (18, 22, 1),
            };
            foreach (var (a, b, step) in extraLink)
            {
                if (!Links2.ContainsKey(a)) Links2.Add(a, new());
                Links2[a].Add((b, step));
                if (!Links2.ContainsKey(b)) Links2.Add(b, new());
                Links2[b].Add((a, step));
            }
        }
    }

    public class State
    {
        // #############
        // #01.2.3.4.56#
        // ###7#8#9#A###
        //   #B#C#D#E#
        //   #########

        public char[] Map;
        public bool[] Moved;
        public int Cost;
        public State parent;
        public Dictionary<int, List<(int, int)>> Links;

        private static readonly char[] DestType = { 'A', 'B', 'C', 'D' };

        public static State FromMap(string[] map)
        {
            (int, int)[] mapMap = {
                (1, 1),
                (1, 2),
                (1, 4),
                (1, 6),
                (1, 8),
                (1, 10),
                (1, 11),
                (2, 3),
                (2, 5),
                (2, 7),
                (2, 9),
                (3, 3),
                (3, 5),
                (3, 7),
                (3, 9),
            };
            var spots = mapMap.Select(x => map[x.Item1][x.Item2]).ToArray();
            return new State { Map = spots, Moved = new bool[8], Links = Day23.Links1 };
        }

        public static int CalcCost(char amphipod, int steps)
        {
            switch (amphipod)
            {
                case 'A': return steps;
                case 'B': return steps * 10;
                case 'C': return steps * 100;
                case 'D': return steps * 1000;
                default: throw new Exception("invalid amphipod type");
            }
        }

        public string Token { get => new string(Map); }

        public List<(int, int)> ValidMovesFrom(int fromPos)
        {
            Debug.Assert(Map[fromPos] != '.');
            Debug.Assert(fromPos < 7 || !Moved[fromPos - 7]);

            var startInHallway = fromPos < 7;
            var moves = new List<(int, int)>();

            var visited = new HashSet<int>();
            visited.Add(fromPos);

            var queue = new PriorityQueue<(int, int)>();
            foreach (var y in Links[fromPos])
            {
                queue.Enqueue(y.Item2, (y.Item1, y.Item2));
            }

            while (queue.Any())
            {
                var x = queue.Dequeue();
                var pos = x.Item1;
                if (visited.Contains(pos)) continue;
                visited.Add(pos);
                if (Map[pos] != '.') continue;

                var inHallway = pos < 7;
                if (inHallway && !startInHallway)
                {
                    // Move into hallway.
                    moves.Add(x);
                }
                else if (!inHallway)
                {
                    var roomNum = (pos - 7) % 4;
                    if (Map[fromPos] == DestType[roomNum])
                    {
                        // Moving into correct room.
                        // Move all the way in.
                        // Don't block any that need to move out.
                        var ok = true;
                        for (var roomPos = pos + 4; roomPos < Map.Length; roomPos += 4)
                        {
                            if (roomPos == fromPos || Map[roomPos] != DestType[roomNum])
                                ok = false;
                        }
                        if (ok)
                        {
                            moves.Add(x);
                        }
                    }
                }

                foreach (var y in Links[pos])
                {
                    var c = x.Item2 + y.Item2;
                    queue.Enqueue(c, (y.Item1, c));
                }
            }

            return moves;
        }

        public List<State> Branch()
        {
            var moves = new List<State>();
            for (int pos = 0; pos < Map.Length; pos++)
            {
                if (Map[pos] == '.') continue;
                if (pos >= 7 && Moved[pos - 7]) continue;

                foreach (var x in ValidMovesFrom(pos))
                {
                    var newMap = (char[])Map.Clone();
                    newMap[x.Item1] = Map[pos];
                    newMap[pos] = '.';
                    var newMoved = (bool[])Moved.Clone();
                    if (pos >= 7) newMoved[pos - 7] = true;
                    moves.Add(new State
                    {
                        Map = newMap,
                        Moved = newMoved,
                        Cost = Cost + CalcCost(Map[pos], x.Item2),
                        parent = this,
                        Links = Links
                    });
                }
            }
            return moves;
        }

        public bool IsGoal()
        {
            for (int room = 0; room < 4; room++)
            {
                for (int roomPos = 0; roomPos < (Map.Length - 7) / 4; roomPos++)
                {
                    var pos = roomPos * 4 + room + 7;
                    if (Map[pos] != DestType[room]) return false;
                }
            }
            return true;
        }

        public void Print()
        {
            Console.WriteLine("" + Map[0] + Map[1] + "." + Map[2] + "." + Map[3]
                + "." + Map[4] + "." + Map[5] + Map[6]
                + " (" + Cost + ")");
            for (int i = 7; i < Map.Length; i += 4)
            {
                Console.WriteLine("  " + Map[i] + " " + Map[i + 1] + " " + Map[i + 2] + " " + Map[i + 3]);
            }
            Console.WriteLine();
        }

        public void PrintSequence()
        {
            var s = this;
            while (s != null)
            {
                s.Print();
                s = s.parent;
            }
        }
    }
}
