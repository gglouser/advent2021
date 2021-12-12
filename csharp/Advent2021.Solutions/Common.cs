using System;
using System.Collections.Generic;
using System.Linq;

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

        public bool InBounds(Pos pos) =>
            pos.X >= 0 && pos.X < Width() && pos.Y >= 0 && pos.Y < Height();

        public T this[Pos pos]
        {
            get => Elems[pos.Y][pos.X];
            set => Elems[pos.Y][pos.X] = value;
        }

        public Grid<S> Map<S>(Func<T, S> func) =>
            new Grid<S>(Elems.Select(row => row.Select(func).ToArray()).ToArray());

        public IEnumerable<Pos> Positions()
        {
            for (var x = 0; x < Width(); x++)
            {
                for (var y = 0; y < Height(); y++)
                {
                    yield return new Pos(x, y);
                }
            }
        }

        public IEnumerable<T> Values() => Elems.SelectMany(row => row);

        private static Pos[] AdjacentOffsets = {
            new Pos(1, 0), new Pos(0, 1), new Pos(-1, 0), new Pos(0, -1) };

        private static Pos[] NeighborOffsets = {
            new Pos(1, 0), new Pos(1, 1), new Pos(0, 1), new Pos(-1, 1),
            new Pos(-1, 0), new Pos(-1, -1), new Pos(0, -1), new Pos(1, -1) };

        public IEnumerable<Pos> Adjacent(Pos pos) =>
            AdjacentOffsets.Select(o => pos + o).Where(InBounds);

        public IEnumerable<Pos> Neighbors(Pos pos) =>
            NeighborOffsets.Select(o => pos + o).Where(InBounds);
    }
}
