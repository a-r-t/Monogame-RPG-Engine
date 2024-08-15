using Engine.Entity;
using Engine.Core;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

// Represents a map tile in a Map's tile map
namespace Engine.Scene
{
    public class MapTile : MapEntity
    {
        // this determines a tile's properties, like if it's passable or not
        public TileType TileType { get; private set; }

        // bottom layer of tile
        public GameObject BottomLayer { get; set; } = new GameObject(0, 0);

        // top layer of tile ("pasted on top of" bottom layer, covers player)
        public GameObject TopLayer { get; set; }

        public int TileIndex { get; private set; }

        public bool IsAnimated
        {
            get
            {
                return (BottomLayer.CurrentAnimation.Length > 1) ||
                    (TopLayer != null && TopLayer.CurrentAnimation.Length > 1);
            }
        }

        public override float X
        {
            get
            {
                return BottomLayer.X;
            }
            set
            {
                BottomLayer.X = value;
                if (TopLayer != null)
                {
                    TopLayer.X = value;
                }
                base.X = value;
            }
        }

        public override float Y
        {
            get
            {
                return BottomLayer.Y;
            }
            set
            {
                BottomLayer.Y = value;
                if (TopLayer != null)
                {
                    TopLayer.Y = value;
                }
                base.Y = value;
            }
        }

        public override float X1
        {
            get
            {
                return BottomLayer.X1;
            }
        }

        public override float Y1
        {
            get
            {
                return BottomLayer.Y1;
            }
        }

        public override float X2
        {
            get
            {
                return BottomLayer.X2;
            }
        }

        public override float Y2
        {
            get
            {
                return BottomLayer.Y2;
            }
        }


        public override Utils.Point Location
        {
            get
            {
                return BottomLayer.Location;
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public override float Scale
        {
            get
            {
                return BottomLayer.Scale;
            }
        }

        public override int Width
        {
            get
            {
                return BottomLayer.Width;
            }
        }

        public override int Height
        {
            get
            {
                return BottomLayer.Height;
            }
        }

        public override Entity.Rectangle Bounds
        {
            get
            {
                return BottomLayer.Bounds;
            }
        }

        public MapTile(float x, float y, GameObject bottomLayer, GameObject topLayer, TileType tileType, int tileIndex)
            : base(x, y)
        {
            BottomLayer = bottomLayer;
            TopLayer = topLayer;
            TileType = tileType;
            TileIndex = tileIndex;
        }

        public MapTile(float x, float y, GameObject bottomLayer, GameObject topLayer, TileType tileType)
            : base(x, y)
        {
            BottomLayer = bottomLayer;
            TopLayer = topLayer;
            TileType = tileType;
        }

        public MapTile(float x, float y, SpriteSheet spriteSheet, TileType tileType)
            : base(x, y)
        {
            BottomLayer = LoadBottomLayer(spriteSheet);
            TopLayer = LoadTopLayer(spriteSheet);
            TileType = tileType;
        }

        protected virtual GameObject LoadBottomLayer(SpriteSheet spriteSheet)
        {
            return null;
        }

        protected virtual GameObject LoadTopLayer(SpriteSheet spriteSheet)
        {
            return null;
        }

        // set this game object's map to make it a "part of" the map, allowing calibrated positions and collision handling logic to work
        // note that both the bottom layer and the top layer need the map reference
        public override void SetMap(Map map)
        {
            this.map = map;
            BottomLayer.SetMap(map);
            if (TopLayer != null)
            {
                TopLayer.SetMap(map);
            }
        }

        public override void MoveX(float dx)
        {
            X += dx;
            BottomLayer.MoveX(dx);
            if (TopLayer != null)
            {
                TopLayer.MoveX(dx);
            }
        }

        public override void MoveRight(float dx)
        {
            X += dx;
            BottomLayer.MoveRight(dx);
            if (TopLayer != null)
            {
                TopLayer.MoveRight(dx);
            }
        }

        public override void MoveLeft(float dx)
        {
            X -= dx;
            BottomLayer.MoveLeft(dx);
            if (TopLayer != null)
            {
                TopLayer.MoveLeft(dx);
            }
        }

        public override void MoveY(float dy)
        {
            Y += dy;
            BottomLayer.MoveY(dy);
            if (TopLayer != null)
            {
                TopLayer.MoveY(dy);
            }
        }

        public override void MoveDown(float dy)
        {
            Y += dy;
            BottomLayer.MoveDown(dy);
            if (TopLayer != null)
            {
                TopLayer.MoveDown(dy);
            }
        }

        public override void MoveUp(float dy)
        {
            Y -= dy;
            BottomLayer.MoveUp(dy);
            if (TopLayer != null)
            {
                TopLayer.MoveUp(dy);
            }
        }

        public override Entity.Rectangle GetIntersectRectangle()
        {
            return BottomLayer.GetIntersectRectangle();
        }

        public override bool Intersects(IntersectableRectangle other)
        {
            return BottomLayer.Intersects(other);
        }

        public override bool Touching(IntersectableRectangle other)
        {
            return BottomLayer.Touching(other);
        }

        public override void Update()
        {
            BottomLayer.Update();
            if (TopLayer != null)
            {
                TopLayer.Update();
            }
        }

        public override void Draw(GraphicsHandler graphicsHandler)
        {
            BottomLayer.Draw(graphicsHandler);
            if (TopLayer != null)
            {
                TopLayer.Draw(graphicsHandler);
            }
        }

        public void DrawBottomLayer(GraphicsHandler graphicsHandler)
        {
            BottomLayer.Draw(graphicsHandler);

            // uncomment this to draw bounds of all non passable tiles (useful for debugging)
            /*
            if (TileType == TileType.NOT_PASSABLE) {
                DrawBounds(graphicsHandler, new Color(0, 0, 255, 100));
            }
            */
        }

        public void DrawTopLayer(GraphicsHandler graphicsHandler)
        {
            TopLayer.Draw(graphicsHandler);

            // uncomment this to draw bounds of all non passable tiles (useful for debugging)
            /*
            if (TileType == TileType.NOT_PASSABLE) {
                DrawBounds(graphicsHandler, new Color(0, 0, 255, 100));
            }
            */
        }
    }
}
