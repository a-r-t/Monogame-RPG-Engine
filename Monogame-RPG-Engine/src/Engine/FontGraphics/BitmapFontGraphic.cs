using Engine.Core;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.FontGraphics
{
    public class BitmapFontGraphic : FontGraphic
    {
        public BitmapFont BitmapFont { get; set; }

        public BitmapFontGraphic(string text, BitmapFont bitmapFont, Vector2 position, Color color) : base(text, position, color)
        {
            BitmapFont = bitmapFont;
        }

        public override void Draw(GraphicsHandler graphicsHandler)
        {
            graphicsHandler.DrawString(BitmapFont, Text, Position, Color);
        }
    }
}
