using MapEditor.src.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEditor.src.MapTilesetEditor
{
    public partial class TilesetEditor : ObservableUserControl<TilesetEditorListener>
    {
        public Map Map { get; set; }

        public TilesetEditor()
        {
            InitializeComponent();
        }

        public void Reset()
        {
            tilesetCombobox.Items.Clear();
            string[] tilesetFiles = Directory.GetFiles("./Resources/TilesetFiles");
            foreach (string tilesetFile in tilesetFiles)
            {
                tilesetCombobox.Items.Add(Path.GetFileNameWithoutExtension(tilesetFile));
            }
            tilesetCombobox.Sorted = true;
            tilesetCombobox.SelectedItem = Map.Tileset.Name;

            scaleTextbox.Text = Map.Tileset.TileScale.ToString();

            errorMessageLabel.Visible = false;
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
