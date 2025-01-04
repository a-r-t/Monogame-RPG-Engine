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

namespace MapEditor.src.MapDimensionsEditor
{
    public partial class DimensionsEditorHandler : ObservableUserControl<DimensionsEditorHandlerListener>, DimensionsDisplayListener, DimensionsEditorListener
    {
        private DimensionsDisplay dimensionsDisplay;
        private DimensionsEditor dimensionsEditor;

        private Map map;
        public Map Map
        {
            get
            {
                return map;
            }
            set
            {
                map = value;
                dimensionsDisplay.Map = map;
                dimensionsDisplay.Reset();
                dimensionsEditor.Map = map;
                dimensionsEditor.Reset();
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
                    dimensionsDisplay.Show();
                    dimensionsEditor.Hide();
                    dimensionsDisplay.Reset();
                }
                else if (editMode == EditMode.EDIT)
                {
                    dimensionsDisplay.Hide();
                    dimensionsEditor.Show();
                    dimensionsEditor.Reset();
                }
            }
        }

        public DimensionsEditorHandler()
        {
            InitializeComponent();
            dimensionsDisplay = new DimensionsDisplay();
            dimensionsEditor = new DimensionsEditor();
            dimensionsEditorPanel.Controls.Add(dimensionsDisplay);
            dimensionsEditorPanel.Controls.Add(dimensionsEditor);
            dimensionsDisplay.Show();
            dimensionsEditor.Hide();
            dimensionsDisplay.Dock = DockStyle.Fill;
            dimensionsEditor.Dock = DockStyle.Fill;
            dimensionsDisplay.AddListener(this);
            dimensionsEditor.AddListener(this);
        }

        public void OnChangeDimensionsRequested()
        {
            EditMode = EditMode.EDIT;
        }

        public void OnDimensionsUpdated(int width, int height)
        {
            EditMode = EditMode.DISPLAY;

            foreach (DimensionsEditorHandlerListener listener in listeners)
            {
                listener.OnDimensionsUpdated(width, height);
            }
        }

        public void OnDimensionsUpdateCanceled()
        {
            EditMode = EditMode.DISPLAY;
        }
    }
}
