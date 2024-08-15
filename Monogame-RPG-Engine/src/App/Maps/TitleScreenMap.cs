using App.Resources;
using App.Tilesets;
using Engine.Core;
using Engine.Entity;
using Engine.Scene;
using Engine.Utils;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Maps
{
    public class TitleScreenMap : Map
    {
        private Sprite cat;

        public TitleScreenMap(ContentLoader contentLoader)
            : base("title_screen_map.txt", new CommonTileset(contentLoader), contentLoader)
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
