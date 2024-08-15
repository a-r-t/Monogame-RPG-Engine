using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Entity
{
    public class Frame : Sprite
    {
        public int Delay { get; private set; }

        public Frame(Texture2D image, SpriteEffects spriteEffect, float scale, Rectangle bounds, int delay)
            : base(image, 0, 0, spriteEffect)
        {
            Scale = scale;
            Bounds = bounds;
            Delay = delay;
        }

        public Frame(Texture2D image, SpriteEffects spriteEffect, float scale, Rectangle bounds)
            : base(image, 0, 0, spriteEffect)
        {
            Scale = scale;
            Bounds = bounds;
        }

        public Frame Copy()
        {
            return new Frame(Image, SpriteEffect, Scale, BoundsDimensions, Delay);
        }
    }
}
