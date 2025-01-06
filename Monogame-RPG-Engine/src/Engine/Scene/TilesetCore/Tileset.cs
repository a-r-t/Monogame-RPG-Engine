using Engine.Builders;
using Engine.Core;
using Engine.Extensions;
using Engine.SpriteGraphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Engine.Utils;
using MonoGame.Extended.Collections;
using src.Engine.Scene.TilesetCore;
using Engine.Scene.MapCore;
using System.Text.Json;
using System.IO;

// This class represents a tileset, which defines a set of tiles based on a sprite sheet image
namespace Engine.Scene.TilesetCore
{
    public class Tileset : SpriteSheet
    {
        // global scale of all tiles in the tileset
        public float TileScale { get; private set; } = 1f;

        // stores tiles mapped to an index
        protected Dictionary<int, MapTileBuilder> tiles;

        // default tile defined for situations where no tile information for an index can be found (failsafe basically)
        protected MapTileBuilder defaultTile;

        private TilesetDataFile tilesetDataFile;

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

        public Tileset(TilesetDataFile tilesetDataFile, ContentLoader contentLoader)
        : base(contentLoader.LoadTexture("Graphics/" + tilesetDataFile.Properties.TilesetImagePath), tilesetDataFile.Properties.TileWidth, tilesetDataFile.Properties.TileHeight)
        {
            this.tilesetDataFile = tilesetDataFile;
            TileScale = tilesetDataFile.Properties.TileScale;
            this.tiles = MapDefinedTilesToIndex();
            this.defaultTile = GetDefaultTile();
        }

        // a subclass of this class must implement this method to define tiles in the tileset
        public virtual List<MapTileBuilder> DefineTiles()
        {
            List<MapTileBuilder> mapTiles = new List<MapTileBuilder>();
            foreach (TilesetDataFile.TileData tileData in tilesetDataFile.Tiles)
            {
                if (tileData.Layers.Count == 0)
                {
                    throw new Exception($"Tile {tileData.Name} has no layers defined (needs at least 1)");
                }
                List<Frame> bottomLayerFrames = new List<Frame>();
                List<Frame> topLayerFrames = new List<Frame>();
                for (int i = 0; i < 2; i++)
                {

                    if (i >= tileData.Layers.Count)
                    {
                        break;
                    }
                    TilesetDataFile.LayerData layerData = tileData.Layers[i];
                    foreach (TilesetDataFile.FrameData frameData in layerData.Frames)
                    {
                        Frame frame = new Frame(GetSubImage(frameData.Row, frameData.Column));
                        frame.Scale = TileScale;
                        if (layerData.Bounds != null)
                        {
                            frame.Bounds = new SpriteGraphics.Rectangle(layerData.Bounds.X, layerData.Bounds.Y, layerData.Bounds.Width, layerData.Bounds.Height);
                        }
                        if (frameData.Delay.HasValue)
                        {
                            frame.Delay = frameData.Delay.Value;
                        }
                        if (frameData.SpriteEffect != null)
                        {
                            switch (frameData.SpriteEffect)
                            {
                                case "FLIP_HORIZONTALLY":
                                    frame.SpriteEffect = SpriteEffects.FlipHorizontally;
                                    break;
                                case "FLIP_VERTICALLY":
                                    frame.SpriteEffect = SpriteEffects.FlipVertically;
                                    break;
                            }
                        }
                        if (i == 0)
                        {
                            bottomLayerFrames.Add(frame);
                        }
                        else if (i == 1)
                        {
                            topLayerFrames.Add(frame);
                        }
                    }

                }


                MapTileBuilder mapTile = new MapTileBuilder(bottomLayerFrames.ToArray());
                if (topLayerFrames.Count > 0)
                {
                    mapTile = mapTile.WithTopLayer(topLayerFrames.ToArray());
                }
                if (tileData.TileType != null)
                {
                    switch(tileData.TileType)
                    {
                        case "PASSABLE":
                            mapTile = mapTile.WithTileType(TileType.PASSABLE);
                            break;
                        case "NOT_PASSABLE":
                            mapTile = mapTile.WithTileType(TileType.NOT_PASSABLE);
                            break;
                    }
                }
                mapTiles.Add(mapTile);
            }

            return mapTiles;
        }

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

        public static TilesetDataFile ReadTilesetDataFile(string tilesetDataFilePath)
        {
            string json = File.ReadAllText(tilesetDataFilePath);
            return JsonSerializer.Deserialize<TilesetDataFile>(json);
        }
    }
}
