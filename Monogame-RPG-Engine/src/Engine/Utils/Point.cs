using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Represents a Point on a 2D plane, has some "point math" methods
namespace Engine.Utils
{
    public class Point
    {

        public readonly float X;
        public readonly float Y;

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Point Add(Point point)
        {
            return new Point(X + point.X, Y + point.Y);
        }

        public Point AddX(int dx)
        {
            return new Point(X + dx, Y);
        }

        public Point AddY(int dy)
        {
            return new Point(X, Y + dy);
        }

        public Point Subtract(Point point)
        {
            return new Point(X - point.X, Y - point.Y);
        }

        public Point SubtractX(int dx)
        {
            return new Point(X - dx, Y);
        }

        public Point SubtractY(int dy)
        {
            return new Point(X, Y - dy);
        }

        public override string ToString() { return string.Format("({0}, {1})", X, Y); }
    }


}
