using Microsoft.Xna.Framework.Graphics;
using Engine.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

// This class is for reading in a SpriteSheet (collection of images laid out in a specific way)
// As long as each graphic on the sheet is the same size, it can parse it into sub images
namespace Engine.Entity
{
    public class SpriteSheet
    {
        public Texture2D Image { get; private set; }
        public int SpriteWidth { get; private set; }
        public int SpriteHeight { get; private set; }
        protected int rowLength;
        protected int columnLength;

        public SpriteSheet(Texture2D image, int spriteWidth, int spriteHeight)
        {
            Image = image;
            SpriteWidth = spriteWidth;
            SpriteHeight = spriteHeight;
            rowLength = image.Height / spriteHeight;
            columnLength = image.Width / spriteWidth;
        }

        // returns a subimage from the sprite sheet image based on the row and column
        public Texture2D GetSprite(int spriteNumber, int animationNumber)
        {
            return Image.Crop(new Microsoft.Xna.Framework.Rectangle(animationNumber * SpriteWidth + animationNumber, spriteNumber * SpriteHeight + spriteNumber, SpriteWidth, SpriteHeight));
        }

        // returns a subimage from the sprite sheet image based on the row and column
        // this does the same as "getSprite", I added two methods that do the same thing for some reason
        public Texture2D GetSubImage(int row, int column)
        {
            return Image.Crop(new Microsoft.Xna.Framework.Rectangle(column * SpriteWidth + column, row * SpriteHeight + row, SpriteWidth, SpriteHeight));
        }


    }
}
