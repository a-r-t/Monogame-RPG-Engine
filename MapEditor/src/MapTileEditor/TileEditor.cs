using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MapEditor.src.MapTilePicker;
using MapEditor.src.ExtensionMethods;
using MapEditor.src.MapList;
using MapEditor.src.Models;

namespace MapEditor.src.MapTileEditor
{
    public partial class TileEditor : ObservableUserControl<TileEditorListener>, TilePickerListener
    {
        private Map map;
        public Map Map
        {
            get
            {
                return map;
            }
            set
            {
                map = value;
                LoadMap();
            }
        }

        private Point hoveredTileIndex = new Point(-1, -1);
        private Point previousHoveredTileIndex = new Point(-1, -1);
        private Tile selectedTile;
        private TilePicker tilePicker;
        private bool isPastEditLimitIndex = false;

        public TileEditor()
        {
            InitializeComponent();
            tilePicker = new TilePicker();
            tilePickerPanel.Controls.Add(tilePicker);
            tilePicker.Dock = DockStyle.Fill;

            tilePicker.AddListener(this);
            this.AddListener(tilePicker);
            mapPanel.DoubleBuffered(true);
            mapPanelScroll.DoubleBuffered(true);
        }

        private void MapBuilder_Load(object sender, EventArgs e)
        {
            mapPanel.ClientSize = new Size(0, 0);
            splitContainer1.SplitterDistance = 688;
        }

        private void mapPanel_Paint(object sender, PaintEventArgs e)
        {
            if (map != null)
            {
                int hScroll = mapPanelScroll.HorizontalScroll.Value / map.Tileset.TilesetScaledWidth;
                int vScroll = mapPanelScroll.VerticalScroll.Value / map.Tileset.TilesetScaledHeight;

                int w = mapPanelScroll.Size.Width / map.Tileset.TilesetScaledWidth;
                int h = mapPanelScroll.Size.Height / map.Tileset.TilesetScaledHeight;
                map.Paint(e.Graphics, hScroll - 1, vScroll - 1, w + 2, h + 2);

                if (hoveredTileIndex.X != -1 && hoveredTileIndex.Y != -1)
                {
                    int borderSize = map.Tileset.TileScale + 1;
                    Color penColor = !isPastEditLimitIndex ? Color.Yellow : Color.Red;
                    Pen pen = new Pen(penColor, borderSize);
                    e.Graphics.DrawRectangle(
                        pen,
                        new Rectangle(
                            (hoveredTileIndex.X * map.MapTileWidth) + (borderSize / 2.0f).Round(),
                            (hoveredTileIndex.Y * map.MapTileHeight) + (borderSize / 2.0f).Round(),
                            map.MapTileWidth - borderSize,
                            map.MapTileHeight - borderSize
                        )
                    );
                }
            }
        }

        private void mapPanel_MouseMove(object sender, MouseEventArgs e)
        {
            isPastEditLimitIndex = false;
            if (map != null)
            {
                previousHoveredTileIndex = hoveredTileIndex;
                int mouseCoordX = e.X;
                int mouseCoordY = e.Y;

                // there's a windows limitation where mouse coords are stored in a 16 bit signed int,
                // meaning after 32767, the mouse coordinates overflow to negative AND mouse clicks can no longer be detected
                // so this will prevent the user from attempting to go past that mouse coordinate, since they can't edit it anyway
                if (mouseCoordX < 0)
                {
                    mouseCoordX = 32767 + (32767 - Math.Abs(e.X));
                    isPastEditLimitIndex = true;
                }
                if (mouseCoordY < 0)
                {
                    mouseCoordY = 32767 + (32767 - Math.Abs(e.Y));
                    isPastEditLimitIndex = true;
                }
                hoveredTileIndex = map.GetTileIndexByPosition(mouseCoordX - map.MapTileWidth / 2, mouseCoordY - map.MapTileHeight / 2);
                selectedTileIndexLabel.Text = $"X: {hoveredTileIndex.X}, Y: {hoveredTileIndex.Y}";
                selectedTileIndexLabel.Location = new Point(heightLabel.Location.X + heightLabel.Width + 10, selectedTileIndexLabel.Location.Y);
                bool mapChanged = false;
                if (e.Button == MouseButtons.Left && !isPastEditLimitIndex)
                {
                    if (selectedTile != null)
                    {
                        Point selectedTileIndex = map.GetTileIndexByPosition(mouseCoordX - map.MapTileWidth / 2, mouseCoordY - map.MapTileHeight / 2);
                        int convertedTileIndex = map.GetConvertedIndex(selectedTileIndex.X, selectedTileIndex.Y);
                        if (convertedTileIndex >= 0 && convertedTileIndex < map.Width * map.Height)
                        {
                            Tile tileToReplace = map.GetMapTile(selectedTileIndex.X, selectedTileIndex.Y);
                            tileToReplace.Index = selectedTile.Index;
                            tileToReplace.Image = (Bitmap)selectedTile.Image.Clone();
                            mapChanged = true;
                        }
                    }
                }
                if (previousHoveredTileIndex != hoveredTileIndex || mapChanged)
                {
                    mapPanel.Invalidate();
                }
            }
        }

        private void mapPanel_MouseLeave(object sender, EventArgs e)
        {
            selectedTileIndexLabel.Visible = false;
            hoveredTileIndex = new Point(-1, -1);

            mapPanel.Invalidate();
        }

        private void mapPanel_MouseEnter(object sender, EventArgs e)
        {
            selectedTileIndexLabel.Visible = true;
        }

        public void OnTileSelect(Tile tile)
        {
            selectedTile = tile;
        }

        private void mapPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (selectedTile != null)
                {
                    Point selectedTileIndex = map.GetTileIndexByPosition(e.X - map.MapTileWidth / 2, e.Y - map.MapTileHeight / 2);
                    int convertedTileIndex = map.GetConvertedIndex(selectedTileIndex.X, selectedTileIndex.Y);

                    if (convertedTileIndex >= 0 && convertedTileIndex < map.Width * map.Height)
                    {
                        Tile tileToReplace = map.GetMapTile(selectedTileIndex.X, selectedTileIndex.Y);
                        tileToReplace.Index = selectedTile.Index;
                        tileToReplace.Image = selectedTile.Image;

                        mapPanel.Invalidate();

                        foreach (TileEditorListener listener in listeners)
                        {
                            listener.OnTileEdited(convertedTileIndex, tileToReplace);
                        }
                    }
                }
            }
        }

        private void mapPanel_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void LoadMap()
        {
            //mapPictureBox.Image = new Bitmap(map.WidthInPixels, map.HeightInPixels);
            //mapPictureBox.ClientSize = mapPictureBox.Image.Size;
            mapPanel.ClientSize = new Size(map.WidthInPixels, map.HeightInPixels);
            widthLabel.Text = $"Width: {map.Width}";
            heightLabel.Text = $"Height: {map.Height}";
            heightLabel.Location = new Point(widthLabel.Location.X + widthLabel.Width + 10, heightLabel.Location.Y);
            mapPanel.Invalidate();

            foreach (TileEditorListener listener in listeners)
            {
                listener.OnTileEditorLoad(map);
            }
        }
    }
}
