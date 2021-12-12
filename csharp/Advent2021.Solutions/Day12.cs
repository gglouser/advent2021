using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent2021.Solutions.Day12
{
    public record Result(int Part1, int Part2) { }

    public class State
    {
        public string Cave;
        public List<string> Path;
        public bool HasSmallRevisit;

        public State(string cave, List<string> path, bool hasSmallRevisit)
        {
            Cave = cave;
            Path = path;
            HasSmallRevisit = hasSmallRevisit;
        }

        public State() : this("start", new List<string>(), false)
        {
            Path.Add("start");
        }

        public bool OkToMoveTo(string cave, bool allowRevisit)
        {
            return char.IsUpper(cave[0])
                || !Path.Contains(cave)
                || (allowRevisit && cave != "start" && cave != "end" && !HasSmallRevisit);
        }

        public State MoveTo(string cave)
        {
            var newPath = Path.ToList();
            newPath.Add(cave);
            var hasSmallRevisit = HasSmallRevisit
                || (char.IsLower(cave[0]) && Path.Contains(cave));
            return new State(cave, newPath, hasSmallRevisit);
        }
    }

    public static class Day12
    {
        public static Result Solve(IEnumerable<string> input)
        {
            var caves = ParseCaves(input);

            var part1 = FindPaths(caves, false).Count;
            var part2 = FindPaths(caves, true).Count;

            return new Result(part1, part2);
        }

        public static Dictionary<string, List<string>> ParseCaves(IEnumerable<string> input)
        {
            var adj = new Dictionary<string, List<string>>();
            foreach (var line in input)
            {
                var caves = line.Split("-");
                if (!adj.ContainsKey(caves[0]))
                    adj.Add(caves[0], new List<string>());
                adj[caves[0]].Add(caves[1]);
                if (!adj.ContainsKey(caves[1]))
                    adj.Add(caves[1], new List<string>());
                adj[caves[1]].Add(caves[0]);
            }
            return adj;
        }

        public static List<List<string>> FindPaths(
            Dictionary<string, List<string>> caves, bool allowRevisit)
        {
            Stack<State> queue = new Stack<State>();
            queue.Push(new State());

            List<List<string>> paths = new List<List<string>>();
            State state;
            while (queue.TryPop(out state))
            {
                if (state.Cave == "end")
                {
                    paths.Add(state.Path);
                    continue;
                }

                foreach (var nextCave in caves[state.Cave])
                {
                    if (state.OkToMoveTo(nextCave, allowRevisit))
                    {
                        queue.Push(state.MoveTo(nextCave));
                    }
                }
            }

            return paths;
        }
    }
}
