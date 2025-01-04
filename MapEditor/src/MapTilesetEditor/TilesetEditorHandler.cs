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

namespace MapEditor.src.MapTilesetEditor
{
    public partial class TilesetEditorHandler : ObservableUserControl<TilesetEditorHandlerListener>, TilesetDisplayListener, TilesetEditorListener
    {
        private TilesetDisplay tilesetDisplay;
        private TilesetEditor tilesetEditor;

        private Map map;
        public Map Map { 
            get
            {
                return map;
            }
            set
            {
                map = value;
                tilesetDisplay.Map = map;
                tilesetDisplay.Reset();
                tilesetEditor.Map = map;
                tilesetEditor.Reset();
            }
        }

        private EditMode editMode;
        public EditMode EditMode
        {
            get
            {
                return editMode;
            }
            set
            {
                editMode = value;
                if (editMode == EditMode.DISPLAY)
                {
                    tilesetDisplay.Show();
                    tilesetEditor.Hide();
                    tilesetDisplay.Reset();
                }
                else if (editMode == EditMode.EDIT)
                {
                    tilesetDisplay.Hide();
                    tilesetEditor.Show();
                    tilesetEditor.Reset();
                }
            }
        }

        public TilesetEditorHandler()
        {
            InitializeComponent();
            tilesetDisplay = new TilesetDisplay();
            tilesetEditor = new TilesetEditor();
            tilesetEditorPanel.Controls.Add(tilesetDisplay);
            tilesetEditorPanel.Controls.Add(tilesetEditor);
            tilesetDisplay.Show();
            tilesetEditor.Hide();
            tilesetDisplay.Dock = DockStyle.Fill;
            tilesetEditor.Dock = DockStyle.Fill;
            tilesetDisplay.AddListener(this);
            tilesetEditor.AddListener(this);
        }

        public void OnChangeTilesetInfoRequested()
        {
            EditMode = EditMode.EDIT;
        }

        public void OnTilesetInfoUpdated(string tilesetName, int scale)
        {
            EditMode = EditMode.DISPLAY;

            foreach (TilesetEditorHandlerListener listener in listeners)
            {
                listener.OnTilesetInfoUpdated(tilesetName, scale);
            }
        }

        public void OnTilesetInfoUpdateCanceled()
        {
            EditMode = EditMode.DISPLAY;
        }
    }
}
