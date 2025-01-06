using MapEditor.src.MapTileEditor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Data.Common;
using MapEditor.Utils;
using static MapEditor.Models.TilesetDataFile;
using MapEditor.Models;

namespace MapEditor.src.Models
{
    public class Tileset
    {
        public string TilesetFilePath { get; set; }
        public string TilesetImageFilePath { get; private set; }
        public Tile[] Tiles { get; private set; }
        public Bitmap TilesetImage { get; private set; }
        public int TilesetImageWidth { get; private set; }
        public int TilesetImageHeight { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public int TileScale { get; set; }
        public int TilesetScaledWidth
        {
            get
            {
                return TileWidth * TileScale;
            }
        }
        public int TilesetScaledHeight
        {
            get
            {
                return TileHeight * TileScale;
            }
        }
        private int numberOfRows;
        private int numberOfColumns;
        public int NumberOfTiles { get; private set; }


        public string Name
        {
            get
            {
                return Path.GetFileNameWithoutExtension(TilesetFilePath);
            }
        }

        public Tileset(string tilesetFilePath)
        {
            TilesetFilePath = tilesetFilePath;
            LoadTileset();
        }

        public static TilesetDataFile ReadTilesetDataFile(string tilesetDataFilePath)
        {
            string json = File.ReadAllText(tilesetDataFilePath);
            return JsonSerializer.Deserialize<TilesetDataFile>(json);
        }

        public void LoadTileset()
        {
            TilesetDataFile tilesetData = Tileset.ReadTilesetDataFile(TilesetFilePath);

            TilesetImageFilePath = $"{Config.GraphicsPath}/{tilesetData.Properties.TilesetImagePath}";
            TilesetImage = new Bitmap(TilesetImageFilePath);
            TileWidth = tilesetData.Properties.TileWidth;
            TileHeight = tilesetData.Properties.TileHeight;
            TileScale = tilesetData.Properties.TileScale;

            TilesetImageWidth = TilesetImage.Width;
            TilesetImageHeight = TilesetImage.Height;
            numberOfRows = TilesetImageHeight / TileHeight;
            numberOfColumns = TilesetImageWidth / TileWidth;

            NumberOfTiles = tilesetData.Tiles.Count;
            Tiles = new Tile[NumberOfTiles];

            for (int i = 0; i < tilesetData.Tiles.Count; i++)
            {
                TileData tileData = tilesetData.Tiles[i];
                Bitmap finalTileImage = null;
                foreach (LayerData layerData in tileData.Layers)
                {
                    FrameData firstFrameOfLayer = layerData.Frames[0];
                    Bitmap layerImage = GetTileSubImage(firstFrameOfLayer.Row, firstFrameOfLayer.Column);
                    layerImage = ImageUtils.MakeColorTransparent(layerImage, Color.Magenta);
                    if (firstFrameOfLayer.SpriteEffect != null)
                    {
                        switch (firstFrameOfLayer.SpriteEffect)
                        {
                            case "FLIP_HORIZONTALLY":
                                layerImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                break;
                            case "FLIP VERTICALLY":
                                layerImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                break;
                        }
                    }
                    if (finalTileImage == null)
                    {
                        finalTileImage = layerImage;
                    }
                    else
                    {
                        using (Graphics g = Graphics.FromImage(finalTileImage))
                        {
                            g.DrawImage(layerImage, new Point(0, 0));
                        }
                    }
                }
                Tiles[i] = new Tile(i, finalTileImage);
            }
        }

        public Bitmap GetTileSubImage(int row, int column)
        {
            Rectangle tileRectangle = new Rectangle(column * TileWidth + column, row * TileHeight + row, TileWidth, TileHeight);
            return TilesetImage.Clone(tileRectangle, TilesetImage.PixelFormat);
        }

       
        /*
        public void SaveTileset()
        {
            try
            {
                StreamWriter sw = new StreamWriter(TilesetFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error writing map to file:\n" + e.StackTrace);
            }
        }
        */
    }
}
