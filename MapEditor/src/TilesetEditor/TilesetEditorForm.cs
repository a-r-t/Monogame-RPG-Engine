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
    public partial class TilesetEditorForm : Form
    {
        private TilesetEditor tilesetEditor;

        public TilesetEditorForm()
        {
            InitializeComponent();

            Size = new Size(900, 700);

            tilesetEditor = new TilesetEditor();
            Controls.Add(tilesetEditor);
            tilesetEditor.Dock = DockStyle.Fill;
        }
    }
}
