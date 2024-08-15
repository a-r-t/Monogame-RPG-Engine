using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Engine.Entity
{
    public class Sprite : Rectangle, IntersectableRectangle
    {
        public Texture2D Image { get; private set; }
        public SpriteEffects SpriteEffect { get; set; }
        private Rectangle bounds;
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(X + bounds.X1 * Scale, Y + bounds.Y1 * Scale, bounds.Width, bounds.Height, Scale);
            }
            set
            {
                if (value != null)
                {
                    bounds = new Rectangle(value.X1, value.Y1, value.Width, value.Height, 1);
                }
            }
        }

        public Rectangle BoundsDimensions
        {
            get
            {
                return bounds;
            }
        }

        public Sprite(Texture2D image)
            : base(0, 0, image.Width, image.Height, 1)
        {
            Image = image;
            Bounds = new Rectangle(0, 0, image.Width, image.Height);
            SpriteEffect = SpriteEffects.None;
        }

        public Sprite(Texture2D image, float x, float y)
            : base(x, y, image.Width, image.Height, 1)
        {
            Image = image;
            Bounds = new Rectangle(0, 0, image.Width, image.Height);
            SpriteEffect = SpriteEffects.None;
        }

        public Sprite(Texture2D image, float x, float y, SpriteEffects spriteEffect)
            : base(x, y, image.Width, image.Height, 1)
        {
            Image = image;
            Bounds = new Rectangle(0, 0, image.Width, image.Height);
            SpriteEffect = spriteEffect;
        }

        public void SetImage(Texture2D texture)
        {
            Image = texture;
        }

        public virtual void SetBounds(float x, float y, int width, int height)
        {
            Bounds = new Rectangle(x, y, width, height, 1);
        }

        public override Rectangle GetIntersectRectangle()
        {
            return Bounds;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(GraphicsHandler graphicsHandler)
        {
            graphicsHandler.DrawImage(Image, new Vector2(X, Y), spriteEffects: SpriteEffect, scale: new Vector2(Scale, Scale));
        }

        public void DrawBounds(GraphicsHandler graphicsHandler, Color color)
        {
            Rectangle tempBounds = new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height, Scale);
            tempBounds.Color = color;
            tempBounds.Draw(graphicsHandler);
        }

        public override string ToString()
        {
            return string.Format("Sprite: x={0} y={1} width={2} height={3} bounds=({4}, {5}, {6}, {7})", X, Y, Width, Height, Bounds.X1, Bounds.Y1, Bounds.Width, Bounds.Height);
        }
    }
}
