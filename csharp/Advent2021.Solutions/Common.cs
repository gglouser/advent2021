using System;
using System.Collections;
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

    public struct Pos3
    {
        public int X;
        public int Y;
        public int Z;

        public Pos3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }

        public int Abs() => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);

        public static Pos3 operator +(Pos3 a, Pos3 b)
        {
            return new Pos3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Pos3 operator -(Pos3 a, Pos3 b)
        {
            return new Pos3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public Pos3 ReflectX() => new(-X, Y, Z);
        public Pos3 ReflectY() => new(X, -Y, Z);
        public Pos3 ReflectZ() => new(X, Y, -Z);
        public Pos3 ReflectXY() => new(Y, X, Z);
        public Pos3 ReflectXYNeg() => new(-Y, -X, Z);
        public Pos3 ReflectXZ() => new(Z, Y, X);
        public Pos3 ReflectXZNeg() => new(-Z, Y, -X);
        public Pos3 ReflectYZ() => new(X, Z, Y);
        public Pos3 ReflectYZNeg() => new(X, -Z, -Y);
    }

    public class Grid<T>
    {
        public T[][] Elems;

        public Grid(T[][] elems)
        {
            Elems = elems;
        }

        public static Grid<T> FromGenerator(int width, int height, Func<Pos, T> fun)
        {
            var elems = new T[height][];
            for (int row = 0; row < height; row++)
            {
                elems[row] = new T[width];
                for (int col = 0; col < width; col++)
                {
                    elems[row][col] = fun(new Pos(col, row));
                }
            }
            return new Grid<T>(elems);
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

    public class Counter<T> : IEnumerable<(T, long)>
    {
        private Dictionary<T, long> Counts = new Dictionary<T, long>();

        public long this[T item]
        {
            get => Counts.ContainsKey(item) ? Counts[item] : 0;
            set => SetCount(item, value);
        }

        public IEnumerator<(T, long)> GetEnumerator()
        {
            foreach (var item in Counts)
            {
                yield return (item.Key, item.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void SetCount(T item, long value)
        {
            if (Counts.ContainsKey(item))
                Counts[item] = value;
            else
                Counts.Add(item, value);
        }
    }

    public class PriorityQueue<T>
    {
        private List<(int, T)> Elems = new();

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

    public static class Extensions
    {
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<(TKey, TValue)> list) =>
            list.ToDictionary(x => x.Item1, x => x.Item2);

        public static Func<A, C> Compose<A, B, C>(this Func<A, B> f, Func<B, C> g) =>
            x => g(f(x));
    }
}
