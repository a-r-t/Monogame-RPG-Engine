using MapEditor.src.Models;
using MapEditor.src.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MapEditor.src.Models.TilesetDataFile;
using static System.Net.Mime.MediaTypeNames;

namespace MapEditor.src.TilesetEditor
{
    // note to future self: this class partially uses the designer because you were literally about to crashout over a bug in winforms over docking controls
    //  and then you were about to go postal over another issue where a panel auto scroll just wouldn't work no matter how hard you tried
    //  I guess it all eventually worked but just trust me, this thing is setup this way for a reason!!!
    public partial class TilesetEditor : UserControl
    {
        private Tileset tileset;

        private PictureBox tilesetTilesPictureBox;

        private Panel tilesetTilesPictureBoxPanel;
        private Panel tilePropertiesPanel;

        private Tile selectedTile;
        private Tile SelectedTile
        {
            get
            {
                return selectedTile;
            }
            set
            {
                if (value != null && (selectedTile == null || selectedTile.Index != value.Index))
                {
                    foreach (System.Windows.Forms.Timer timer in animationTimers)
                    {
                        timer.Stop();
                    }
                    selectedTile = value;
                    selectedTileData = tileset.TilesetDataFile.Tiles[value.Index];
                    OnTileSelected();
                }
            }
        }
        private TileData selectedTileData;

        private TextBox nameTextbox;
        private ComboBox tileTypeComboBox;

        // list of layers, each layer then has a list of each frame of the bitmap
        private List<List<(FrameData frameData, Bitmap image)>> tileLayerPreviews;
        private PictureBox tilePreviewPictureBox;
        private List<int> currentFrameIndexes;
        private List<System.Windows.Forms.Timer> animationTimers = new List<System.Windows.Forms.Timer>();
        private List<int> frameTimers;


        private Label layerLabel;
        private Button addLayerButton;
        private Button previousLayer;
        private Button nextLayer;

        private Label frameLabel;
        private Button previousFrame;
        private Button nextFrame;
        private Button addFrameButton;

        private NumericUpDown rowInput;
        private NumericUpDown columnInput;
        private Button selectGraphicButton;
        private ComboBox imageEffectComboBox;
        private NumericUpDown delayInput;
        

        public TilesetEditor()
        {
            InitializeComponent();

            // had to partially use the designer view to use split containers, so this is just an alias to make the code easier to read since I can't rename the panels in the split containers
            tilesetTilesPictureBoxPanel = splitContainer2.Panel1;
            tilePropertiesPanel = splitContainer2.Panel2;

            string[] tilesetFilePaths = Directory.GetFiles(Config.TilesetFilesPath, "*.tileset", SearchOption.AllDirectories);
            Array.Sort(tilesetFilePaths, StringComparer.OrdinalIgnoreCase);
            foreach (string tilesetFilePath in Directory.GetFiles(Config.TilesetFilesPath))
            {
                string tilesetName = Path.GetFileNameWithoutExtension(tilesetFilePath);
                tilesetListBox.Items.Add(new ListBoxItem<string>(tilesetFilePath.Replace("\\", "/"), tilesetName));
            }
            tilesetListBox.SelectionMode = SelectionMode.One;

            tilesetListBox.SelectedIndexChanged += (sender, e) =>
            {
                ListBoxItem<string> selectedTileset = tilesetListBox.Items[tilesetListBox.SelectedIndex] as ListBoxItem<string>;
                tileset = new Tileset(selectedTileset.Value);

                SetupTilePanel();

            };

            tilesetTilesPictureBoxPanel.BackColor = Config.TransparentColor;
            tilesetTilesPictureBoxPanel.AutoScroll = true;
            tilesetTilesPictureBoxPanel.Resize += (sender, e) =>
            {
                if (tileset != null)
                {
                    SetupTilePanel();
                    tilesetTilesPictureBoxPanel.Invalidate();
                }
            };

            tilesetTilesPictureBox = new PictureBox();
            tilesetTilesPictureBox.Location = new Point(0, 0);
            tilesetTilesPictureBox.BackColor = Color.Black;
            tilesetTilesPictureBoxPanel.Controls.Add(tilesetTilesPictureBox);
            tilesetTilesPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;

            tilesetTilesPictureBox.Paint += (sender, e) =>
            {
                if (tileset != null)
                {
                    e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                    // paint tile picker tiles
                    for (int i = 0; i < tileset.Tiles.Length; i++)
                    {
                        Tile tile = tileset.Tiles[i];
                        tile.Paint(e.Graphics);
                    }

                    // paint yellow rectangle around selected tile
                    if (selectedTile != null)
                    {
                        int borderSize = tileset.TileScale + 2;
                        Pen pen = new Pen(Color.Yellow, borderSize);
                        e.Graphics.DrawRectangle(
                            pen,
                            new Rectangle(
                                selectedTile.X - borderSize / 2,
                                selectedTile.Y - borderSize / 2,
                                selectedTile.Width + borderSize,
                                selectedTile.Height + borderSize
                            )
                        );
                    }
                }
            };

            tilesetTilesPictureBox.MouseMove += (sender, e) =>
            {
                if (tileset != null)
                {
                    foreach (Tile tile in tileset.Tiles)
                    {
                        if (tile.IsPointInTile(e.Location))
                        {
                            Cursor = Cursors.Hand;
                            return;
                        }
                    }
                    Cursor = Cursors.Default;
                }
            };

            tilesetTilesPictureBox.MouseLeave += (sender, e) =>
            {
                Cursor = Cursors.Default;
            };

            tilesetTilesPictureBox.MouseClick += (sender, e) =>
            {
                if (tileset != null)
                {
                    foreach (Tile tile in tileset.Tiles)
                    {
                        if (tile.IsPointInTile(e.Location))
                        {
                            Cursor = Cursors.Hand;
                            SelectedTile = tile;
                            tilesetTilesPictureBox.Invalidate();
                            return;
                        }
                    }
                    Cursor = Cursors.Default;
                }
            };

            tilesetTilesPictureBox.MouseDown += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    foreach (Tile tile in tileset.Tiles)
                    {
                        if (tile.IsPointInTile(e.Location))
                        {
                            SelectedTile = tile;
                            tilesetTilesPictureBox.Invalidate();
                            return;
                        }
                    }
                }
            };


            Label nameLabel = new Label
            {
                Text = "Name:",
                Location = new Point(5, 12),
                AutoSize = true
            };
            nameTextbox = new TextBox
            {
                Location = new Point(50, 10)
            };
            nameTextbox.Width = Math.Max(tilePropertiesPanel.ClientSize.Width - nameTextbox.Left - 5, 100);
            tilePropertiesPanel.Controls.Add(nameLabel);
            tilePropertiesPanel.Controls.Add(nameTextbox);

            Label tileTypeLabel = new Label
            {
                Text = "Type:",
                Location = new Point(5, 42),
                AutoSize = true
            };
            tileTypeComboBox = new ComboBox
            {
                Location = new Point(50, 40),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            tileTypeComboBox.Items.Add("PASSABLE");
            tileTypeComboBox.Items.Add("NOT PASSABLE");
            tileTypeComboBox.Width = Math.Max(tilePropertiesPanel.ClientSize.Width - tileTypeComboBox.Left - 5, 100);
            tilePropertiesPanel.Controls.Add(tileTypeLabel);
            tilePropertiesPanel.Controls.Add(tileTypeComboBox);

            //Label rowLabel = new Label
            //{
            //    Text = "Row:",
            //    Location = new Point(5, 42),
            //    AutoSize = true
            //};
            //rowInput = new NumericUpDown
            //{
            //    Location = new Point(50, 40)
            //};
            //rowInput.Width = Math.Max(tilePropertiesPanel.ClientSize.Width - rowInput.Left - 5, 50);
            //tilePropertiesPanel.Controls.Add(rowLabel);
            //tilePropertiesPanel.Controls.Add(rowInput);

            //Label columnLabel = new Label
            //{
            //    Text = "Col:",
            //    Location = new Point(5, 62),
            //    AutoSize = true
            //};
            //columnInput = new NumericUpDown
            //{
            //    Location = new Point(50, 60)
            //};
            //columnInput.Width = Math.Max(tilePropertiesPanel.ClientSize.Width - columnInput.Left - 5, 50);
            //tilePropertiesPanel.Controls.Add(columnLabel);
            //tilePropertiesPanel.Controls.Add(columnInput);

            Label previewLabel = new Label
            {
                Text = "Preview:",
                Location = new Point(5, 72),
                AutoSize = true
            };
            tilePreviewPictureBox = new PictureBox();
            tilePreviewPictureBox.BackColor = Color.Black;
            tilePreviewPictureBox.Location = new Point(5, 92);
            tilePropertiesPanel.Controls.Add(previewLabel);
            tilePropertiesPanel.Controls.Add(tilePreviewPictureBox);

            tilePreviewPictureBox.Paint += (sender, e) =>
            {
                if (selectedTile != null)
                {
                    e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    for (int i = 0; i < tileLayerPreviews.Count; i++)
                    {
                        e.Graphics.DrawImage(tileLayerPreviews[i][currentFrameIndexes[i]].image, new Rectangle(0, 0, tileset.TilesetScaledWidth, tileset.TilesetScaledHeight));
                    }
                }
            };

            tilePropertiesPanel.Resize += (sender, e) =>
            {
                nameTextbox.Width = Math.Max(tilePropertiesPanel.ClientSize.Width - nameTextbox.Left - 5, 100);
                tileTypeComboBox.Width = Math.Max(tilePropertiesPanel.ClientSize.Width - tileTypeComboBox.Left - 5, 100);

                //rowInput.Width = Math.Max(tilePropertiesPanel.ClientSize.Width - rowInput.Left - 5, 50);
                //columnInput.Width = Math.Max(tilePropertiesPanel.ClientSize.Width - columnInput.Left - 5, 50);
            };


            tilesetListBox.SelectedIndex = 0;

            if (tileset.Tiles.Length > 0)
            {
                SelectedTile = tileset.Tiles[0];
            }
        }

        private void SetupTilePanel()
        {
            int tileSpacing = tileset.TileScale + 2;
            int numberOfColumns = Math.Max((tilesetTilesPictureBoxPanel.ClientSize.Width - tileSpacing) / (tileset.TilesetScaledWidth + tileSpacing), 1);
            int numberOfRows = (int)Math.Ceiling(tileset.NumberOfTiles / (float)numberOfColumns);

            // set location of tiles in tile picker
            for (int i = 0; i < tileset.Tiles.Length; i++)
            {
                Tile tile = tileset.Tiles[i];
                int row = i / numberOfColumns;
                int column = i % numberOfColumns;
                tile.SetLocation(column * tileset.TilesetScaledWidth + (column * tileSpacing) + tileSpacing, row * tileset.TilesetScaledHeight + (row * tileSpacing) + tileSpacing);
                tile.SetDimensions(tileset.TilesetScaledWidth, tileset.TilesetScaledHeight);
            }

            tilesetTilesPictureBox.Image = new Bitmap(tilesetTilesPictureBoxPanel.ClientSize.Width, numberOfRows * tileset.TilesetScaledHeight + (numberOfRows * tileSpacing) + tileSpacing);
        }

        private void OnTileSelected()
        {
            nameTextbox.Text = selectedTileData.Name;

            switch (selectedTileData.TileType)
            {
                case "NOT_PASSABLE":
                    tileTypeComboBox.SelectedItem = "NOT PASSABLE";
                    break;
                case "PASSABLE":
                default:
                    tileTypeComboBox.SelectedItem = "PASSABLE";
                    break;
            }

            tileLayerPreviews = new List<List<(FrameData frameData, Bitmap image)>>();
            currentFrameIndexes = new List<int>();
            animationTimers = new List<System.Windows.Forms.Timer>();
            frameTimers = new List<int>();

            for (int i = 0; i < selectedTileData.Layers.Count; i++)
            {
                LayerData layerData = selectedTileData.Layers[i];
                List<(FrameData frameData, Bitmap image)> layerFrames = new List<(FrameData frameData, Bitmap image)>();

                foreach (FrameData frameData in layerData.Frames)
                {
                    Bitmap layerImage = ImageUtils.MakeColorTransparent(tileset.GetTileSubImage(frameData.Row, frameData.Column), Color.Magenta);
                    if (frameData.SpriteEffect != null)
                    {
                        switch (frameData.SpriteEffect)
                        {
                            case "FLIP_HORIZONTALLY":
                                layerImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                break;
                            case "FLIP VERTICALLY":
                                layerImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                break;
                        }
                    }
                    layerFrames.Add((frameData, layerImage));
                }
                tileLayerPreviews.Add(layerFrames);
                currentFrameIndexes.Add(0);
                frameTimers.Add(0);

                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                timer.Interval = 240; // closest I can get to 60 fps

                int layerIndex = i; // this is needed to prevent i from increasing by 1 when going into the tick method later, don't ask I don't really understand why it does that
                timer.Tick += (sender, e) =>
                {
                    int? delay = tileLayerPreviews[layerIndex][currentFrameIndexes[layerIndex]].frameData.Delay;
                    if (delay.HasValue)
                    {
                        if (delay.Value > frameTimers[layerIndex])
                        {
                            currentFrameIndexes[layerIndex]++;
                            if (currentFrameIndexes[layerIndex] >= tileLayerPreviews[layerIndex].Count)
                            {
                                currentFrameIndexes[layerIndex] = 0;
                                frameTimers[layerIndex] = 0;
                            }
                            tilePreviewPictureBox.Refresh();
                        }
                        else
                        {
                            frameTimers[layerIndex]++;
                        }
                    }
                };
                animationTimers.Add(timer);
                
            }

            foreach (System.Windows.Forms.Timer timer in animationTimers)
            {
                timer.Start();
            }

            tilePreviewPictureBox.Size = new Size(tileset.TilesetScaledWidth, tileset.TilesetScaledHeight);
            tilePreviewPictureBox.Image = new Bitmap(tileset.TilesetScaledWidth, tileset.TilesetScaledHeight);

        }

    }
}
