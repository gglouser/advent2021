using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent2021.Solutions.Day21
{
    public record Result(long Part1, long Part2) { }

    public static class Day21
    {
        public static Result Solve(IEnumerable<string> input)
        {
            int[] startPos = input.Select(line => int.Parse(line.Split(": ")[1])).ToArray();

            var part1 = Play(startPos[0], startPos[1]);
            var part2 = CountUniverses(startPos[0], startPos[1]);

            return new Result(part1, part2);
        }

        public static long Play(int start1, int start2)
        {
            int pos1 = start1;
            int pos2 = start2;
            int score1 = 0;
            int score2 = 0;
            int die = 1;
            int rolls = 0;
            while (true)
            {
                var r1 = die;
                die = die % 100 + 1;
                rolls++;
                var r2 = die;
                die = die % 100 + 1;
                rolls++;
                var r3 = die;
                die = die % 100 + 1;
                rolls++;
                pos1 = (pos1 + r1 + r2 + r3 - 1) % 10 + 1;
                score1 += pos1;
                if (score1 >= 1000)
                    return (score2 * rolls);

                var r4 = die;
                die = die % 100 + 1;
                rolls++;
                var r5 = die;
                die = die % 100 + 1;
                rolls++;
                var r6 = die;
                die = die % 100 + 1;
                rolls++;
                pos2 = (pos2 + r4 + r5 + r6 - 1) % 10 + 1;
                score2 += pos2;
                if (score2 >= 1000)
                    return (score1 * rolls);
            }

            throw new Exception("unreachable");
        }

        public static (int, long)[] GetDieOutcomes()
        {
            var dieOutcomes = new Counter<int>();
            for (int die1 = 1; die1 <= 3; die1++)
                for (int die2 = 1; die2 <= 3; die2++)
                    for (int die3 = 1; die3 <= 3; die3++)
                        dieOutcomes[die1 + die2 + die3]++;
            return dieOutcomes.ToArray();
        }

        public static long CountUniverses(int start1, int start2)
        {
            const int WinningScore = 21;

            var dieOutcomes = GetDieOutcomes();

            var state = new long[10, 10, WinningScore, WinningScore];
            state[start1 - 1, start2 - 1, 0, 0] = 1;

            long totalPlayer1Wins = 0;
            long totalPlayer2Wins = 0;
            bool keepGoing = true;
            for (int turn = 0; keepGoing; turn++)
            {
                // Console.WriteLine($"{turn}: {totalPlayer1Wins} vs {totalPlayer2Wins}");
                keepGoing = false;
                var newState = new long[10, 10, WinningScore, WinningScore];

                for (int pos1 = 0; pos1 <= state.GetUpperBound(0); pos1++)
                {
                    for (int pos2 = 0; pos2 <= state.GetUpperBound(1); pos2++)
                    {
                        for (int score1 = 0; score1 <= state.GetUpperBound(2); score1++)
                        {
                            for (int score2 = 0; score2 <= state.GetUpperBound(3); score2++)
                            {
                                var count = state[pos1, pos2, score1, score2];
                                if (count == 0) continue;
                                keepGoing = true;
                                if (turn % 2 == 0)
                                {
                                    // Player 1 move
                                    foreach (var (roll, weight) in dieOutcomes)
                                    {
                                        var newPos = (pos1 + roll) % 10;
                                        var newScore = score1 + newPos + 1;
                                        if (newScore < WinningScore)
                                            newState[newPos, pos2, newScore, score2] += count * weight;
                                        else
                                            totalPlayer1Wins += count * weight;
                                    }
                                }
                                else
                                {
                                    // Player 2 move
                                    foreach (var (roll, weight) in dieOutcomes)
                                    {
                                        var newPos = (pos2 + roll) % 10;
                                        var newScore = score2 + newPos + 1;
                                        if (newScore < WinningScore)
                                            newState[pos1, newPos, score1, newScore] += count * weight;
                                        else
                                            totalPlayer2Wins += count * weight;
                                    }
                                }

                            }
                        }
                    }
                }

                state = newState;
            }

            return Math.Max(totalPlayer1Wins, totalPlayer2Wins);
        }
    }
}
