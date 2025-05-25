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

            Size = new Size(900, 730);

            tilesetEditor = new TilesetEditor();
            Controls.Add(tilesetEditor);
            tilesetEditor.Dock = DockStyle.Fill;

            MenuStrip menuStrip = new MenuStrip();

            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
            ToolStripMenuItem save = new ToolStripMenuItem("Save");
            save.Click += (sender, e) =>
            {
                // MODAL CONFIRMATION
                // SAVE CHANGES
            };
            fileMenu.DropDownItems.Add(save);

            ToolStripMenuItem editMenu = new ToolStripMenuItem("Edit");

            ToolStripMenuItem addTile = new ToolStripMenuItem("Add Tile");
            addTile.Click += (sender, e) =>
            {
                tilesetEditor.AddTile();
            };
            editMenu.DropDownItems.Add(addTile);

            ToolStripMenuItem deleteTile = new ToolStripMenuItem("Delete Tile");
            deleteTile.Click += (sender, e) =>
            {
                // DELETE CURRENTLY SELECTED TILE FROM TILE SET
            };
            editMenu.DropDownItems.Add(deleteTile);

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(editMenu);

            MainMenuStrip = menuStrip;
            Controls.Add(menuStrip);
        }
    }
}
