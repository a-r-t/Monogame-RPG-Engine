using MapEditor.src.Models;
using MapEditor.src.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        private Tileset Tileset
        {
            get
            {
                return tileset;
            }
            set
            {
                tileset = value;
                OnTilesetSelected();
            }
        }
        private PictureBox tilesetTilesPictureBox;

        private Panel tilesetTilesPictureBoxPanel;
        private Panel tilePropertiesPanel;
        private Panel tilePreviewPanel;

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
                    selectedTileData = Tileset.TilesetDataFile.Tiles[value.Index];
                    OnTileSelected();
                }
            }
        }
        private TileData selectedTileData;

        private TextBox nameTextbox;
        private ComboBox tileTypeComboBox;

        // list of layers, each layer then has a list of each frame of the bitmap
        private List<List<(FrameData frameData, Bitmap image)>> tileLayerPreviews;
        private List<int> currentFrameIndexes;
        private List<System.Windows.Forms.Timer> animationTimers = new List<System.Windows.Forms.Timer>();
        private List<int> frameTimers;

        private PictureBox tilePreviewPictureBox;
        private ComboBox previewComboBox;
        private CheckBox showBoundsCheckBox;
        private bool showBoundsInPreview;
        private Color selectedBoundsColor;
        private ComboBox boundsColorComboBox;
        private Button removeTile;
        private NumericUpDown tileIndexInput;

        private const int MAX_LAYER_COUNT = 2;
        private Label numberOfLayersLabel;
        private Label selectedLayerLabel;

        private Button addLayerButton;
        private Button removeLayerButton;
        private NumericUpDown selectLayerInput;
        private int selectedLayer;

        private Label numberOfFramesLabel;
        private NumericUpDown selectFrameInput;
        private Button addFrameButton;
        private Button removeFrameButton;
        private int selectedFrame;

        private NumericUpDown rowInput;
        private NumericUpDown columnInput;
        private Button selectGraphicButton;
        private ComboBox imageEffectComboBox;
        private NumericUpDown delayInput;


        public TilesetEditor()
        {
            InitializeComponent();

            // had to partially use the designer view to use split containers, so this is just an alias to make the code easier to read since I can't rename the panels in the split containers
            tilesetTilesPictureBoxPanel = splitContainer3.Panel1;
            tilePropertiesPanel = splitContainer2.Panel2;
            tilePreviewPanel = splitContainer3.Panel2;


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
                Tileset = new Tileset(selectedTileset.Value);

                SetupTilePanel();
            };

            tilesetTilesPictureBoxPanel.BackColor = Color.Black;
            tilesetTilesPictureBoxPanel.AutoScroll = true;
            tilesetTilesPictureBoxPanel.Resize += (sender, e) =>
            {
                if (Tileset != null)
                {
                    SetupTilePanel();
                    tilesetTilesPictureBoxPanel.Invalidate();
                }
            };

            tilesetTilesPictureBox = new PictureBox();
            tilesetTilesPictureBox.Location = new Point(0, 0);
            tilesetTilesPictureBox.BackColor = Config.TransparentColor;
            tilesetTilesPictureBoxPanel.Controls.Add(tilesetTilesPictureBox);
            tilesetTilesPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;

            tilesetTilesPictureBox.Paint += (sender, e) =>
            {
                if (Tileset != null)
                {
                    e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                    // paint tile picker tiles
                    for (int i = 0; i < Tileset.Tiles.Length; i++)
                    {
                        Tile tile = Tileset.Tiles[i];
                        tile.Paint(e.Graphics);
                    }

                    // paint yellow rectangle around selected tile
                    if (selectedTile != null)
                    {
                        int borderSize = Tileset.TileScale + 2;
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
                if (Tileset != null)
                {
                    foreach (Tile tile in Tileset.Tiles)
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
                if (Tileset != null)
                {
                    foreach (Tile tile in Tileset.Tiles)
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
                    foreach (Tile tile in Tileset.Tiles)
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


            Label tileIndexLabel = new Label()
            {
                Text = "Index:",
                Location = new Point(5, 74),
                AutoSize = true
            };

            tileIndexInput = new NumericUpDown()
            {
                Location = new Point(50, 71),
                Width = 60
            };
            tilePropertiesPanel.Controls.Add(tileIndexLabel);
            tilePropertiesPanel.Controls.Add(tileIndexInput);
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
                Text = "Preview Mode:",
                Location = new Point(5, 10),
                AutoSize = true
            };
            tilePreviewPictureBox = new PictureBox();
            tilePreviewPictureBox.BackColor = Config.TransparentColor;
            tilePreviewPictureBox.Location = new Point(tilePreviewPanel.Width / 2 - tilePreviewPictureBox.Width / 2, 100);

            tilePreviewPictureBox.Paint += (sender, e) =>
            {
                if (selectedTile != null)
                {
                    e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                    string previewSelection = previewComboBox.SelectedItem.ToString();
                    if (previewSelection == "Full")
                    {
                        for (int i = 0; i < tileLayerPreviews.Count; i++)
                        {
                            e.Graphics.DrawImage(tileLayerPreviews[i][currentFrameIndexes[i]].image, new Rectangle(0, 0, Tileset.TilesetScaledWidth, Tileset.TilesetScaledHeight));
                        }
                    }
                    else if (previewSelection == "Layer")
                    {
                        e.Graphics.DrawImage(tileLayerPreviews[selectedLayer][currentFrameIndexes[selectedLayer]].image, new Rectangle(0, 0, Tileset.TilesetScaledWidth, Tileset.TilesetScaledHeight));
                    }
                    else if (previewSelection == "Frame")
                    {
                        e.Graphics.DrawImage(tileLayerPreviews[selectedLayer][selectedFrame].image, new Rectangle(0, 0, Tileset.TilesetScaledWidth, Tileset.TilesetScaledHeight));
                    }

                    if (showBoundsInPreview && selectedTileData.TileType == "NOT_PASSABLE")
                    {
                        if (selectedTileData.Bounds != null)
                        {
                            e.Graphics.FillRectangle(new SolidBrush(selectedBoundsColor), new Rectangle(selectedTileData.Bounds.X * Tileset.TileScale, selectedTileData.Bounds.Y * Tileset.TileScale, selectedTileData.Bounds.Width * Tileset.TileScale, selectedTileData.Bounds.Height * Tileset.TileScale));
                        }
                        else
                        {
                            e.Graphics.FillRectangle(new SolidBrush(selectedBoundsColor), new Rectangle(0, 0, Tileset.TilesetScaledWidth, Tileset.TilesetScaledHeight));
                        }
                    }
                }
            };

            previewComboBox = new ComboBox()
            {
                Location = new Point(94, 7),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 80
            };
            previewComboBox.Items.AddRange(new string[] { "Full", "Layer", "Frame" });
            //previewComboBox.Width = Math.Max(tilePropertiesPanel.ClientSize.Width - previewComboBox.Left - 5, 50);

            previewComboBox.SelectedIndexChanged += (sender, e) =>
            {
                currentFrameIndexes = currentFrameIndexes.Select(frameIndex => 0).ToList();
                tilePreviewPictureBox.Invalidate();
            };

            showBoundsCheckBox = new CheckBox()
            {
                Text = "Show Bounds",
                Location = new Point(5, 40),
                CheckAlign = ContentAlignment.MiddleLeft,
                Width = 100
            };
            showBoundsCheckBox.CheckedChanged += (sender, e) =>
            {
                showBoundsInPreview = showBoundsCheckBox.Checked;
                tilePreviewPictureBox.Invalidate();
            };

            boundsColorComboBox = new ComboBox()
            {
                Location = new Point(108, 40),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 66
            };
            boundsColorComboBox.Items.AddRange(new string[] { "Red", "Blue", "Green" });
            boundsColorComboBox.SelectedIndexChanged += (sender, e) =>
            {
                switch (boundsColorComboBox.Items[boundsColorComboBox.SelectedIndex] as string)
                {
                    case "Red":
                        selectedBoundsColor = Color.FromArgb(100, 255, 0, 0);
                        break;
                    case "Blue":
                        selectedBoundsColor = Color.FromArgb(100, 0, 0, 255);
                        break;
                    case "Green":
                        selectedBoundsColor = Color.FromArgb(100, 0, 255, 0);
                        break;
                }
                tilePreviewPictureBox.Invalidate();
            };

            tilePreviewPanel.Controls.Add(previewLabel);
            tilePreviewPanel.Controls.Add(tilePreviewPictureBox);
            tilePreviewPanel.Controls.Add(previewComboBox);
            tilePreviewPanel.Controls.Add(showBoundsCheckBox);
            tilePreviewPanel.Controls.Add(boundsColorComboBox);



            GroupBox layerGroupBox = new GroupBox()
            {
                Text = "Layers",
                Location = new Point(5, 172),
                Size = new Size(150, 170)
            };

            numberOfLayersLabel = new Label
            {
                Text = "Number of Layers:",
                Location = new Point(5, 25),
                AutoSize = true
            };
            selectedLayer = 0;

            Label selectedLayerLabel = new Label
            {
                Text = "Selected Layer:",
                Location = new Point(5, 54),
                AutoSize = true
            };
            selectLayerInput = new NumericUpDown()
            {
                Location = new Point(95, 52),
                Width = 50
            };
            //selectLayerInput.Width = Math.Max(layerGroupBox.ClientSize.Width - selectLayerInput.Left - 5, 50);

            selectLayerInput.ValueChanged += (sender, e) =>
            {
                selectedLayer = (int)selectLayerInput.Value - 1;
                tilePreviewPictureBox.Invalidate();
            };

            addLayerButton = new Button()
            {
                Text = "Add Layer",
                Size = new Size(100, 30),
                Location = new Point(5, 89)
            };
            removeLayerButton = new Button()
            {
                Text = "Remove Layer",
                Size = new Size(100, 30),
                Location = new Point(5, 128)
            };


            layerGroupBox.Controls.Add(selectedLayerLabel);
            layerGroupBox.Controls.Add(selectLayerInput);
            layerGroupBox.Controls.Add(numberOfLayersLabel);
            layerGroupBox.Controls.Add(addLayerButton);
            layerGroupBox.Controls.Add(removeLayerButton);
            tilePropertiesPanel.Controls.Add(layerGroupBox);


            GroupBox frameGroupBox = new GroupBox()
            {
                Text = "Frames",
                Location = new Point(5, 372),
                Size = new Size(150, 170)
            };

            numberOfFramesLabel = new Label
            {
                Text = "Number of Frames:",
                Location = new Point(5, 25),
                AutoSize = true
            };
            selectedFrame = 0;

            Label selectedFrameLabel = new Label
            {
                Text = "Selected Frame:",
                Location = new Point(5, 54),
                AutoSize = true
            };
            selectFrameInput = new NumericUpDown()
            {
                Location = new Point(95, 52),
                Width = 50
            };
            //selectFrameInput.Width = Math.Max(frameGroupBox.ClientSize.Width - selectFrameInput.Left - 5, 50);

            selectFrameInput.ValueChanged += (sender, e) =>
            {
                selectedFrame = (int)selectFrameInput.Value - 1;
                tilePreviewPictureBox.Invalidate();
            };

            addFrameButton = new Button()
            {
                Text = "Add Frame",
                Size = new Size(100, 30),
                Location = new Point(5, 89)
            };
            removeFrameButton = new Button()
            {
                Text = "Remove Frame",
                Size = new Size(100, 30),
                Location = new Point(5, 128)
            };


            frameGroupBox.Controls.Add(selectedFrameLabel);
            frameGroupBox.Controls.Add(selectFrameInput);
            frameGroupBox.Controls.Add(numberOfFramesLabel);
            frameGroupBox.Controls.Add(addFrameButton);
            frameGroupBox.Controls.Add(removeFrameButton);
            tilePropertiesPanel.Controls.Add(frameGroupBox);

            tilePropertiesPanel.Resize += (sender, e) =>
            {
                nameTextbox.Width = Math.Max(tilePropertiesPanel.ClientSize.Width - nameTextbox.Left - 5, 100);
                tileTypeComboBox.Width = Math.Max(tilePropertiesPanel.ClientSize.Width - tileTypeComboBox.Left - 5, 100);
                //previewComboBox.Width = Math.Max(tilePropertiesPanel.ClientSize.Width - previewComboBox.Left - 5, 50);

                //selectLayerInput.Width = Math.Max(tilePropertiesPanel.ClientSize.Width - selectLayerInput.Left - 5, 50);

                //rowInput.Width = Math.Max(tilePropertiesPanel.ClientSize.Width - rowInput.Left - 5, 50);
                //columnInput.Width = Math.Max(tilePropertiesPanel.ClientSize.Width - columnInput.Left - 5, 50);

            };

            tilePreviewPanel.Resize += (sender, e) =>
            {
                tilePreviewPictureBox.Location = new Point(tilePreviewPanel.Width / 2 - tilePreviewPictureBox.Width / 2, tilePreviewPictureBox.Location.Y);

            };


            tilesetListBox.SelectedIndex = 0;
            boundsColorComboBox.SelectedIndex = 0;


        }

        private void OnTilesetSelected()
        {
            if (Tileset.Tiles.Length > 0)
            {
                SelectedTile = Tileset.Tiles[0];
            }

            tileIndexInput.Minimum = 1;
            tileIndexInput.Maximum = Tileset.Tiles.Length;
        }

        private void SetupTilePanel()
        {
            int tileSpacing = Tileset.TileScale + 2;
            int numberOfColumns = Math.Max((tilesetTilesPictureBoxPanel.ClientSize.Width - tileSpacing) / (Tileset.TilesetScaledWidth + tileSpacing), 1);
            int numberOfRows = (int)Math.Ceiling(Tileset.NumberOfTiles / (float)numberOfColumns);

            // set location of tiles in tile picker
            for (int i = 0; i < Tileset.Tiles.Length; i++)
            {
                Tile tile = Tileset.Tiles[i];
                int row = i / numberOfColumns;
                int column = i % numberOfColumns;
                tile.SetLocation(column * Tileset.TilesetScaledWidth + (column * tileSpacing) + tileSpacing, row * Tileset.TilesetScaledHeight + (row * tileSpacing) + tileSpacing);
                tile.SetDimensions(Tileset.TilesetScaledWidth, Tileset.TilesetScaledHeight);
            }

            tilesetTilesPictureBox.Image = new Bitmap(tilesetTilesPictureBoxPanel.ClientSize.Width, numberOfRows * Tileset.TilesetScaledHeight + (numberOfRows * tileSpacing) + tileSpacing);
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
                    Bitmap layerImage = ImageUtils.MakeColorTransparent(Tileset.GetTileSubImage(frameData.Row, frameData.Column), Config.TransparentColor);
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

            tilePreviewPictureBox.Size = new Size(Tileset.TilesetScaledWidth, Tileset.TilesetScaledHeight);
            tilePreviewPictureBox.Image = new Bitmap(Tileset.TilesetScaledWidth, Tileset.TilesetScaledHeight);
            tilePreviewPictureBox.Location = new Point((tilePreviewPanel.ClientSize.Width / 2) - (tilePreviewPictureBox.Width / 2), tilePreviewPictureBox.Location.Y);

            previewComboBox.SelectedItem = "Full";
            numberOfLayersLabel.Text = $"Number of Layers: {tileLayerPreviews.Count}";
            selectedLayer = 0;
            selectLayerInput.Minimum = 1;
            selectLayerInput.Maximum = tileLayerPreviews.Count;

            numberOfFramesLabel.Text = $"Number of Frames: {tileLayerPreviews[selectedLayer].Count}";
            selectedFrame = 0;
            selectFrameInput.Minimum = 1;
            selectFrameInput.Maximum = tileLayerPreviews[selectedLayer].Count;

            tileIndexInput.Value = SelectedTile.Index + 1;


        }

    }
}
