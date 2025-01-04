using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MapEditor.src.MapTileEditor;
using MapEditor.src.ExtensionMethods;
using MapEditor.src.Models;

namespace MapEditor.src.MapTilePicker
{
    public partial class TilePicker : ObservableUserControl<TilePickerListener>, TileEditorListener
    {
        private Tileset tileset;
        public bool TilePickerRepaint { get; set; }
        private Tile selectedTile = null;

        public TilePicker()
        {
            InitializeComponent();
        }

        public void SetupTilePicker()
        {
            tilePickerPictureBox.Size = new Size(0, 0);

            // number of columns and rows needed to fit all tiles based on size of parent panel's width number of tiles
            // the smaller the width of the parent panel, the less columns and more rows that will be needed
            int tileSpacing = tileset.TileScale + 2;
            int numberOfColumns = Math.Max((tilePickerPanel.ClientSize.Width - tileSpacing) / (tileset.TilesetScaledWidth + tileSpacing), 1);
            int numberOfRows = (int)Math.Ceiling(tileset.NumberOfTiles / (float)numberOfColumns);

            tilePickerPictureBox.Location = new Point(0, 0);
            tilePickerPictureBox.Image = new Bitmap(tilePickerPanel.ClientSize.Width, numberOfRows * tileset.TilesetScaledHeight + (numberOfRows * tileSpacing) + tileSpacing);
            tilePickerPictureBox.Size = new Size(numberOfColumns * tileset.TilesetScaledWidth + (numberOfColumns * tileSpacing) + tileSpacing, tilePickerPictureBox.Image.Height);
            
            // set location of tiles in tile picker
            for (int i = 0; i < tileset.Tiles.Length; i++)
            {
                Tile tile = tileset.Tiles[i];
                int row = i / numberOfColumns;
                int column = i % numberOfColumns;
                tile.SetLocation(column * tileset.TilesetScaledWidth + (column * tileSpacing) + tileSpacing, row * tileset.TilesetScaledHeight + (row * tileSpacing) + tileSpacing);
                tile.SetDimensions(tileset.TilesetScaledWidth, tileset.TilesetScaledHeight);
            }

            // hacky in order to get winforms panel to stop creating a stupid horizontal scroll bar when there is no need to.
            // the picturebox is always set to be the same width as the parent panel's width, so there should never be a situation where the horizontal scroll bar is needed.
            // somehow there is a random off by 1 pixel error that I'm pretty sure is a bug in winforms and not my fault, which makes the picturebox extend 1 pixel past the parent panel's width and pops up the horizontal scroll bar.
            // so this just removes one pixel from the picturebox if the horizontal scroll bar is showing.
            if (tilePickerPanel.HorizontalScroll.Visible)
            {
                tilePickerPictureBox.Size = new Size(tilePickerPictureBox.Width - 1, tilePickerPictureBox.Height);
            }
        }

        private void tilePickerPictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (tileset != null)
            {
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                // paint tile picker tiles
                for (int i = 0; i < tileset.Tiles.Length; i++)
                {
                    Tile tile = tileset.Tiles[i];
                    tile.Paint(e.Graphics);
                }

                // paint yellow rectangle around selected tile
                if (selectedTile != null)
                {
                    int borderSize = tileset.TileScale + 2;
                    Pen pen = new Pen(Color.Yellow, borderSize);
                    e.Graphics.DrawRectangle(
                        pen,
                        new Rectangle(
                            selectedTile.X - borderSize / 2,
                            selectedTile.Y - borderSize / 2,
                            selectedTile.Width + borderSize,
                            selectedTile.Height + borderSize
                        )
                    );
                }
            }

        }

        private void tilePickerPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (tileset != null) { 
                foreach (Tile tile in tileset.Tiles)
                {
                    if (tile.IsPointInTile(e.Location))
                    {
                        Cursor = Cursors.Hand;
                        return;
                    }
                }
                Cursor = Cursors.Arrow;
            }
        }

        private void tilePickerPictureBox_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void tilePickerPanel_Resize(object sender, EventArgs e)
        {
            if (tileset != null)
            {
                SetupTilePicker();
                tilePickerPictureBox.Invalidate();
            }
        }

        private void tilePickerPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (tileset != null)
            {
                foreach (Tile tile in tileset.Tiles)
                {
                    if (tile.IsPointInTile(e.Location))
                    {
                        Cursor = Cursors.Hand;
                        selectedTile = tile;
                        return;
                    }
                }
                Cursor = Cursors.Arrow;
            }
        }

        private void tilePickerPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                foreach (Tile tile in tileset.Tiles)
                {
                    if (tile.IsPointInTile(e.Location))
                    {
                        selectedTile = tile;
                        tilePickerPictureBox.Invalidate();
                        
                        foreach (TilePickerListener listener in listeners)
                        {
                            listener.OnTileSelect(selectedTile);
                        }
                        return;
                    }
                }
            }
        }
        
        public void OnTileEditorLoad(Map map)
        {
            this.tileset = map.Tileset;
            SetupTilePicker();
            this.selectedTile = null;
            tilePickerPictureBox.Invalidate();
        }

        public void OnTileEdited(int index, Tile tile)
        {
            return;
        }
    }
}
