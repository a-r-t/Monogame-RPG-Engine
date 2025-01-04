using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MapEditor.src.Models;

namespace MapEditor.src.MapTilesetEditor
{
    public partial class TilesetDisplay : ObservableUserControl<TilesetDisplayListener>
    {
        public Map Map { get; set; }

        public TilesetDisplay()
        {
            InitializeComponent();
        }

        public void Reset()
        {
            tilesetLabel.Text = $"Tileset: {Map.Tileset.Name}";
            scaleLabel.Text = $"Scale: {Map.Tileset.TileScale}";
        }

        private void changeTilesetInfoButton_Click(object sender, EventArgs e)
        {
            foreach (TilesetDisplayListener listener in listeners)
            {
                listener.OnChangeTilesetInfoRequested();
            }
        }
    }
}
