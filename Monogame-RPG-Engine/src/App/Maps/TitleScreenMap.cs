using Monogame_RPG_Engine.App.Resources;
using Monogame_RPG_Engine.App.Tilesets;
using Monogame_RPG_Engine.Engine.Core;
using Monogame_RPG_Engine.Engine.SpriteGraphics;
using Monogame_RPG_Engine.Engine.Scene;
using Monogame_RPG_Engine.Engine.Utils;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monogame_RPG_Engine.Engine.Scene.MapCore;

namespace Monogame_RPG_Engine.App.Maps
{
    public class TitleScreenMap : Map
    {
        private Sprite cat;

        public TitleScreenMap(int cameraWidth, int cameraHeight, ContentLoader contentLoader)
            : base("title_screen_map.txt", new CommonTileset(contentLoader), cameraWidth, cameraHeight, contentLoader)
        {
            Point catLocation = GetMapTile(8, 5).Location.SubtractX(6).SubtractY(7);
            cat = new Sprite(new SpriteSheet(contentLoader.LoadTexture(GraphicsHelper.CAT), 24, 24).GetSprite(0, 0));
            cat.Scale = 3;
            cat.SpriteEffect = SpriteEffects.FlipHorizontally;
            cat.SetLocation(catLocation.X, catLocation.Y);
        }

        public override void Draw(GraphicsHandler graphicsHandler)
        {
            base.Draw(graphicsHandler);
            cat.Draw(graphicsHandler);
        }
    }
}
