using System.Windows.Forms;

namespace Basic_Map_Editor.Forms
{
    public partial class MainForm : Form
    {
        private Panel mainPanel;

        public MainForm()
        {
            InitializeComponent();

            Width = 800;
            Height = 600;

            CenterToScreen();

            mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = Color.CornflowerBlue;

            Controls.Add(mainPanel);
        }
    }
}