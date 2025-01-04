using MapEditor.src.ExtensionMethods;
using MapEditor.src.MapTileEditor;
using MapEditor.src.MapTilePicker;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.src.Models
{
    public class Map
    {
        public Tile[] MapTiles { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string MapFilePath { get; private set; }
        
        public int WidthInPixels
        {
            get
            {
                return Width * Tileset.TilesetScaledWidth;
            }
        }

        public int HeightInPixels
        {
            get
            {
                return Height * Tileset.TilesetScaledHeight;
            }
        }

        public Tileset Tileset { get; private set; }

        public int MapTileWidth
        {
            get
            {
                return Tileset.TilesetScaledWidth;
            }
        }

        public int MapTileHeight
        {
            get
            {
                return Tileset.TilesetScaledHeight;
            }
        }

        public Map(string mapFilePath)
        {
            MapFilePath = mapFilePath;
            LoadMap();
        }

        public void LoadMap()
        {
            using (StreamReader sr = File.OpenText(MapFilePath))
            {
                string[] dimensions = sr.ReadLine().Split(' ');
                Width = int.Parse(dimensions[0]);
                Height = int.Parse(dimensions[1]);

                string[] tilesetInfo = sr.ReadLine().Split(' ');
                string tilesetName = tilesetInfo[0];
                int mapTileScale = int.Parse(tilesetInfo[1]);

                this.Tileset = new Tileset($"./Resources/TilesetFiles/{tilesetName}.tileset", mapTileScale);

                MapTiles = new Tile[Width * Height];
                string indexes = "";
                int heightCounter = 0;
                while ((indexes = sr.ReadLine()) != null)
                {
                    string[] mapTileIndexes = indexes.Split(' ');
                    for (int i = 0; i < mapTileIndexes.Length; i++)
                    {
                        int tileIndex = int.Parse(mapTileIndexes[i]);
                        int row = heightCounter;
                        int column = i % Width;
                        int tileX = column * Tileset.TilesetScaledWidth;
                        int tileY = row * Tileset.TilesetScaledHeight;
                        int tileWidth = Tileset.TilesetScaledWidth;
                        int tileHeight = Tileset.TilesetScaledHeight;

                        Tile tile = new Tile(tileIndex, Tileset.GetTileSubImage(tileIndex));
                        tile.SetLocation(tileX, tileY);
                        tile.SetDimensions(tileWidth, tileHeight);
                        SetMapTile(i, heightCounter, tile);
                    }
                    heightCounter++;
                }
            }
            PrintMap();
        }


        public int GetConvertedIndex(int x, int y)
        {
            return x + Width * y;
        }

        public Tile GetMapTile(int x, int y)
        {
            return MapTiles[GetConvertedIndex(x, y)];
        }

        public Point GetTileIndexByPosition(float xPosition, float yPosition)
        {
            int xIndex = (xPosition / (float)Tileset.TilesetScaledWidth).Round();
            int yIndex = (yPosition / (float)Tileset.TilesetScaledHeight).Round();
            return new Point(xIndex, yIndex);
        }

        public void SetMapTile(int x, int y, Tile tile)
        {
            MapTiles[GetConvertedIndex(x, y)] = tile;
        }

        public bool IsInBounds(int x, int y)
        {
            int convertedIndex = GetConvertedIndex(x, y);
            return convertedIndex >= 0 && convertedIndex < MapTiles.Length;
        }

        public void PrintMap()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Console.Write($"{GetMapTile(j, i)} ");
                }
                Console.Write("\n");
            }
        }

        public void ReloadMapTiles()
        {
            int heightCounter = 0;
            for (int i = 0; i < MapTiles.Length; i++)
            {
                int tileIndex = MapTiles[i].Index;
                int column = i % Width;
                if (i != 0 && i % Width == 0)
                {
                    heightCounter++;
                }
                int row = heightCounter;

                int tileX = column * Tileset.TilesetScaledWidth;
                int tileY = row * Tileset.TilesetScaledHeight;
                int tileWidth = Tileset.TilesetScaledWidth;
                int tileHeight = Tileset.TilesetScaledHeight;

                Tile tile = new Tile(tileIndex, Tileset.GetTileSubImage(tileIndex));
                tile.SetLocation(tileX, tileY);
                tile.SetDimensions(tileWidth, tileHeight);
                MapTiles[i] = tile;
            }
        }

        public void Paint(Graphics graphics)
        {    
            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            
            for (int i = 0; i < MapTiles.Length; i++)
            {
                Tile tile = MapTiles[i];
                tile.Paint(graphics);
            }
        }

        public void Paint(Graphics graphics, int x, int y, int width, int height)
        {
            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            int startX = Math.Max(x, 0);
            int startY = Math.Max(y, 0);
            int endX = startX + width;
            int endY = startY + height;
            for (int i = startY; i <= endY; i++)
            {
                for (int j = startX; j <= endX; j++)
                {
                    if (IsInBounds(j, i))
                    {
                        Tile tile = MapTiles[GetConvertedIndex(j, i)];
                        tile.Paint(graphics);
                    }
                }
            }
        }

        public void SaveMap()
        {
            try
            {
                StreamWriter sw = new StreamWriter(MapFilePath);
                sw.WriteLine($"{Width} {Height}");
                sw.WriteLine($"{Tileset.Name} {Tileset.TileScale}");
                for (int i = 0; i < Height; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int j = 0; j < Width; j++)
                    {
                        int mapTileIndex = GetConvertedIndex(j, i);
                        sb.Append(MapTiles[mapTileIndex].Index);
                        if (j != Width - 1)
                        {
                            sb.Append(" ");
                        }
                    }

                    if (i < Height - 1)
                    {
                        sb.Append("\n");
                    }

                    sw.Write(sb.ToString());
                }
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error writing map to file:\n" + e.StackTrace);
            }
        }

        public static void CreateNewMapFile(string filePath)
        {
            try
            {
                StreamWriter sw = new StreamWriter(filePath);
                sw.WriteLine($"0 0");
                sw.WriteLine($"UnknownTileset 1");
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error creating new map:\n" + e.StackTrace);
            }
        }
    }
}
