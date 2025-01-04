using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEditor.src
{
    public partial class ObservableUserControl<T> : UserControl
    {
        protected List<T> listeners = new List<T>();

        public ObservableUserControl()
        {
            InitializeComponent();
        }

        public void AddListener(T listener)
        {
            listeners.Add(listener);
        }
    }
}
