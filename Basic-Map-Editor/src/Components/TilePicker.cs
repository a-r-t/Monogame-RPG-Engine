using Basic_Map_Editor.Models;
using Microsoft.VisualBasic.ApplicationServices;
using Monogame_RPG_Engine.Engine.Builders;
using Monogame_RPG_Engine.Engine.Core;
using Monogame_RPG_Engine.Engine.Scene.MapCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Basic_Map_Editor.Components
{
    public partial class TilePicker : UserControl
    {
        private Tileset tileset;
        private Dictionary<int, MapTile> mapTiles = new Dictionary<int, MapTile>();
        private int selectedTileIndex = 0;
        private SelectedTileIndexHolder selectedTileIndexHolder;

        public TilePicker(SelectedTileIndexHolder selectedTileIndexHolder)
        {
            InitializeComponent();

            BackColor = Color.Magenta;
            Location = new Point(0, 0);
            Size = new Size(187, 391);
            BorderStyle = BorderStyle.FixedSingle;
            
            //setBorder(BorderFactory.createLineBorder(Color.black, 2));
            this.selectedTileIndexHolder = selectedTileIndexHolder;

            MouseDown += (sender, e) =>
            {
                TileSelected(e.Location);
            };

            MouseMove += (sender, e) =>
            {
                TileHovered(e.Location);
            };

        }

        public void SetTileset(Map map, Tileset tileset)
        {
            mapTiles.Clear();
            this.tileset = tileset;
            Dictionary<int, MapTileBuilder> mapTileBuilders = this.tileset.MapDefinedTilesToIndex();

            int width = (int)this.Width / this.tileset.SpriteWidthScaled;
            if (width == 0)
            {
                width = 1;
            }
            int height = (int)Math.Ceiling(mapTileBuilders.Keys.Count / (double)width);
            if (height == 0)
            {
                height = 1;
            }
            Size = new Size(Math.Max(144, width * tileset.SpriteWidthScaled), Math.Max(391, height * tileset.SpriteHeightScaled + (6 * height)));

            int[] tileKeys = mapTileBuilders.Keys.ToArray();
            Array.Sort(tileKeys);
            int currentKeyIndex = 0;

            bool breakOuterLoop = false;
            for (int i = 0; i < height; i++)
            {

                for (int j = 0; j < width; j++)
                {

                    if (currentKeyIndex >= tileKeys.Length)
                    {
                        breakOuterLoop = true;
                        break;
                    }

                    int x = j * tileset.SpriteWidthScaled + ((j * 5) + 5);
                    int y = i * tileset.SpriteHeightScaled + ((i * 5) + 5);
                    MapTile tile = mapTileBuilders[tileKeys[currentKeyIndex]].Build(x, y);
                    tile.SetMap(map);
                    mapTiles.Add(currentKeyIndex, tile);
                    currentKeyIndex++;
                }

                if (breakOuterLoop)
                {
                    break;
                }
            }
            Refresh();
        }

        public void Draw()
        {
            foreach (MapTile mapTile in mapTiles.Values)
            {
                mapTile.Draw(graphicsHandler);
            }

            MapTile selectedTile = mapTiles[selectedTileIndex];
            if (selectedTile == null)
            {
                selectedTile = mapTiles[0];
            }
            graphicsHandler.drawRectangle(
                    Math.round(selectedTile.getX()) - 2,
                    Math.round(selectedTile.getY()) - 2,
                    selectedTile.getWidth() + 4,
                    selectedTile.getHeight() + 4,
                    Color.YELLOW,
            4
            );
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); // Ensures the base class does its painting

            // Custom drawing logic
            graphicsHandler.setGraphics((Graphics2D)g);
            Draw();
        }

        protected void TileSelected(Point clickedPoint)
        {
            int selectedTileIndex = GetClickedTileIndex(clickedPoint);
            if (selectedTileIndex >= 0)
            {
                this.selectedTileIndex = selectedTileIndex;
                selectedTileIndexHolder.SelectedTileIndex = selectedTileIndex;
                Refresh();
            }
        }

        protected void TileHovered(Point hoveredPoint)
        {
            if (IsMouseInTileBounds(hoveredPoint))
            {
                Cursor = Cursors.Hand;
            }
            else
            {
                Cursor = Cursors.Default;
            }
        }

        protected bool IsMouseInTileBounds(Point mousePoint)
        {
            foreach (MapTile mapTile in mapTiles.Values)
            {
                if (IsPointInTile(mousePoint, mapTile))
                {
                    return true;
                }
            }
            return false;
        }

        protected int GetClickedTileIndex(Point mousePoint)
        {
            foreach (KeyValuePair<int, MapTile> entry in mapTiles)
            {
                if (IsPointInTile(mousePoint, entry.Value))
                {
                    return entry.Key;
                }
            }
            return -1;
        }

        protected bool IsPointInTile(Point point, MapTile tile)
        {
            return (point.X >= tile.X && point.X <= tile.X + tile.Width &&
                    point.Y >= tile.Y && point.Y <= tile.Y + tile.Height);
        }
    }
}
