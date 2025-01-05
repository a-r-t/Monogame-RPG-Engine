using Basic_Map_Editor.Models;
using Microsoft.VisualBasic.ApplicationServices;
using MonoGame.Extended.Input.InputListeners;
using Monogame_RPG_Engine.Engine.Core;
using Monogame_RPG_Engine.Engine.Scene.EntitiesCore;
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

namespace Basic_Map_Editor.Components
{
    public partial class TileBuilder : UserControl
    {
        private Map map;
        private MapTile hoveredMapTile;
        private SelectedTileIndexHolder controlPanelHolder;
        //private GraphicsHandler graphicsHandler = new GraphicsHandler();
        private Label hoveredTileIndexLabel;
        private bool showNPCs;
        private bool showEnhancedMapTiles;
        private bool showTriggers;
        private bool dragged;

        public TileBuilder(SelectedTileIndexHolder controlPanelHolder, Label hoveredTileIndexLabel)
        {
            InitializeComponent();

            BackColor = Color.Magenta;
            Location = new Point(0, 0);
            Size = new Size(585, 562);
            this.controlPanelHolder = controlPanelHolder;
            this.hoveredTileIndexLabel = hoveredTileIndexLabel;

            MouseLeave += (sender, e) =>
            {
                hoveredMapTile = null;
                hoveredTileIndexLabel.Text = "";
                Refresh();
            };

            MouseDown += (sender, e) =>
            {
                TileSelected(e.Location);
                dragged = true;
            };

            MouseMove += (sender, e) =>
            {
                TileHovered(e.Location);

                if (dragged)
                {
                    TileSelected(e.Location);
                }
            };

            MouseUp += (sender, e) =>
            {
                dragged = false;
            };

        }

        public void SetMap(Map map)
        {
            this.map = map;
            Size = new Size(map.WidthPixels, map.HeightPixels);
            Refresh();
        }

        public void Draw()
        {
            foreach (MapTile tile in map.MapTiles)
            {
                tile.Draw(graphicsHandler);
            }

            if (showEnhancedMapTiles)
            {
                foreach (EnhancedMapTile enhancedMapTile in map.EnhancedMapTiles)
                {
                    enhancedMapTile.Draw(graphicsHandler);
                }
            }

            if (showNPCs)
            {
                foreach (NPC npc in map.NPCs)
                {
                    npc.Draw(graphicsHandler);
                }
            }

            if (showTriggers)
            {
                foreach (Trigger trigger in map.Triggers)
                {
                    trigger.Draw(graphicsHandler, Color.FromArgb(100, 255, 0, 255));
                }
            }

            if (hoveredMapTile != null)
            {
                graphicsHandler.drawRectangle(
                        Math.round(hoveredMapTile.getX()) + 2,
                        Math.round(hoveredMapTile.getY()) + 2,
                        hoveredMapTile.getWidth() - 5,
                        hoveredMapTile.getHeight() - 5,
                        Color.YELLOW,
                        5
                );
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //graphicsHandler.setGraphics((Graphics2D)g);
            Draw();
        }

        public void TileSelected(Point selectedPoint)
        {
            int selectedTileIndex = GetSelectedTileIndex(selectedPoint);
            if (selectedTileIndex != -1)
            {
                MapTile oldMapTile = map.MapTiles[selectedTileIndex];
                MapTile newMapTile = map.Tileset.GetTile(controlPanelHolder.SelectedTileIndex).Build(oldMapTile.X, oldMapTile.Y);
                newMapTile.SetMap(map);
                map.MapTiles[selectedTileIndex] = newMapTile;

            }
            Refresh();
        }

        public void TileHovered(Point hoveredPoint)
        {
            this.hoveredMapTile = GetHoveredTile(hoveredPoint);
            if (this.hoveredMapTile != null)
            {
                int hoveredIndexX = (int)Math.Round(this.hoveredMapTile.X) / map.Tileset.SpriteWidthScaled;
                int hoveredIndexY = (int)Math.Round(this.hoveredMapTile.Y) / map.Tileset.SpriteHeightScaled;
                hoveredTileIndexLabel.Text = "X: " + hoveredIndexX + ", Y: " + hoveredIndexY;
                Refresh();
            }
        }

        protected MapTile GetHoveredTile(Point mousePoint)
        {
            foreach (MapTile mapTile in map.MapTiles)
            {
                if (IsPointInTile(mousePoint, mapTile))
                {
                    return mapTile;
                }
            }
            return null;
        }

        protected int GetSelectedTileIndex(Point mousePoint)
        {
            MapTile[] mapTiles = map.MapTiles;
            for (int i = 0; i < mapTiles.Length; i++)
            {
                if (IsPointInTile(mousePoint, mapTiles[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        protected bool IsPointInTile(Point point, MapTile tile)
        {
            return (point.X >= tile.X && point.X <= tile.X + tile.Width &&
                    point.Y >= tile.Y && point.Y <= tile.Y + tile.Height);
        }

        public bool GetShowNPCs()
        {
            return showNPCs;
        }

        public void SetShowNPCs(bool showNPCs)
        {
            this.showNPCs = showNPCs;
            Refresh();
        }

        public bool GetShowEnhancedMapTiles()
        {
            return showEnhancedMapTiles;
        }

        public void SetShowEnhancedMapTiles(bool showEnhancedMapTiles)
        {
            this.showEnhancedMapTiles = showEnhancedMapTiles;
            Refresh();
        }

        public bool GetShowTriggers()
        {
            return showTriggers;
        }

        public void SetShowTriggers(bool showTriggers)
        {
            this.showTriggers = showTriggers;
            Refresh();
        }
    }
}
