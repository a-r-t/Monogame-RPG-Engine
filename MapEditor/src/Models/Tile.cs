using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.src.Models
{
    public class Tile
    {
        public int Index { get; set; }
        public Bitmap Image { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Tile(int index, Bitmap image)
        {
            Index = index;
            Image = image;
        }

        public void SetLocation(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void SetDimensions(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void Paint(Graphics graphics)
        {
            graphics.DrawImage(Image, X, Y, Width, Height);
        }

        public bool IsPointInTile(Point point)
        {
            return point.X > X && point.X < X + Width && point.Y > Y && point.Y < Y + Height;
        }
    }
}
