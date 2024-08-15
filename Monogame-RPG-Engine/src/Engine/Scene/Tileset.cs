using Engine.Builders;
using Engine.Core;
using Engine.Extensions;
using Engine.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Engine.Utils;
using MonoGame.Extended.Collections;

// This class represents a tileset, which defines a set of tiles based on a sprite sheet image
namespace Engine.Scene
{
    public abstract class Tileset : SpriteSheet
    {
        // global scale of all tiles in the tileset
        public float TileScale { get; private set; } = 1f;

        // stores tiles mapped to an index
        protected Dictionary<int, MapTileBuilder> tiles;

        // default tile defined for situations where no tile information for an index can be found (failsafe basically)
        protected MapTileBuilder defaultTile;

        public int SpriteWidthScaled
        {
            get
            {
                return SpriteWidth * TileScale.Round();
            }
        }

        public int SpriteHeightScaled
        {
            get
            {
                return SpriteHeight * TileScale.Round();
            }
        }

        public Tileset(Texture2D image, int tileWidth, int tileHeight)
        : base(image, tileWidth, tileHeight)
        {
            this.tiles = MapDefinedTilesToIndex();
            this.defaultTile = GetDefaultTile();
        }

        public Tileset(Texture2D image, int tileWidth, int tileHeight, int tileScale)
            : base(image, tileWidth, tileHeight)
        {
            TileScale = tileScale;
            this.tiles = MapDefinedTilesToIndex();
            this.defaultTile = GetDefaultTile();
        }

        // a subclass of this class must implement this method to define tiles in the tileset
        public abstract List<MapTileBuilder> DefineTiles();

        // get specific tile from tileset by index, if not found the default tile is returned
        public MapTileBuilder GetTile(int tileNumber)
        {
            return tiles.GetValueOrDefault(tileNumber, GetDefaultTile());
        }

        // maps all tiles to a tile index, which is how it is identified by the map file
        public Dictionary<int, MapTileBuilder> MapDefinedTilesToIndex()
        {
            List<MapTileBuilder> mapTileBuilders = DefineTiles();
            Dictionary<int, MapTileBuilder> tilesToIndex = new Dictionary<int, MapTileBuilder>();
            for (int i = 0; i < mapTileBuilders.Count; i++)
            {
                tilesToIndex.Add(i, mapTileBuilders[i].WithTileIndex(i));
            }
            return tilesToIndex;
        }

        public MapTileBuilder GetDefaultTile()
        {
            return new MapTileBuilder(new FrameBuilder(ImageUtils.CreateSolidImage(Color.Black, SpriteWidth, SpriteHeight), 0).WithScale(TileScale).Build());
        }
    }
}
