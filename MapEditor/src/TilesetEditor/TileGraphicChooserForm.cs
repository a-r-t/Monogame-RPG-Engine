using MapEditor.src.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEditor.src.TilesetEditor
{
    public partial class TileGraphicChooserForm : Form
    {
        private Tileset tileset;
        public Tileset Tileset
        {
            get
            {
                return tileset;
            }
            set
            {
                tileset = value;
                OnTilesetLoaded();
            }
        }
        private List<Tile> spriteSheetTiles;

        private (int Row, int Column) selectedTileLocation;
        public (int Row, int Column) SelectedTileLocation
        {
            get
            {
                return selectedTileLocation;
            }
            set
            {
                selectedTileLocation = value;
                tileGraphicPictureBox.Invalidate();
            }
        }

        private int NumberOfColumnsInTileset
        {
            get
            {
                return Tileset.TilesetImageWidth / Tileset.TileWidth;

            }
        }

        private Tile SelectedTile
        {
            get
            {
                return spriteSheetTiles[(SelectedTileLocation.Row * NumberOfColumnsInTileset) + SelectedTileLocation.Column];
            }
        }

        public TileGraphicChooserForm()
        {
            InitializeComponent();
            spriteSheetTiles = new List<Tile>();

            tileGraphicPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            tileGraphicPictureBox.BackColor = Config.TransparentColor;
            tileGraphicPanel.BackColor = Config.TransparentColor;

            tileGraphicPictureBox.Paint += (sender, e) =>
            {
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                if (tileset != null)
                {
                    for (int i = 0; i < spriteSheetTiles.Count; i++)
                    {
                        Tile tile = spriteSheetTiles[i];
                        tile.Paint(e.Graphics);
                    }

                    // paint yellow rectangle around selected tile
                    int borderSize = tileset.TileScale;
                    Pen pen = new Pen(Color.Yellow, borderSize);
                    e.Graphics.DrawRectangle(
                        pen,
                        new Rectangle(
                            SelectedTile.X - borderSize / 2,
                            SelectedTile.Y - borderSize / 2,
                            SelectedTile.Width + borderSize,
                            SelectedTile.Height + borderSize
                        )
                    );
                }
            };

            tileGraphicPictureBox.MouseMove += (sender, e) =>
            {
                if (Tileset != null)
                {
                    foreach (Tile tile in spriteSheetTiles)
                    {
                        if (tile.IsPointInTile(e.Location))
                        {
                            Cursor = Cursors.Hand;
                            return;
                        }
                    }
                    Cursor = Cursors.Default;
                }
            };

            tileGraphicPictureBox.MouseLeave += (sender, e) =>
            {
                Cursor = Cursors.Default;
            };

            tileGraphicPictureBox.MouseClick += (sender, e) =>
            {
                if (Tileset != null)
                {
                    for (int i = 0; i < spriteSheetTiles.Count; i++)
                    {
                        Tile tile = spriteSheetTiles[i];
                        if (tile.IsPointInTile(e.Location))
                        {
                            Cursor = Cursors.Hand;
                            SelectedTileLocation = (i / NumberOfColumnsInTileset, i % NumberOfColumnsInTileset);
                            tileGraphicPictureBox.Invalidate();
                            return;
                        }
                    }
                    Cursor = Cursors.Default;
                }
            };

            tileGraphicPictureBox.MouseDown += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (Tileset != null)
                    {
                        for (int i = 0; i < spriteSheetTiles.Count; i++)
                        {
                            Tile tile = spriteSheetTiles[i];
                            if (tile.IsPointInTile(e.Location))
                            {
                                Cursor = Cursors.Hand;
                                SelectedTileLocation = (i / NumberOfColumnsInTileset, i % NumberOfColumnsInTileset);
                                tileGraphicPictureBox.Invalidate();
                                return;
                            }
                        }
                        Cursor = Cursors.Default;
                    }
                }
            };

            tileGraphicPanel.Resize += (sender, e) =>
            {
                tileGraphicPictureBox.Location = new Point(Math.Max(tileGraphicPanel.ClientSize.Width / 2 - tileGraphicPictureBox.Width / 2, 0), Math.Max(tileGraphicPanel.ClientSize.Height / 2 - tileGraphicPictureBox.Height / 2, 0));
            };

            okButton.Click += (sender, e) =>
            {
                this.DialogResult = DialogResult.OK;
                Close();
            };
            cancelButton.Click += (sender, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                Close();
            };
        }

        private void OnTilesetLoaded()
        {
            spriteSheetTiles.Clear();

            int numberOfRows = Tileset.TilesetImageHeight / Tileset.TileHeight;
            int numberOfColumns = Tileset.TilesetImageWidth / Tileset.TileWidth;
            int numberOfTiles = numberOfRows * numberOfColumns;
            for (int i = 0; i < numberOfTiles; i++)
            {
                int row = i / numberOfColumns;
                int column = i % numberOfColumns;
                Rectangle tileSubImageRect = new Rectangle(column + (Tileset.TileWidth * column), row + (Tileset.TileHeight * row), Tileset.TileWidth, Tileset.TileHeight);
                int x = column * Tileset.TilesetScaledWidth + (column * Tileset.TileScale) + Tileset.TileScale;
                int y = row * Tileset.TilesetScaledHeight + (row * Tileset.TileScale) + Tileset.TileScale;
                Bitmap tileGraphic = Tileset.TilesetImage.Clone(tileSubImageRect, Tileset.TilesetImage.PixelFormat);
                spriteSheetTiles.Add(new Tile(i, tileGraphic) { X = x, Y = y, Width = Tileset.TilesetScaledWidth, Height = Tileset.TilesetScaledHeight });
            }
            tileGraphicPictureBox.Image = new Bitmap(numberOfColumns * tileset.TilesetScaledWidth + (numberOfColumns * Tileset.TileScale) + Tileset.TileScale, numberOfRows * tileset.TilesetScaledHeight + (numberOfRows * Tileset.TileScale) + Tileset.TileScale);

            tileGraphicPictureBox.Location = new Point(Math.Max(tileGraphicPanel.ClientSize.Width / 2 - tileGraphicPictureBox.Width / 2, 0), Math.Max(tileGraphicPanel.ClientSize.Height / 2 - tileGraphicPictureBox.Height / 2, 0));
            tileGraphicPictureBox.Invalidate();
        }
    }
}
