using Engine.Core;
using Engine.Entity;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

// This class is a base class for all enhanced map tiles in the game -- all enhanced map tiles should extend from it
namespace Engine.Scene
{
    public class EnhancedMapTile : MapTile
    {
        public EnhancedMapTile(float x, float y, GameObject bottomLayer, GameObject topLayer, TileType tileType)
            : base(x, y, bottomLayer, topLayer, tileType)
        {
        }

        public EnhancedMapTile(float x, float y, SpriteSheet spriteSheet, TileType tileType)
            : base(x, y, spriteSheet, tileType)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }


        public virtual void Update(Player player)
        {
            base.Update();
        }

        public override void Draw(GraphicsHandler graphicsHandler)
        {
            base.Draw(graphicsHandler);
        }
    }
}
