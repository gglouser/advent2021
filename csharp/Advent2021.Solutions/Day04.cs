using System.Collections.Generic;
using System.Linq;

namespace Advent2021.Solutions.Day04
{
    public record Result(int Part1, int Part2) { }

    public class Board
    {
        private List<List<int>> numbers;
        private List<List<bool>> marks;

        public Board(List<List<int>> numbers)
        {
            this.numbers = numbers;
            this.marks = numbers.Select(row => row.Select(_ => false).ToList()).ToList();
        }

        public void MarkNumber(int number)
        {
            for (int row = 0; row < marks.Count; row++)
            {
                for (int col = 0; col < marks[row].Count; col++)
                {
                    if (numbers[row][col] == number)
                    {
                        marks[row][col] = true;
                        return;
                    }
                }
            }
        }

        public bool CheckBingo()
        {
            for (int i = 0; i < marks.Count; i++)
            {
                if (marks[i].All(m => m)) return true;
                if (marks.All(row => row[i])) return true;
            }
            return false;
        }

        public int Score()
        {
            int score = 0;
            for (int row = 0; row < marks.Count; row++)
            {
                for (int col = 0; col < marks[row].Count; col++)
                {
                    if (!marks[row][col])
                    {
                        score += numbers[row][col];
                    }
                }
            }
            return score;
        }
    }

    public static class Day04
    {
        public static Result Solve(IEnumerable<string> input)
        {
            var numStrs = input.Take(2).First().Split(',');
            var nums = numStrs.Select(int.Parse).ToArray();

            var boards = ParseBoards(input.Skip(2));

            var part1 = 0;
            var part2 = 0;

            foreach (var num in nums)
            {
                foreach (var board in boards)
                {
                    board.MarkNumber(num);
                    if (part1 == 0 && board.CheckBingo())
                    {
                        part1 = board.Score() * num;
                    }
                }
                if (boards.Count() == 1 && boards[0].CheckBingo())
                {
                    part2 = boards[0].Score() * num;
                    break;
                }
                boards = boards.Where(board => !board.CheckBingo()).ToList();
            }

            return new Result(part1, part2);
        }

        public static List<Board> ParseBoards(IEnumerable<string> input)
        {
            List<Board> boards = new List<Board>();
            while (input.Any())
            {
                var board = input.TakeWhile(line => line != "")
                    .Select(line =>
                    {
                        var strs = System.Text.RegularExpressions.Regex.Split(line.Trim(), "\\s+");
                        return strs.Select(int.Parse).ToList();
                    })
                    .ToList();
                boards.Add(new Board(board));
                input = input.SkipWhile(line => line != "").Skip(1);
            }
            return boards;
        }
    }
}
