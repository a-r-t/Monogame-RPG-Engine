using Engine.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.FontGraphics
{
    public abstract class FontGraphic
    {
        public string Text { get; set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; }

        public FontGraphic(string text, Vector2 position, Color color)
        {
            Text = text;
            Position = position;
            Color = color;
        }

        public void SetLocation(float dx, float dy)
        {
            Position = new Vector2(dx, dy);
        }

        public void MoveX(float dx)
        {
            Position += new Vector2(dx, 0);
        }

        public void MoveY(float dy)
        {
            Position += new Vector2(0, dy);
        }

        public void MoveRight(float dx)
        {
            Position += new Vector2(dx, 0);
        }

        public void MoveLeft(float dx)
        {
            Position += new Vector2(-dx, 0);
        }

        public void MoveDown(float dy)
        {
            Position += new Vector2(0, dy);
        }

        public void MoveUp(float dy)
        {
            Position += new Vector2(0, -dy);
        }

        public abstract void Draw(GraphicsHandler graphicsHandler);
    }
}
