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

namespace MapEditor.src.MapDimensionsEditor
{
    public partial class DimensionsDisplay : ObservableUserControl<DimensionsDisplayListener>
    {
        public Map Map { get; set; }

        public DimensionsDisplay()
        {
            InitializeComponent();
        }

        private void changeMapDimensionsButton_Click(object sender, EventArgs e)
        {
            foreach (DimensionsDisplayListener listener in listeners)
            {
                listener.OnChangeDimensionsRequested();
            }
        }

        public void Reset()
        {
            widthLabel.Text = $"Width: {Map.Width}";
            heightLabel.Text = $"Height: {Map.Height}";
        }
    }
}
