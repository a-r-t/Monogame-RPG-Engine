using Basic_Map_Editor.Models;
using Monogame_RPG_Engine.Engine.Scene.MapCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Basic_Map_Editor.Components
{
    public partial class MapBuilder : UserControl
    {
        private Map map;
        private Panel tileBuilderScroll;
        public TileBuilder TileBuilder { get; private set; }
        private Label mapWidthLabel;
        private Label mapHeightLabel;
        private Label hoveredTileIndexLabel;

        public MapBuilder(SelectedTileIndexHolder controlPanelHolder)
        {
            InitializeComponent();

            BackColor = Color.CornflowerBlue;
            Location = new Point(205, 5);

            Panel labelPanel = new Panel();
            labelPanel.Size = new Size(200, 30);
            labelPanel.BackColor = Color.CornflowerBlue;
            mapWidthLabel = new Label();
            mapWidthLabel.Text = "Width: ";
            mapWidthLabel.Size = new Size(70, 20);
            mapWidthLabel.Location = new Point(2, 5);
            labelPanel.Controls.Add(mapWidthLabel);
            mapHeightLabel = new Label();
            mapHeightLabel.Text = "Height: ";
            mapHeightLabel.Size = new Size(70, 20);
            mapHeightLabel.Location = new Point(76, 5);
            labelPanel.Controls.Add(mapHeightLabel);
            hoveredTileIndexLabel = new Label();
            hoveredTileIndexLabel.Text = "X: , Y:";
            hoveredTileIndexLabel.Size = new Size(140, 20);
            hoveredTileIndexLabel.Location = new Point(152, 5);
            labelPanel.Controls.Add(hoveredTileIndexLabel);
            Controls.Add(labelPanel);
            labelPanel.Dock = DockStyle.Bottom;

            TileBuilder = new TileBuilder(controlPanelHolder, hoveredTileIndexLabel);
            tileBuilderScroll = new Panel();
            tileBuilderScroll.Controls.Add(TileBuilder);
            TileBuilder.Dock = DockStyle.Fill;
            ScrollToMaxY();
            tileBuilderScroll.Location = new Point(0, 0);
            tileBuilderScroll.Size = new Size(585, 546);
            Controls.Add(tileBuilderScroll);
            tileBuilderScroll.Dock = DockStyle.Fill;
        }

        public void SetMap(Map map)
        {
            this.map = map;
            RefreshTileBuilder();
        }

        public void RefreshTileBuilder()
        {
            tileBuilderScroll.Controls.Clear();
            TileBuilder.SetMap(map);
            tileBuilderScroll.Controls.Add(TileBuilder);

            ScrollToMaxY();
            mapWidthLabel.Text = "Width: " + map.Width;
            mapHeightLabel.Text = "Height: " + map.Height;
        }

        public void ScrollToMaxY()
        {
            tileBuilderScroll.VerticalScroll.Value = tileBuilderScroll.VerticalScroll.Maximum;
        }

    }
}
