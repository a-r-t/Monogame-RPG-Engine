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
using System.Diagnostics;

namespace MapEditor.src.MapTilesetEditor
{
    public partial class TilesetDisplay : ObservableUserControl<TilesetDisplayListener>
    {
        public Map Map { get; set; }
        private Panel tilesetGraphicDisplayPanel;
        private PictureBox tilesetGraphicDisplay;

        public TilesetDisplay()
        {
            InitializeComponent();

            tilesetGraphicDisplayPanel = new Panel();
            tilesetGraphicDisplayPanel.AutoScroll = true;
            tilesetGraphicDisplay = new PictureBox();
            tilesetGraphicDisplay.SizeMode = PictureBoxSizeMode.AutoSize;

            tilesetGraphicDisplayPanel.Size = new Size(400, 400);

            tilesetGraphicDisplayPanel.Controls.Add(tilesetGraphicDisplay);
            Controls.Add(tilesetGraphicDisplayPanel);
            tilesetGraphicDisplayPanel.Location = new Point(0, 150);
            tilesetGraphicDisplay.Paint += (sender, e) =>
            {
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                if (Map != null)
                {
                    e.Graphics.DrawImage(Map.Tileset.TilesetImage, new Rectangle(0, 0, Map.Tileset.TilesetImage.Width * Map.Tileset.TileScale, Map.Tileset.TilesetImage.Height * Map.Tileset.TileScale));
                }
            };

            Label tilesetGraphicPreviewLabel = new Label();
            tilesetGraphicPreviewLabel.Text = "Tileset Preview:";
            tilesetGraphicPreviewLabel.Location = new Point(0, 130);
            Controls.Add(tilesetGraphicPreviewLabel);
        }

        public void Reset()
        {
            tilesetLabel.Text = $"Tileset: {Map.Tileset.Name}";
            scaleLabel.Text = $"Scale: {Map.Tileset.TileScale}";

            tilesetGraphicDisplay.Image = new Bitmap(Map.Tileset.TilesetImage.Width * Map.Tileset.TileScale, Map.Tileset.TilesetImage.Height * Map.Tileset.TileScale);
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
