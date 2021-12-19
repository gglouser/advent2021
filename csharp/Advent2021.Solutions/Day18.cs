using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent2021.Solutions.Day18
{
    public record Result(int Part1, int Part2) { }

    public static class Day18
    {
        public static Result Solve(IEnumerable<string> input)
        {
            var nums = input.Select(SFNum.Parse).ToList();

            var part1 = nums.Aggregate((a, b) => a + b).Magnitude();

            var max = 0;
            for (int i = 0; i < nums.Count; i++)
            {
                for (int j = 0; j < nums.Count; j++)
                {
                    if (i == j) continue;
                    var mag = (nums[i] + nums[j]).Magnitude();
                    if (mag > max) max = mag;
                }
            }
            var part2 = max;

            return new Result(part1, part2);
        }
    }

    public class SFNum
    {
        public int Value;
        public SFNum Left;
        public SFNum Right;

        public SFNum(int value)
        {
            Value = value;
        }

        public SFNum(SFNum left, SFNum right)
        {
            Value = -1;
            Left = left;
            Right = right;
            left.RightMost().LinkRight(right.LeftMost());
        }

        public static SFNum Parse(string s)
        {
            var stack = new Stack<SFNum>();
            foreach (var c in s)
            {
                if (char.IsDigit(c))
                {
                    stack.Push(new SFNum(c - '0'));
                }
                else if (c == ']')
                {
                    var right = stack.Pop();
                    var left = stack.Pop();
                    stack.Push(new SFNum(left, right));
                }
            }
            var result = stack.Pop();
            if (stack.Any()) throw new Exception("invalid parse");
            return result;
        }

        public static SFNum operator +(SFNum a, SFNum b)
        {
            var n = new SFNum(a.Clone(), b.Clone());
            n.Reduce();
            return n;
        }

        public override string ToString()
            => Value >= 0 ? Value.ToString() : $"[{Left.ToString()},{Right.ToString()}]";

        public SFNum Clone()
            => Value >= 0 ? new SFNum(Value) : new SFNum(Left.Clone(), Right.Clone());

        public SFNum LeftMost() => Value >= 0 ? this : Left.LeftMost();

        public SFNum RightMost() => Value >= 0 ? this : Right.RightMost();

        public void LinkLeft(SFNum left)
        {
            Left = left;
            if (left != null) left.Right = this;
        }

        public void LinkRight(SFNum right)
        {
            Right = right;
            if (right != null) right.Left = this;
        }

        public bool TryExplode(int depth)
        {
            if (Value >= 0) return false;

            if (depth < 4)
                return Left.TryExplode(depth + 1) || Right.TryExplode(depth + 1);

            // Explode!
            var leftleft = Left.Left;
            if (leftleft != null) leftleft.Value += Left.Value;

            var rightright = Right.Right;
            if (rightright != null) rightright.Value += Right.Value;

            Value = 0;
            LinkLeft(leftleft);
            LinkRight(rightright);
            return true;
        }

        public bool TrySplit()
        {
            if (Value < 0)
                return Left.TrySplit() || Right.TrySplit();

            if (Value <= 9) return false;

            // Split!
            var l = new SFNum(Value / 2);
            l.LinkLeft(Left);
            Left = l;

            var r = new SFNum(Value - l.Value);
            r.LinkRight(Right);
            Right = r;

            Value = -1;
            Left.LinkRight(Right);
            return true;
        }

        public void Reduce()
        {
            while (TryExplode(0) || TrySplit()) { }
        }

        public int Magnitude()
            => Value >= 0 ? Value : 3 * Left.Magnitude() + 2 * Right.Magnitude();
    }
}
