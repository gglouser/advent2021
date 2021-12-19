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

    public abstract class SFNum
    {
        public abstract SFLiteral LeftMost();
        public abstract SFLiteral RightMost();
        public abstract SFNum Clone();
        public abstract bool TryExplode(int depth);
        public abstract bool TrySplit();
        public abstract int Magnitude();

        public static SFNum Parse(string s)
        {
            Stack<SFNum> stack = new Stack<SFNum>();
            foreach (var c in s)
            {
                if (char.IsDigit(c))
                {
                    stack.Push(new SFLiteral(c - '0'));
                }
                else if (c == ']')
                {
                    var right = stack.Pop();
                    var left = stack.Pop();
                    var num = new SFPair(left, right);
                    stack.Push(num);
                }
            }
            var result = stack.Pop();
            if (stack.Any()) throw new Exception("invalid parse");
            return result;
        }

        public static SFPair operator +(SFNum a, SFNum b)
        {
            var n = new SFPair(a.Clone(), b.Clone());
            n.Reduce();
            return n;
        }
    }

    public class SFPair : SFNum
    {
        public SFNum Left;
        public SFNum Right;

        public SFPair(SFNum left, SFNum right)
        {
            Left = left;
            Right = right;
            left.RightMost().LinkRight(right.LeftMost());
        }

        public override string ToString()
        {
            return "[" + Left.ToString() + "," + Right.ToString() + "]";
        }

        public override SFLiteral LeftMost() => Left.LeftMost();

        public override SFLiteral RightMost() => Right.RightMost();

        public override SFNum Clone() => new SFPair(Left.Clone(), Right.Clone());

        public override bool TryExplode(int depth)
        {
            if (depth < 4)
            {
                return (Left.TryExplode(depth + 1))
                    || (Right.TryExplode(depth + 1));
            }
            else
            {
                if (Left is SFPair)
                {
                    Left = ((SFPair)Left).ExplodePatch();
                    return true;
                }
                else if (Right is SFPair)
                {
                    Right = ((SFPair)Right).ExplodePatch();
                    return true;
                }
                return false;
            }
        }

        public SFLiteral ExplodePatch()
        {
            var leftOrd = (SFLiteral)Left;
            var leftleft = leftOrd.Left;
            if (leftleft != null) leftleft.Value += leftOrd.Value;
            var rightOrd = (SFLiteral)Right;
            var rightright = rightOrd.Right;
            if (rightright != null) rightright.Value += rightOrd.Value;
            var zero = new SFLiteral(0);
            zero.LinkLeft(leftleft);
            zero.LinkRight(rightright);
            return zero;
        }

        public override bool TrySplit()
        {
            if (Left is SFLiteral)
            {
                var n = (SFLiteral)Left;
                if (n.Value > 9)
                {
                    Left = n.Split();
                    return true;
                }
            }
            else if (((SFPair)Left).TrySplit())
            {
                return true;
            }

            if (Right is SFLiteral)
            {
                var n = (SFLiteral)Right;
                if (n.Value > 9)
                {
                    Right = n.Split();
                    return true;
                }
            }
            else
            {
                return (((SFPair)Right).TrySplit());
            }

            return false;
        }

        public override int Magnitude() => 3 * Left.Magnitude() + 2 * Right.Magnitude();

        public void Reduce()
        {
            while (TryExplode(1) || TrySplit()) { }
        }
    }

    public class SFLiteral : SFNum
    {
        public int Value;
        public SFLiteral Left;
        public SFLiteral Right;

        public SFLiteral(int value)
        {
            Value = value;
        }

        public override SFLiteral LeftMost() => this;
        public override SFLiteral RightMost() => this;
        public override SFNum Clone() => new SFLiteral(Value);

        public override bool TryExplode(int depth) => false;

        public override bool TrySplit() => false;

        public override int Magnitude() => Value;

        public override string ToString()
        {
            return Value.ToString();
        }

        public void LinkLeft(SFLiteral left)
        {
            Left = left;
            if (left != null) left.Right = this;
        }

        public void LinkRight(SFLiteral right)
        {
            Right = right;
            if (right != null) right.Left = this;
        }

        public SFPair Split()
        {
            var l = new SFLiteral(Value / 2);
            var r = new SFLiteral(Value - l.Value);
            l.LinkLeft(Left);
            r.LinkRight(Right);
            return new SFPair(l, r);
        }
    }
}
