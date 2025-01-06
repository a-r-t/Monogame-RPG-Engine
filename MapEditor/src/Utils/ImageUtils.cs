using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Utils
{
    public static class ImageUtils
    {
        public static Bitmap MakeColorTransparent(Bitmap bitmap, Color color)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    if (pixelColor.A == color.A && pixelColor.R == color.R && pixelColor.G == color.G && pixelColor.B == color.B)
                    {
                        bitmap.SetPixel(x, y, Color.Transparent);
                    }
                }
            }
            return bitmap;
        }
    }
}
