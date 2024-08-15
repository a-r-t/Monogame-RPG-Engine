using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Engine.Extensions;
using Engine.Core;

namespace Engine.Entity
{
    public class Rectangle : IntersectableRectangle
    {
        public float X { get; set; }
        public float X1
        {
            get
            {
                return X;
            }
        }
        public float X2
        {
            get
            {
                return X + Width - 1;
            }
        }
        public float Y { get; set; }
        public float Y1
        {
            get
            {
                return Y;
            }
        }
        public float Y2
        {
            get
            {
                return Y + Height - 1;
            }
        }
        private int width;
        public int Width
        {
            get
            {
                return (width * Scale).Round();
            }
            set
            {
                width = value;
            }
        }
        private int height;
        public int Height
        {
            get
            {
                return (height * Scale).Round();
            }
            set
            {
                height = value;
            }
        }
        public float Scale { get; set; }
        public Color Color { get; set; }
        public Color BorderColor { get; set; }
        public int BorderThickness { get; set; }
        public Utils.Point Location
        {
            get
            {
                return new Utils.Point(X, Y);
            }
        }

        public Rectangle(float x, float y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Scale = 1f;
            Color = Color.White;
            BorderColor = Color.Transparent;
            BorderThickness = 0;
        }

        public Rectangle(float x, float y, int width, int height, float scale)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Scale = scale;
            Color = Color.White;
            BorderColor = Color.Transparent;
            BorderThickness = 0;
        }

        public void MoveX(float dx)
        {
            X += dx;
        }

        public void MoveRight(float dx)
        {
            X += dx;
        }

        public void MoveLeft(float dx)
        {
            X -= dx;
        }

        public void MoveY(float dy)
        {
            Y += dy;
        }

        public void MoveDown(float dy)
        {
            Y += dy;
        }

        public void MoveUp(float dy)
        {
            Y -= dy;
        }

        public void SetLocation(float x, float y)
        {
            X = x;
            Y = y;
        }

        public virtual void Update() { }

        public virtual void Draw(GraphicsHandler graphicsHandler)
        {
            graphicsHandler.DrawFilledRectangle(X.Round(), Y.Round(), Width, Height, Color);
            if (!BorderColor.Equals(Color.Transparent) && !BorderColor.Equals(Color))
            {
                graphicsHandler.DrawRectangle(X.Round(), Y.Round(), Width, Height, BorderColor, BorderThickness);
            }
        }

        public virtual Rectangle GetIntersectRectangle()
        {
            return new Rectangle(X, Y, Width, Height);
        }

        // check if this intersects with another rectangle
        public bool Intersects(IntersectableRectangle other)
        {
            Rectangle intersectRectangle = GetIntersectRectangle();
            Rectangle otherIntersectRectangle = other.GetIntersectRectangle();
            return intersectRectangle.X1.Round() < (otherIntersectRectangle.X2 + 1).Round() && (intersectRectangle.X2 + 1).Round() > otherIntersectRectangle.X1.Round() &&
                    intersectRectangle.Y1.Round() < (otherIntersectRectangle.Y2 + 1).Round() && (intersectRectangle.Y2 + 1).Round() > otherIntersectRectangle.Y1.Round();
        }

        // check if this touching with another rectangle
        public bool Touching(IntersectableRectangle other)
        {
            Rectangle intersectRectangle = GetIntersectRectangle();
            Rectangle otherIntersectRectangle = other.GetIntersectRectangle();
            return intersectRectangle.X1.Round() <= (otherIntersectRectangle.X2 + 1).Round() && (intersectRectangle.X2 + 1).Round() >= otherIntersectRectangle.X1.Round() &&
                    intersectRectangle.Y1.Round() <= (otherIntersectRectangle.Y2 + 1).Round() && (intersectRectangle.Y2 + 1).Round() >= otherIntersectRectangle.Y1.Round();
        }

        // calculates the area that a rectangle is overlapping another rectangle by
        // and returns the total number of pixels
        public float GetAreaOverlapped(IntersectableRectangle other)
        {
            Rectangle intersectRectangle = GetIntersectRectangle();
            Rectangle otherIntersectRectangle = other.GetIntersectRectangle();
            if (!Touching(other))
            {
                return 0;
            }
            float width = Math.Abs(Math.Min(intersectRectangle.X2 + 1, otherIntersectRectangle.X2 + 1) - Math.Max(intersectRectangle.X1, otherIntersectRectangle.X1));
            float height = Math.Abs(Math.Min(intersectRectangle.Y2 + 1, otherIntersectRectangle.Y2 + 1) - Math.Max(intersectRectangle.Y1, otherIntersectRectangle.Y1));
            return width * height;
        }

        public override string ToString()
        {
            return string.Format("Rectangle: x={0} y={1} width={2} height={3}", X, Y, Width, Height);
        }
    }
}
