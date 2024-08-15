using Engine.Core;
using Engine.Extensions;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace Engine.FontGraphics
{
    public class SpriteFontGraphic : FontGraphic
    {
        public SpriteFont SpriteFont { get; set; }
        public Color OutlineColor { get; set; } = Color.Transparent;
        public int OutlineThickness { get; set; } = 0;
        private FontSystem fontSystem;


        public SpriteFontGraphic(string text, SpriteFont spriteFont, Vector2 position, Color color) : base(text, position, color)
        {
            fontSystem = new FontSystem();
            fontSystem.AddFont(File.ReadAllBytes(@"Content/Fonts/SpriteFonts/Arial.ttf"));
            SpriteFont = spriteFont;
        }

        public override void Draw(GraphicsHandler graphicsHandler)
        {
            if (!OutlineColor.Equals(Color) && !OutlineColor.Equals(Color.Transparent))
            {
                graphicsHandler.DrawStringWithOutline(SpriteFont, Text, new Vector2(Position.X.Round(), Position.Y.Round()), Color, OutlineColor, OutlineThickness);
            }
            else
            {
                graphicsHandler.DrawString(SpriteFont, Text, new Vector2(Position.X.Round(), Position.Y.Round()), Color);
            }
        }

        public void DrawWithParsedNewLines(GraphicsHandler graphicsHandler, int gapBetweenLines)
        {
            int drawLocationY = Position.Y.Round();
            foreach (string line in Text.Split("\n"))
            {
                graphicsHandler.DrawString(SpriteFont, line, new Vector2(Position.X, drawLocationY), Color);
                drawLocationY += SpriteFont.LineSpacing + gapBetweenLines;
            }
        }
    }
}
