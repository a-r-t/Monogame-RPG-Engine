using MapEditor.Models;
using MapEditor.src.MapTilePicker;
using MapEditor.src.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEditor.src.TilesetEditor
{
    // note to future self: this class partially uses the designer because you were literally about to crashout over a bug in winforms over docking controls
    //  and then you were about to go postal over another issue where a panel auto scroll just wouldn't work no matter how hard you tried
    //  I guess it all eventually worked but just trust me, this thing is setup this way for a reason!!!
    public partial class TilesetEditor : UserControl
    {
        private Tileset tileset;

        private PictureBox tilesetTilesPictureBox;

        private Panel tilesetTilesPictureBoxPanel;

        private Tile selectedTile;

        public TilesetEditor()
        {
            InitializeComponent();

            // had to partially use the designer view to use split containers, so this is just an alias to make the code easier to read since I can't rename the panels in the split containers
            tilesetTilesPictureBoxPanel = splitContainer2.Panel1;

            string[] tilesetFilePaths = Directory.GetFiles(Config.TilesetFilesPath, "*.tileset", SearchOption.AllDirectories);
            Array.Sort(tilesetFilePaths, StringComparer.OrdinalIgnoreCase);
            foreach (string tilesetFilePath in Directory.GetFiles(Config.TilesetFilesPath))
            {
                string tilesetName = Path.GetFileNameWithoutExtension(tilesetFilePath);
                tilesetListBox.Items.Add(new ListBoxItem<string>(tilesetFilePath.Replace("\\", "/"), tilesetName));
            }
            tilesetListBox.SelectionMode = SelectionMode.One;

            tilesetListBox.SelectedIndexChanged += (sender, e) =>
            {
                ListBoxItem<string> selectedTileset = tilesetListBox.Items[tilesetListBox.SelectedIndex] as ListBoxItem<string>;
                tileset = new Tileset(selectedTileset.Value);

                SetupTilePanel();
            };

            tilesetTilesPictureBoxPanel.BackColor = Color.Black;
            tilesetTilesPictureBoxPanel.AutoScroll = true;
            tilesetTilesPictureBoxPanel.Resize += (sender, e) =>
            {
                if (tileset != null)
                {
                    SetupTilePanel();
                    tilesetTilesPictureBoxPanel.Invalidate();
                }
            };

            tilesetTilesPictureBox = new PictureBox();
            tilesetTilesPictureBox.Location = new Point(0, 0);
            tilesetTilesPictureBox.BackColor = Color.Black;
            tilesetTilesPictureBoxPanel.Controls.Add(tilesetTilesPictureBox);
            tilesetTilesPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;

            tilesetTilesPictureBox.Paint += (sender, e) =>
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
            };

            tilesetTilesPictureBox.MouseMove += (sender, e) =>
            {
                if (tileset != null)
                {
                    foreach (Tile tile in tileset.Tiles)
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

            tilesetTilesPictureBox.MouseLeave += (sender, e) =>
            {
                Cursor = Cursors.Default;
            };

            tilesetTilesPictureBox.MouseClick += (sender, e) =>
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
                    Cursor = Cursors.Default;
                }
            };

            tilesetTilesPictureBox.MouseDown += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    foreach (Tile tile in tileset.Tiles)
                    {
                        if (tile.IsPointInTile(e.Location))
                        {
                            selectedTile = tile;
                            tilesetTilesPictureBox.Invalidate();
                            return;
                        }
                    }
                }
            };




tilesetListBox.SelectedIndex = 0;
        }

        private void SetupTilePanel()
        {
            int tileSpacing = tileset.TileScale + 2;
            int numberOfColumns = Math.Max((tilesetTilesPictureBoxPanel.ClientSize.Width - tileSpacing) / (tileset.TilesetScaledWidth + tileSpacing), 1);
            int numberOfRows = (int)Math.Ceiling(tileset.NumberOfTiles / (float)numberOfColumns);

            // set location of tiles in tile picker
            for (int i = 0; i < tileset.Tiles.Length; i++)
            {
                Tile tile = tileset.Tiles[i];
                int row = i / numberOfColumns;
                int column = i % numberOfColumns;
                tile.SetLocation(column * tileset.TilesetScaledWidth + (column * tileSpacing) + tileSpacing, row * tileset.TilesetScaledHeight + (row * tileSpacing) + tileSpacing);
                tile.SetDimensions(tileset.TilesetScaledWidth, tileset.TilesetScaledHeight);
            }

            tilesetTilesPictureBox.Image = new Bitmap(tilesetTilesPictureBoxPanel.ClientSize.Width, numberOfRows * tileset.TilesetScaledHeight + (numberOfRows * tileSpacing) + tileSpacing);
        }
    }
}
