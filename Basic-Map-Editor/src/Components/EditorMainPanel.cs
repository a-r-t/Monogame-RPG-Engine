using Basic_Map_Editor.Models;
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
    public partial class EditorMainPanel : UserControl
    {
        public EditorControlPanel EditorControlPanel { get; private set; }
        public MapBuilder MapBuilder { get; private set; }

        public EditorMainPanel()
        {
            InitializeComponent();

            BackColor = Color.Black;
            SelectedTileIndexHolder selectedTileIndexHolder = new SelectedTileIndexHolder();
            mapBuilder = new MapBuilder(selectedTileIndexHolder);
            mapBuilder.Dock = DockStyle.Fill;
            Controls.Add(mapBuilder);
            editorControlPanel = new EditorControlPanel(selectedTileIndexHolder, mapBuilder);
            mapBuilder.setMap(editorControlPanel.getSelectedMap());
            Controls.Add(editorControlPanel);
            editorControlPanel.Dock = DockStyle.Right;

        }
    }
}
