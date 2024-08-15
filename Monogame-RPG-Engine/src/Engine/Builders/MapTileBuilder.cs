using Engine.Entity;
using Engine.Scene;
using System;
using System.Collections.Generic;
using System.Text;

// Builder class to instantiate a MapTile class
namespace Engine.Builders
{
    public class MapTileBuilder
    {
        private TileType tileType = TileType.PASSABLE;
        private int tileIndex = -1;
        private Dictionary<string, Frame[]> bottomLayer = new Dictionary<string, Frame[]>();
        private Dictionary<string, Frame[]> topLayer = new Dictionary<string, Frame[]>();


        public MapTileBuilder(Frame bottomLayer) 
        {
            this.bottomLayer.Add("DEFAULT", new Frame[] { bottomLayer });
        }

        public MapTileBuilder(Frame[] bottomLayer)
        {
            this.bottomLayer.Add("DEFAULT", bottomLayer);
        }

        public MapTileBuilder WithTileType(TileType tileType)
        {
            this.tileType = tileType;
            return this;
        }

        public MapTileBuilder WithTileIndex(int tileIndex)
        {
            this.tileIndex = tileIndex;
            return this;
        }

        public MapTileBuilder WithBottomLayer(Frame bottomLayer)
        {
            this.bottomLayer.Add("DEFAULT", new Frame[] { bottomLayer });
            return this;
        }

        public MapTileBuilder WithBottomLayer(Frame[] bottomLayer)
        {
            this.bottomLayer.Add("DEFAULT", bottomLayer);
            return this;
        }

        public MapTileBuilder WithTopLayer(Frame topLayer)
        {
            this.topLayer.Add("DEFAULT", new Frame[] { topLayer });
            return this;
        }

        public MapTileBuilder WithTopLayer(Frame[] topLayer)
        {
            this.topLayer.Add("DEFAULT", topLayer);
            return this;
        }

        private Dictionary<string, Frame[]> CloneAnimations(Dictionary<string, Frame[]> animations)
        {
            Dictionary<string, Frame[]> animationsCopy = new Dictionary<string, Frame[]>();
            foreach (string key in animations.Keys)
            {
                Frame[] frames = animations[key];
                Frame[] framesCopy = new Frame[frames.Length];
                for (int i = 0; i < framesCopy.Length; i++)
                {
                    framesCopy[i] = frames[i].Copy();
                }
                animationsCopy.Add(key, framesCopy);
            }
            return animationsCopy;
        }

        public MapTile Build(float x, float y)
        {
            GameObject bottomLayerAnimation = new GameObject(x, y, CloneAnimations(bottomLayer), "DEFAULT");
            GameObject topLayerAnimation = null;
            if (topLayer.Count > 0)
            {
                topLayerAnimation = new GameObject(x, y, CloneAnimations(topLayer), "DEFAULT");
            }

            return new MapTile(x, y, bottomLayerAnimation, topLayerAnimation, tileType, tileIndex);
        }
    }
}
