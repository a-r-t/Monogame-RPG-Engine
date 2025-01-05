using Monogame_RPG_Engine.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_RPG_Engine.Engine.Utils
{
    public class ImageUtils
    {
        public static Texture2D CreateSolidImage(Color color, int width, int height)
        {
            return new Texture2D(GameLoop.GraphicsDeviceManager.GraphicsDevice, width, height);
        }
    }
}
