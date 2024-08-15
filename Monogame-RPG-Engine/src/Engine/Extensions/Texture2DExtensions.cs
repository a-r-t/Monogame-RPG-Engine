using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Extensions
{
    public static class Texture2DExtensions
    {
        public static Texture2D Crop(this Texture2D image, Rectangle source)
        {
            var graphics = image.GraphicsDevice;
            var ret = new RenderTarget2D(graphics, source.Width, source.Height);
            var sb = new SpriteBatch(graphics);

            graphics.SetRenderTarget(ret); // draw to image
            graphics.Clear(new Color(0, 0, 0, 0));

            sb.Begin();
            sb.Draw(image, Vector2.Zero, source, Color.White);
            sb.End();

            graphics.SetRenderTarget(null); // set back to main window

            return (Texture2D)ret;
        }
    }
}
