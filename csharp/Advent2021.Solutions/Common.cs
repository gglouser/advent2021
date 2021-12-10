using System.Collections.Generic;

namespace Advent2021.Solutions
{
    public struct Pos
    {
        public int X;
        public int Y;

        public Pos(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public static Pos operator +(Pos a, Pos b)
        {
            return new Pos(a.X + b.X, a.Y + b.Y);
        }
    }

    public class Grid<T>
    {
        public T[][] Elems;

        public Grid(T[][] elems)
        {
            Elems = elems;
        }

        public int Width() => Elems[0].Length;
        public int Height() => Elems.Length;

        public T Get(int x, int y) => Elems[y][x];
        public T Get(Pos pos) => Elems[pos.Y][pos.X];

        public IEnumerable<Pos> adjacent(Pos pos)
        {
            if (pos.X > 0) yield return new Pos(pos.X - 1, pos.Y);
            if (pos.X < Width() - 1) yield return new Pos(pos.X + 1, pos.Y);
            if (pos.Y > 0) yield return new Pos(pos.X, pos.Y - 1);
            if (pos.Y < Height() - 1) yield return new Pos(pos.X, pos.Y + 1);
        }
    }
}
