using MapEditor.src.Models;
using MapEditor.src.TilesetsManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MapEditor.src.Models.TilesetDataFile;

namespace MapEditor.src.TilesetEditor
{
    public partial class TilesetsManagerForm : Form
    {
        private Tileset tileset;

        public TilesetsManagerForm()
        {
            InitializeComponent();

            tilesetsListBox.SelectionMode = SelectionMode.One;

            tilesetsListBox.SelectedIndexChanged += (sender, e) =>
            {
                ListBoxItem<string> selectedTileset = tilesetsListBox.Items[tilesetsListBox.SelectedIndex] as ListBoxItem<string>;
                tileset = new Tileset(selectedTileset.Value);

                SetupTilePanel();
            };

            tilesetImagePreviewPanel.BackColor = Color.Black;
            tilesetImagePreviewPanel.AutoScroll = true;
            tilesetImagePreviewPanel.Resize += (sender, e) =>
            {
                if (tileset != null)
                {
                    SetupTilePanel();
                    tilesetImagePreviewPanel.Invalidate();
                }
            };

            tilesetPreviewPictureBox.Location = new Point(0, 0);
            tilesetPreviewPictureBox.BackColor = Color.Transparent;
            tilesetPreviewPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;

            tilesetPreviewPictureBox.Paint += (sender, e) =>
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
                }
            };

            LoadTilesets();
        }

        private void LoadTilesets()
        {
            tilesetsListBox.Items.Clear();
            string[] tilesetFilePaths = Directory.GetFiles(Config.TilesetFilesPath, "*.tileset", SearchOption.AllDirectories);
            Array.Sort(tilesetFilePaths, StringComparer.OrdinalIgnoreCase);
            foreach (string tilesetFilePath in Directory.GetFiles(Config.TilesetFilesPath))
            {
                string tilesetName = Path.GetFileNameWithoutExtension(tilesetFilePath);
                tilesetsListBox.Items.Add(new ListBoxItem<string>(tilesetFilePath.Replace("\\", "/"), tilesetName));
            }

            if (tilesetsListBox.Items.Count > 0)
            {
                tilesetsListBox.SelectedIndex = 0;
                tilesetPreviewPictureBox.BackColor = Config.TransparentColor;
            }
        }

        private void SetupTilePanel()
        {
            int tileSpacing = tileset.TileScale + 2;
            int numberOfColumns = Math.Max((tilesetImagePreviewPanel.ClientSize.Width - tileSpacing) / (tileset.TilesetScaledWidth + tileSpacing), 1);
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

            tilesetPreviewPictureBox.Image = new Bitmap(tilesetImagePreviewPanel.ClientSize.Width, numberOfRows * tileset.TilesetScaledHeight + (numberOfRows * tileSpacing) + tileSpacing);
        }

        private void createTilesetButton_Click(object sender, EventArgs e)
        {
            CreateOrEditTilesetForm createTilesetForm = new CreateOrEditTilesetForm();
            createTilesetForm.Text = "Create Tileset";

            DialogResult createTilesetFormResult = createTilesetForm.ShowDialog();
            if (createTilesetFormResult == DialogResult.OK)
            {
                TilesetDataFile tilesetDataFile = new TilesetDataFile();
                tilesetDataFile.Properties = new PropertiesData();
                tilesetDataFile.Properties.TileWidth = createTilesetForm.TilesetWidth;
                tilesetDataFile.Properties.TileHeight = createTilesetForm.TilesetHeight;
                tilesetDataFile.Properties.TileScale = createTilesetForm.TilesetScale;
                tilesetDataFile.Properties.TilesetImagePath = createTilesetForm.TilesetImage;
                tilesetDataFile.Tiles = new List<TileData>();

                string newTilesetPath = $"{Config.TilesetFilesPath}/{createTilesetForm.TilesetName}.tileset";
                if (File.Exists(newTilesetPath))
                {
                    MessageBox.Show($"Error creating tileset: file {newTilesetPath} already exists!");
                }
                else
                {
                    try
                    {
                        File.WriteAllText(newTilesetPath, JsonSerializer.Serialize(tilesetDataFile, new JsonSerializerOptions { WriteIndented = true }));
                        LoadTilesets();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error creating tileset: {ex.Message}");
                    }
                }
            }
            createTilesetForm.Dispose();
        }

        private void editTilesetButton_Click(object sender, EventArgs e)
        {
            CreateOrEditTilesetForm editTilesetForm = new CreateOrEditTilesetForm();
            editTilesetForm.Text = "Edit Tileset Properties";
            editTilesetForm.TilesetName = tileset.Name;
            editTilesetForm.TilesetWidth = tileset.TileWidth;
            editTilesetForm.TilesetHeight = tileset.TileHeight;
            editTilesetForm.TilesetScale = tileset.TileScale;
            editTilesetForm.TilesetImage = tileset.TilesetDataFile.Properties.TilesetImagePath;

            DialogResult editTilesetFormResult = editTilesetForm.ShowDialog();
            if (editTilesetFormResult == DialogResult.OK)
            {
                TilesetDataFile tilesetDataFile = new TilesetDataFile();
                tilesetDataFile.Properties = new PropertiesData();
                tilesetDataFile.Properties.TileWidth = editTilesetForm.TilesetWidth;
                tilesetDataFile.Properties.TileHeight = editTilesetForm.TilesetHeight;
                tilesetDataFile.Properties.TileScale = editTilesetForm.TilesetScale;
                tilesetDataFile.Properties.TilesetImagePath = editTilesetForm.TilesetImage;
                tilesetDataFile.Tiles = new List<TileData>();

                string newTilesetPath = $"{Config.TilesetFilesPath}/{editTilesetForm.TilesetName}.tileset";
                string originalTilesetPath = $"{Config.TilesetFilesPath}/{tileset.Name}.tileset";

                if (originalTilesetPath != newTilesetPath && File.Exists(newTilesetPath))
                {
                    MessageBox.Show($"Error editing tileset: file {newTilesetPath} already exists, cannot rename!!");
                }
                else
                {
                    try
                    {
                        File.WriteAllText(originalTilesetPath, JsonSerializer.Serialize(tilesetDataFile, new JsonSerializerOptions { WriteIndented = true }));
                        if (originalTilesetPath != newTilesetPath)
                        {
                            File.Move(originalTilesetPath, newTilesetPath, false);
                        }
                        LoadTilesets();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating tileset: {ex.Message}");
                    }
                }
            }
            editTilesetForm.Dispose();
        }

        private void deleteTilesetButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete tileset {tilesetsListBox.Items[tilesetsListBox.SelectedIndex]}?",       
                "Confirm",                       
                MessageBoxButtons.OKCancel,       
                MessageBoxIcon.Question           
            );

            if (result == DialogResult.OK)
            {
                try
                {
                    string tilesetPath = $"{Config.TilesetFilesPath}/{tileset.Name}.tileset";
                    File.Delete(tilesetPath);
                    LoadTilesets();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting tileset: {ex.Message}");
                }
            }
        }
    }
}
