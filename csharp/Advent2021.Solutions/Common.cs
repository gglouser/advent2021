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
}
