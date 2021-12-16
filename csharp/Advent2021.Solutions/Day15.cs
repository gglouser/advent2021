using System.Collections;
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

        public class State
        {
            public Pos Pos;
            public List<Pos> Path;
            public int Risk;

            public State()
            {
                Pos = new Pos();
                Path = new List<Pos>();
                Risk = 0;
            }

            public State(Pos pos, List<Pos> path, int risk)
            {
                Pos = pos;
                Path = path;
                Risk = risk;
            }

            public bool CanGoTo(Pos next) => !Path.Contains(next);

            public State MoveTo(Pos next, int risk)
            {
                var path = Path.ToList();
                path.Add(Pos);
                return new State(next, path, Risk + risk);
            }
        }

        public class PriorityQueue<T>
        {
            private List<(int, T)> Elems = new List<(int, T)>();
            public void Enqueue(int priority, T item)
            {
                Elems.Add((priority, item));
                UpHeap(Elems.Count - 1);
            }
            public T Dequeue()
            {
                Swap(0, Elems.Count - 1);
                var x = Elems.Last();
                Elems.RemoveAt(Elems.Count - 1);
                DownHeap(0);
                return x.Item2;
            }
            public bool Any()
            {
                return Elems.Any();
            }
            private void Swap(int a, int b)
            {
                var tmp = Elems[a];
                Elems[a] = Elems[b];
                Elems[b] = tmp;
            }
            private void UpHeap(int pos)
            {
                if (pos == 0) return;
                int parent = (pos - 1) / 2;
                if (Elems[pos].Item1 < Elems[parent].Item1)
                {
                    Swap(pos, parent);
                    UpHeap(parent);
                }
            }
            private void DownHeap(int pos)
            {
                var left = 2 * pos + 1;
                var right = 2 * pos + 2;
                var smallest = pos;
                if (left < Elems.Count && Elems[left].Item1 < Elems[smallest].Item1)
                    smallest = left;
                if (right < Elems.Count && Elems[right].Item1 < Elems[smallest].Item1)
                    smallest = right;
                if (smallest != pos)
                {
                    Swap(pos, smallest);
                    DownHeap(smallest);
                }
            }
        }

        public static int LowestRisk(Grid<int> cave)
        {
            var queue = new PriorityQueue<State>();
            queue.Enqueue(0, new State());
            var goal = new Pos(cave.Width() - 1, cave.Height() - 1);
            var visited = new HashSet<Pos>();

            while (queue.Any())
            {
                var state = queue.Dequeue();
                if (state.Pos.Equals(goal)) return state.Risk;
                if (visited.Contains(state.Pos)) continue;
                visited.Add(state.Pos);
                foreach (var adj in cave.Adjacent(state.Pos))
                {
                    if (!state.CanGoTo(adj)) continue;
                    var next = state.MoveTo(adj, cave[adj]);
                    var nextScore = next.Risk + cave.Width() - 1 - next.Pos.X
                        + cave.Height() - 1 - next.Pos.Y;
                    queue.Enqueue(nextScore, next);
                }
            }

            return -1;
        }

        public static Grid<int> GrowMap(Grid<int> cave)
        {
            int[][] bigCave = new int[5 * cave.Height()][];
            for (int i = 0; i < bigCave.Length; i++)
                bigCave[i] = new int[5 * cave.Width()];

            for (int i = 0; i < bigCave.Length; i++)
            {
                int y = i % cave.Height();
                int ywraps = i / cave.Height();
                for (int j = 0; j < bigCave[0].Length; j++)
                {
                    int x = j % cave.Width();
                    int xwraps = j / cave.Width();
                    bigCave[i][j] = (cave[new Pos(x, y)] + xwraps + ywraps - 1) % 9 + 1;
                }
            }

            return new Grid<int>(bigCave);
        }
    }
}
