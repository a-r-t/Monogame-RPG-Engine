using MapEditor.Models;
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
    public partial class TilesetEditor : UserControl
    {
        private Tileset tileset;
        private ListBox tilesetListBox;

        public TilesetEditor()
        {
            InitializeComponent();
            tilesetListBox = new ListBox();
            tilesetListBox.Dock = DockStyle.Left;
            string[] tilesetFilePaths = Directory.GetFiles(Config.TilesetFilesPath, "*.tileset", SearchOption.AllDirectories);
            Array.Sort(tilesetFilePaths, StringComparer.OrdinalIgnoreCase);
            foreach (string tilesetFilePath in Directory.GetFiles(Config.TilesetFilesPath))
            {
                string tilesetName = Path.GetFileNameWithoutExtension(tilesetFilePath);
                tilesetListBox.Items.Add(new ListBoxItem<string>(tilesetFilePath.Replace("\\", "/"), tilesetName));
            }

            Controls.Add(tilesetListBox);
            tilesetListBox.SelectionMode = SelectionMode.One;

            tilesetListBox.SelectedIndexChanged += (sender, e) =>
            {
                ListBoxItem<string> selectedTileset = tilesetListBox.Items[tilesetListBox.SelectedIndex] as ListBoxItem<string>;
                tileset = new Tileset(selectedTileset.Value);
            };

            tilesetListBox.SelectedIndex = 0;

        }
    }
}
