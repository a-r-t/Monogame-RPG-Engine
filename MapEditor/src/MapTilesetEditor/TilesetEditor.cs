using MapEditor.Models;
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
            tilesetGraphicDisplayPanel.Location = new Point(0, 190);

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
            tilesetGraphicPreviewLabel.Location = new Point(0, 170);
            Controls.Add(tilesetGraphicPreviewLabel);

            tilesetCombobox.SelectedIndexChanged += (sender, e) =>
            {
                Dictionary<string, JsonElement> tilesetProperties = Tileset.ReadTilesetFile((tilesetCombobox.SelectedItem as ComboBoxItem<string>).Value);
                selectedTilesetGraphic = new Bitmap($"./Resources/Tilesets/{tilesetProperties["tilesetImage"].ToString()}");
                UpdateTilesetPreviewImage();
            };

            scaleTextbox.TextChanged += (sender, e) =>
            {
                if (IsScaleInputValid())
                {
                    selectedScale = int.Parse(scaleTextbox.Text);
                    UpdateTilesetPreviewImage();
                }
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
            string[] tilesetFiles = Directory.GetFiles("./Resources/TilesetFiles");
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

            scaleTextbox.Text = selectedScale.ToString();

            errorMessageLabel.Visible = false;

            tilesetGraphicDisplay.Image = new Bitmap(selectedTilesetGraphic.Width * selectedScale, selectedTilesetGraphic.Height * selectedScale);
        }

        private bool IsScaleInputValid()
        {
            int newScale = 0;
            try
            {
                newScale = int.Parse(scaleTextbox.Text);
            }
            catch (Exception ex)
            {
                return false;
            }
            if (newScale < 1)
            {
                return false;
            }
            return true;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            errorMessageLabel.Visible = false;
            bool isValid = true;

            // validate scale
            int newScale = 0;
            try
            {
                newScale = int.Parse(scaleTextbox.Text);
            }
            catch (Exception ex)
            {
                isValid = false;
                if (ex is ArgumentNullException || ex is FormatException || ex is OverflowException)
                {
                    ShowChangeDimensionsError("Scale must be an int");
                }
                else
                {
                    ShowChangeDimensionsError(ex.Message);
                }
            }
            if (newScale < 1)
            {
                isValid = false;
                ShowChangeDimensionsError("Scale must be >= 1");
            }

            if (isValid)
            {
                UpdateTilesetInfo(tilesetCombobox.SelectedItem.ToString(), newScale);
            }
        }

        private void UpdateTilesetInfo(string newTilesetName, int newTilesetScale)
        {
            if (newTilesetName != Map.Tileset.Name)
            {
                Map.Tileset.TilesetFilePath = $"./Resources/TilesetFiles/{newTilesetName}.tileset";
            }
            if (newTilesetScale != Map.Tileset.TileScale)
            {
                Map.Tileset.TileScale = newTilesetScale;
            }

            Map.Tileset.LoadTileset();

            foreach (TilesetEditorListener listener in listeners)
            {
                listener.OnTilesetInfoUpdated(newTilesetName, newTilesetScale);
            }
        }

        private void ShowChangeDimensionsError(string errorMessage)
        {
            errorMessageLabel.Visible = true;
            errorMessageLabel.Text = $"Error: {errorMessage}";
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
