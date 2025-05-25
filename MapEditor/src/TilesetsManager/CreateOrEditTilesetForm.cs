using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEditor.src.TilesetsManager
{
    public partial class CreateOrEditTilesetForm : Form
    {
        private string tilesetName;
        public string TilesetName
        {
            get
            {
                return tilesetName;
            }
            set
            {
                tilesetName = value;
                nameTextBox.Text = tilesetName;
            }
        }
        private int tilesetWidth;
        public int TilesetWidth
        {
            get
            {
                return tilesetWidth;
            }
            set
            {
                tilesetWidth = value;
                widthTextBox.Value = tilesetWidth;
            }
        }
        private int tilesetHeight;
        public int TilesetHeight
        {
            get
            {
                return tilesetHeight;
            }
            set
            {
                tilesetHeight = value;
                heightTextBox.Value = tilesetHeight;
            }
        }
        private int tilesetScale;
        public int TilesetScale
        {
            get
            {
                return tilesetScale;
            }
            set
            {
                tilesetScale = value;
                scaleTextBox.Value = tilesetScale;
            }
        }
        private string tilesetImage;
        public string TilesetImage
        {
            get
            {
                return tilesetImage;
            }
            set
            {
                tilesetImage = value;
                imageTextBox.Text = tilesetImage;
            }
        }

        public CreateOrEditTilesetForm()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            widthTextBox.Minimum = 1;
            widthTextBox.Maximum = decimal.MaxValue;
            heightTextBox.Minimum = 1;
            heightTextBox.Maximum = decimal.MaxValue;
            scaleTextBox.Minimum = 1;
            scaleTextBox.Maximum = decimal.MaxValue;

            nameTextBox.TextChanged += (sender, e) =>
            {
                TilesetName = nameTextBox.Text.Replace(" ", "_");
            };
            TilesetName = nameTextBox.Text;

            widthTextBox.ValueChanged += (sender, e) =>
            {
                TilesetWidth = (int)widthTextBox.Value;
            };
            TilesetWidth = (int)widthTextBox.Value;

            heightTextBox.ValueChanged += (sender, e) =>
            {
                TilesetHeight = (int)heightTextBox.Value;
            };
            TilesetHeight = (int)heightTextBox.Value;

            scaleTextBox.ValueChanged += (sender, e) =>
            {
                TilesetScale = (int)scaleTextBox.Value;
            };
            TilesetScale = (int)scaleTextBox.Value;

            imageTextBox.TextChanged += (sender, e) =>
            {
                TilesetImage = imageTextBox.Text;
            };
            TilesetImage = imageTextBox.Text;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            errorMessage.Visible = false;
            errorMessage.Text = "";
            if (TilesetName == null || TilesetName == "")
            {
                errorMessage.Text = "Name cannot be empty";
            }
            else if (TilesetWidth < 1)
            {
                errorMessage.Text = "Width cannot be less than 1";
            }
            else if (TilesetHeight < 1)
            {
                errorMessage.Text = "Height cannot be less than 1";
            }
            else if (TilesetScale < 1)
            {
                errorMessage.Text = "Scale cannot be less than 1";
            }
            else if (TilesetImage == null || TilesetImage == "")
            {
                errorMessage.Text = "Image cannot be empty";
            }

            if (errorMessage.Text != "")
            {
                errorMessage.Visible = true;
            }
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
