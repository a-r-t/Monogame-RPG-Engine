using MapEditor.src.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEditor.src.MapTilesetEditor
{
    public partial class TilesetEditor : ObservableUserControl<TilesetEditorListener>
    {
        public Map Map { get; set; }
        private Panel tilesetGraphicDisplayPanel;
        private PictureBox tilesetGraphicDisplay;
        private Bitmap selectedTilesetGraphic;
        private int selectedScale = 0;

        public TilesetEditor()
        {
            InitializeComponent();

            tilesetGraphicDisplayPanel = new Panel();
            tilesetGraphicDisplayPanel.AutoScroll = true;
            tilesetGraphicDisplay = new PictureBox();
            tilesetGraphicDisplay.SizeMode = PictureBoxSizeMode.AutoSize;

            tilesetGraphicDisplayPanel.Size = new Size(400, 400);

            tilesetGraphicDisplayPanel.Controls.Add(tilesetGraphicDisplay);
            Controls.Add(tilesetGraphicDisplayPanel);
            tilesetGraphicDisplayPanel.Location = new Point(0, 170);

            tilesetGraphicDisplay.Paint += (sender, e) =>
            {
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                if (selectedTilesetGraphic != null)
                {
                    e.Graphics.DrawImage(selectedTilesetGraphic, new Rectangle(0, 0, selectedTilesetGraphic.Width * selectedScale, selectedTilesetGraphic.Height * selectedScale));
                }
            };

            Label tilesetGraphicPreviewLabel = new Label();
            tilesetGraphicPreviewLabel.Text = "Tileset Preview:";
            tilesetGraphicPreviewLabel.Location = new Point(0, 150);
            Controls.Add(tilesetGraphicPreviewLabel);

            tilesetCombobox.SelectedIndexChanged += (sender, e) =>
            {
                TilesetDataFile tilesetData = Tileset.ReadTilesetDataFile((tilesetCombobox.SelectedItem as ComboBoxItem<string>).Value);
                selectedTilesetGraphic = new Bitmap($"{Config.GraphicsPath}/{tilesetData.Properties.TilesetImagePath}");
                selectedScale = tilesetData.Properties.TileScale;
                UpdateTilesetPreviewImage();
            };
        }

        private void UpdateTilesetPreviewImage()
        {
            try
            {
                tilesetGraphicDisplay.Image = new Bitmap(selectedTilesetGraphic.Width * selectedScale, selectedTilesetGraphic.Height * selectedScale);
                tilesetGraphicDisplay.Refresh();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("Unable to update preview!");
            }
        }

        public void Reset()
        {
            tilesetCombobox.Items.Clear();
            string[] tilesetFiles = Directory.GetFiles($"{Config.TilesetFilesPath}");
            ComboBoxItem<string> selectedItem = null;
            foreach (string tilesetFile in tilesetFiles)
            {
                ComboBoxItem<string> comboBoxItem = new ComboBoxItem<string>(tilesetFile, Path.GetFileNameWithoutExtension(tilesetFile));
                tilesetCombobox.Items.Add(comboBoxItem);
                if (comboBoxItem.Display == Map.Tileset.Name)
                {
                    selectedItem = comboBoxItem;
                }
                
            }
            tilesetCombobox.Sorted = true;
            tilesetCombobox.SelectedItem = selectedItem;

            selectedTilesetGraphic = Map.Tileset.TilesetImage;
            selectedScale = Map.Tileset.TileScale;

            errorMessageLabel.Visible = false;

            tilesetGraphicDisplay.Image = new Bitmap(selectedTilesetGraphic.Width * selectedScale, selectedTilesetGraphic.Height * selectedScale);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            errorMessageLabel.Visible = false;
            try
            {
                UpdateTilesetInfo(tilesetCombobox.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                errorMessageLabel.Visible = true;
                errorMessageLabel.Text = $"Error: {ex.Message}";
            }
        }

        private void UpdateTilesetInfo(string newTilesetName)
        {
            if (newTilesetName != Map.Tileset.Name)
            {
                Map.Tileset.TilesetFilePath = $"./Resources/TilesetFiles/{newTilesetName}.tileset";
            }

            Map.Tileset.LoadTileset();

            foreach (TilesetEditorListener listener in listeners)
            {
                listener.OnTilesetInfoUpdated(newTilesetName);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            foreach (TilesetEditorListener listener in listeners)
            {
                listener.OnTilesetInfoUpdateCanceled();
            }
        }

        private void TilesetEditor_Load(object sender, EventArgs e)
        {

        }
    }
}
