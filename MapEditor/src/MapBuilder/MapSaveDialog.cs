using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEditor.src.MapBuilder
{
    public partial class MapSaveDialog : Form
    {
        private MapBuilder mapBuilder;

        // for designer purposes only
        public MapSaveDialog()
        {
            InitializeComponent();
        }

        public MapSaveDialog(MapBuilder mapBuilder)
        {
            InitializeComponent();
            this.mapBuilder = mapBuilder;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            mapBuilder?.SaveMap();
            this.Close();
        }

        private void btnDiscard_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static void Show(MapBuilder mapBuilder)
        {
            MapSaveDialog mapSaveDialog = new MapSaveDialog(mapBuilder);
            mapSaveDialog.ShowDialog();
        }
    }
}
