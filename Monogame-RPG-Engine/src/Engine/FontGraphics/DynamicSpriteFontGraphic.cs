using Engine.Core;
using Engine.Extensions;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.IO;

namespace Engine.FontGraphics
{
    public class DynamicSpriteFontGraphic : FontGraphic
    {
        private byte[] trueTypeFont;
        public byte[] TrueTypeFont
        {
            get
            {
                return trueTypeFont;
            }
            set
            {
                trueTypeFont = value;
                fontSystem = new FontSystem();
                fontSystem.AddFont(trueTypeFont);
            }
        }
        public Color OutlineColor { get; set; } = Color.Transparent;
        public int OutlineThickness { get; set; } = 0;
        public int FontSize { get; set; }
        private FontSystem fontSystem;


        public DynamicSpriteFontGraphic(string text, byte[] trueTypeFont, int fontSize, Vector2 position, Color color) : base(text, position, color)
        {
            TrueTypeFont = trueTypeFont;
            FontSize = fontSize;
        }

        public override void Draw(GraphicsHandler graphicsHandler)
        {
            if (!OutlineColor.Equals(Color) && !OutlineColor.Equals(Color.Transparent))
            {
                graphicsHandler.DrawStringWithOutline(fontSystem.GetFont(FontSize), Text, new Vector2(Position.X.Round(), Position.Y.Round()), Color, OutlineColor, OutlineThickness);
            }
            else
            {
                graphicsHandler.DrawString(fontSystem.GetFont(FontSize), Text, new Vector2(Position.X.Round(), Position.Y.Round()), Color);
            }
        }

        public void DrawWithParsedNewLines(GraphicsHandler graphicsHandler, int gapBetweenLines)
        {
            int drawLocationY = Position.Y.Round();
            foreach (string line in Text.Split("\n"))
            {
                graphicsHandler.DrawString(fontSystem.GetFont(FontSize), line, new Vector2(Position.X, drawLocationY), Color);
                drawLocationY += fontSystem.GetFont(FontSize).LineHeight + gapBetweenLines;
            }
        }
    }
}
