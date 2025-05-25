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

        private int selectedTileIndex;
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
                    OnTileSelected();
                }
            }
        }
        private TileData SelectedTileData
        {
            get
            {
                return Tileset.TilesetDataFile.Tiles[selectedTileIndex];
            }
        }

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

        private NumericUpDown boundsXInput;
        private NumericUpDown boundsYInput;
        private NumericUpDown boundsWidthInput;
        private NumericUpDown boundsHeightInput;

        private Label numberOfLayersLabel;
        private Button addLayerButton;
        private Button removeLayerButton;
        private NumericUpDown selectLayerInput;
        private int selectedLayer;

        private Label numberOfFramesLabel;
        private NumericUpDown selectFrameInput;
        private Button addFrameButton;
        private Button removeFrameButton;
        private int selectedFrame;

        private Button selectFrameGraphicButton;
        private ComboBox imageEffectComboBox;
        private string selectedImageEffect;
        private NumericUpDown delayInput;

        private Dictionary<int, int> tileIndexChanges;


        public TilesetEditor()
        {
            InitializeComponent();

            // had to partially use the designer view to use split containers, so this is just an alias to make the code easier to read since I can't rename the panels in the split containers
            tilesetTilesPictureBoxPanel = splitContainer3.Panel1;
            tilePropertiesPanel = splitContainer2.Panel2;
            tilePreviewPanel = splitContainer3.Panel2;

            tileIndexChanges = new Dictionary<int, int>();

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
                    for (int i = 0; i < Tileset.Tiles.Length; i++)
                    {
                        Tile tile = Tileset.Tiles[i];
                        if (tile.IsPointInTile(e.Location))
                        {
                            Cursor = Cursors.Hand;
                            selectedTileIndex = i;
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
                    for (int i = 0; i < Tileset.Tiles.Length; i++)
                    {
                        Tile tile = Tileset.Tiles[i];
                        if (tile.IsPointInTile(e.Location))
                        {
                            Cursor = Cursors.Hand;
                            selectedTileIndex = i;
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
            nameTextbox.TextChanged += (sender, e) =>
            {
                tileset.TilesetDataFile.Tiles[selectedTileIndex].Name = nameTextbox.Text;
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
            tileTypeComboBox.Items.AddRange(new string[] { "PASSABLE", "NOT PASSABLE" });
            tileTypeComboBox.SelectedIndexChanged += (sender, e) =>
            {
                string selectedTileType = tileTypeComboBox.Items[tileTypeComboBox.SelectedIndex] as string;
                if (selectedTileType.Replace(" ", "_") != Tileset.TilesetDataFile.Tiles[selectedTileIndex].TileType)
                {
                    switch (tileTypeComboBox.Items[tileTypeComboBox.SelectedIndex])
                    {
                        case "PASSABLE":
                            Tileset.TilesetDataFile.Tiles[selectedTileIndex].TileType = "PASSABLE";
                            break;
                        case "NOT PASSABLE":
                            Tileset.TilesetDataFile.Tiles[selectedTileIndex].TileType = "NOT_PASSABLE";
                            break;
                    }
                    SetUpBoundsControls();
                    tilePreviewPictureBox.Invalidate();
                }
            };

            //tileTypeComboBox.Width = Math.Max(tilePropertiesPanel.ClientSize.Width - tileTypeComboBox.Left - 5, 100);
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
                Width = 60,
                DecimalPlaces = 0
            };
            tileIndexInput.ValueChanged += (sender, e) =>
            {
                int oldIndex = selectedTileIndex;
                int newIndex = (int)tileIndexInput.Value;

                if (oldIndex != newIndex)
                {

                    TileData currentTileData = tileset.TilesetDataFile.Tiles[oldIndex];
                    tileset.TilesetDataFile.Tiles.RemoveAt(oldIndex);

                    tileset.TilesetDataFile.Tiles.Insert(newIndex, currentTileData);

                    Tile currentTile = Tileset.Tiles[oldIndex];
                    currentTile.Index = newIndex;
                    List<Tile> tempTilesList = Tileset.Tiles.ToList();
                    tempTilesList.RemoveAt(oldIndex);
                    tempTilesList.Insert(newIndex, currentTile);

                    for (int i = 0; i < Tileset.Tiles.Length; i++)
                    {
                        Tileset.Tiles[i] = tempTilesList[i];
                        Tileset.Tiles[i].Index = i;
                    }

                    tileIndexChanges[oldIndex] = newIndex;

                    if (newIndex < oldIndex)
                    {
                        for (int i = newIndex + 1; i < oldIndex + 1; i++)
                        {
                            tileIndexChanges[i] = i + 1;
                        }
                    }
                    else if (newIndex > oldIndex)
                    {
                        for (int i = oldIndex + 1; i < newIndex; i++)
                        {
                            tileIndexChanges[i] = i - 1;
                        }
                    }
                    selectedTileIndex = newIndex;
                    SetupTilePanel();
                    tilesetTilesPictureBox.Invalidate();
                }
            };
            tilePropertiesPanel.Controls.Add(tileIndexLabel);
            tilePropertiesPanel.Controls.Add(tileIndexInput);

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

                    if (showBoundsInPreview && SelectedTileData.TileType == "NOT_PASSABLE")
                    {
                        if (SelectedTileData.Bounds != null)
                        {
                            e.Graphics.FillRectangle(new SolidBrush(selectedBoundsColor), new Rectangle(SelectedTileData.Bounds.X * Tileset.TileScale, SelectedTileData.Bounds.Y * Tileset.TileScale, SelectedTileData.Bounds.Width * Tileset.TileScale, SelectedTileData.Bounds.Height * Tileset.TileScale));
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

            GroupBox boundsGroupBox = new GroupBox()
            {
                Text = "Bounds",
                Location = new Point(5, 104),
                Size = new Size(150, 90)
            };

            tilePropertiesPanel.Controls.Add(boundsGroupBox);

            Label boundsXLabel = new Label
            {
                Text = "X:",
                Location = new Point(5, 25),
                AutoSize = true
            };
            boundsXInput = new NumericUpDown
            {
                Location = new Point(26, 22),
                Size = new Size(45, 50),
                DecimalPlaces = 0
            };
            boundsXInput.ValueChanged += (sender, e) =>
            {
                if (Tileset.TilesetDataFile.Tiles[selectedTileIndex].TileType == "NOT_PASSABLE")
                {
                    BoundsData boundsData = Tileset.TilesetDataFile.Tiles[selectedTileIndex].Bounds;
                    if (boundsData != null)
                    {
                        Tileset.TilesetDataFile.Tiles[selectedTileIndex].Bounds.X = (int)boundsXInput.Value;
                    }
                    else
                    {
                        Tileset.TilesetDataFile.Tiles[selectedTileIndex].Bounds = new BoundsData()
                        {
                            X = (int)boundsXInput.Value,
                            Y = 0,
                            Width = Tileset.TileWidth,
                            Height = Tileset.TileHeight
                        };
                    }
                    boundsWidthInput.Maximum = Tileset.TileWidth - Tileset.TilesetDataFile.Tiles[selectedTileIndex].Bounds.X;
                    tilePreviewPictureBox.Invalidate();
                }
            };
            boundsGroupBox.Controls.Add(boundsXLabel);
            boundsGroupBox.Controls.Add(boundsXInput);

            Label boundsYLabel = new Label
            {
                Text = "Y:",
                Location = new Point(80, 25),
                AutoSize = true
            };
            boundsYInput = new NumericUpDown
            {
                Location = new Point(101, 22),
                Size = new Size(45, 50),
                DecimalPlaces = 0
            };
            boundsYInput.ValueChanged += (sender, e) =>
            {
                if (Tileset.TilesetDataFile.Tiles[selectedTileIndex].TileType == "NOT_PASSABLE")
                {
                    BoundsData boundsData = Tileset.TilesetDataFile.Tiles[selectedTileIndex].Bounds;
                    if (boundsData != null)
                    {
                        Tileset.TilesetDataFile.Tiles[selectedTileIndex].Bounds.Y = (int)boundsYInput.Value;
                    }
                    else
                    {
                        Tileset.TilesetDataFile.Tiles[selectedTileIndex].Bounds = new BoundsData()
                        {
                            X = 0,
                            Y = (int)boundsYInput.Value,
                            Width = Tileset.TileWidth,
                            Height = Tileset.TileHeight
                        };
                    }
                    boundsHeightInput.Maximum = Tileset.TileHeight - Tileset.TilesetDataFile.Tiles[selectedTileIndex].Bounds.Y;
                    tilePreviewPictureBox.Invalidate();
                }
            };
            boundsGroupBox.Controls.Add(boundsYLabel);
            boundsGroupBox.Controls.Add(boundsYInput);

            Label boundsWidthLabel = new Label
            {
                Text = "W:",
                Location = new Point(5, 55),
                AutoSize = true
            };
            boundsWidthInput = new NumericUpDown
            {
                Location = new Point(26, 52),
                Size = new Size(45, 50),
                DecimalPlaces = 0
            };
            boundsWidthInput.ValueChanged += (sender, e) =>
            {
                if (Tileset.TilesetDataFile.Tiles[selectedTileIndex].TileType == "NOT_PASSABLE")
                {
                    BoundsData boundsData = Tileset.TilesetDataFile.Tiles[selectedTileIndex].Bounds;
                    if (boundsData != null)
                    {
                        Tileset.TilesetDataFile.Tiles[selectedTileIndex].Bounds.Width = (int)boundsWidthInput.Value;
                    }
                    else
                    {
                        Tileset.TilesetDataFile.Tiles[selectedTileIndex].Bounds = new BoundsData()
                        {
                            X = 0,
                            Y = 0,
                            Width = (int)boundsWidthInput.Value,
                            Height = Tileset.TileHeight
                        };
                    }
                    tilePreviewPictureBox.Invalidate();
                }
            };
            boundsGroupBox.Controls.Add(boundsWidthLabel);
            boundsGroupBox.Controls.Add(boundsWidthInput);

            Label boundsHeightLabel = new Label
            {
                Text = "H:",
                Location = new Point(80, 55),
                AutoSize = true
            };
            boundsHeightInput = new NumericUpDown
            {
                Location = new Point(101, 52),
                Size = new Size(45, 50),
                DecimalPlaces = 0
            };
            boundsHeightInput.ValueChanged += (sender, e) =>
            {
                if (Tileset.TilesetDataFile.Tiles[selectedTileIndex].TileType == "NOT_PASSABLE")
                {
                    BoundsData boundsData = Tileset.TilesetDataFile.Tiles[selectedTileIndex].Bounds;
                    if (boundsData != null)
                    {
                        Tileset.TilesetDataFile.Tiles[selectedTileIndex].Bounds.Height = (int)boundsHeightInput.Value;
                    }
                    else
                    {
                        Tileset.TilesetDataFile.Tiles[selectedTileIndex].Bounds = new BoundsData()
                        {
                            X = 0,
                            Y = 0,
                            Width = Tileset.TileWidth,
                            Height = (int)boundsHeightInput.Value
                        };
                    }
                    tilePreviewPictureBox.Invalidate();
                }
            };
            boundsGroupBox.Controls.Add(boundsHeightLabel);
            boundsGroupBox.Controls.Add(boundsHeightInput);

            GroupBox layerGroupBox = new GroupBox()
            {
                Text = "Layers",
                Location = new Point(5, 202),
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
                Width = 50,
                DecimalPlaces = 0
            };
            //selectLayerInput.Width = Math.Max(layerGroupBox.ClientSize.Width - selectLayerInput.Left - 5, 50);

            selectLayerInput.ValueChanged += (sender, e) =>
            {
                selectedLayer = (int)selectLayerInput.Value - 1;

                selectedFrame = 0;
                selectFrameInput.Minimum = 1;
                selectFrameInput.Maximum = tileLayerPreviews[selectedLayer].Count;
                selectFrameInput.Value = 1;

                SetUpDelayControls();
                SetUpImageEffectControls();
                tilePreviewPictureBox.Invalidate();
            };

            addLayerButton = new Button()
            {
                Text = "Add Layer",
                Size = new Size(100, 30),
                Location = new Point(5, 89)
            };
            addLayerButton.Click += (sender, e) =>
            {
                StopAnimationTimers();
                SelectedTileData.Layers.Add(new LayerData()
                {
                    Frames = new List<FrameData>()
                    {
                        new FrameData()
                        {
                            Row = 0,
                            Column = 0
                        }
                    }
                });
                OnTileSelected();
            };
            removeLayerButton = new Button()
            {
                Text = "Remove Layer",
                Size = new Size(100, 30),
                Location = new Point(5, 128)
            };
            removeLayerButton.Click += (sender, e) =>
            {
                int numberOfLayers = SelectedTileData.Layers.Count;
                if (numberOfLayers > 1)
                {
                    StopAnimationTimers();
                    SelectedTileData.Layers.RemoveAt(selectedLayer);
                    if (selectedLayer > numberOfLayers - 1)
                    {
                        selectedLayer--;
                    }
                    OnTileSelected();
                }
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
                Location = new Point(5, 380),
                Size = new Size(150, 278)
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
                Width = 50,
                DecimalPlaces = 0
            };
            //selectFrameInput.Width = Math.Max(frameGroupBox.ClientSize.Width - selectFrameInput.Left - 5, 50);

            selectFrameInput.ValueChanged += (sender, e) =>
            {
                selectedFrame = (int)selectFrameInput.Value - 1;

                SetUpDelayControls();
                SetUpImageEffectControls();

                tilePreviewPictureBox.Invalidate();
            };

            frameGroupBox.Controls.Add(selectedFrameLabel);
            frameGroupBox.Controls.Add(selectFrameInput);
            frameGroupBox.Controls.Add(numberOfFramesLabel);

            Label delayLabel = new Label
            {
                Text = "Delay:",
                Location = new Point(5, 88),
                AutoSize = true
            };
            delayInput = new NumericUpDown
            {
                Location = new Point(50, 86),
                Width = 50,
                DecimalPlaces = 0,
                Minimum = 0,
                Maximum = decimal.MaxValue
            };
            delayInput.ValueChanged += (sender, e) =>
            {
                int? oldDelay = SelectedTileData.Layers[selectedLayer].Frames[selectedFrame].Delay;
                if (!oldDelay.HasValue || oldDelay != (int)delayInput.Value)
                {
                    SelectedTileData.Layers[selectedLayer].Frames[selectedFrame].Delay = (int)delayInput.Value;
                    if (SelectedTileData.Layers[selectedLayer].Frames.Count > 1)
                    {
                        OnTileSelected();
                    }
                }
            };
            frameGroupBox.Controls.Add(delayLabel);
            frameGroupBox.Controls.Add(delayInput);

            Label imageEffectLabel = new Label
            {
                Text = "Effect:",
                Location = new Point(5, 120),
                AutoSize = true
            };
            imageEffectComboBox = new ComboBox
            {
                Location = new Point(50, 118),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 70
            };
            imageEffectComboBox.Items.AddRange(new string[] { "NONE", "FLIP H", "FLIP V", "FLIP HV" });
            imageEffectComboBox.SelectedIndexChanged += (sender, e) =>
            {
                string oldImageEffect = SelectedTileData.Layers[selectedLayer].Frames[selectedFrame].SpriteEffect;
                string newImageEffect = ConvertImageEffectText(imageEffectComboBox.Items[imageEffectComboBox.SelectedIndex] as string);
                if (oldImageEffect == null || oldImageEffect != newImageEffect)
                {
                    SelectedTileData.Layers[selectedLayer].Frames[selectedFrame].SpriteEffect = newImageEffect;
                    OnTileSelected();
                }

            };
            frameGroupBox.Controls.Add(imageEffectLabel);
            frameGroupBox.Controls.Add(imageEffectComboBox);

            selectFrameGraphicButton = new Button()
            {
                Text = "Choose Graphic",
                Size = new Size(100, 30),
                Location = new Point(5, 161)
            };
            selectFrameGraphicButton.Click += (e, sender) =>
            {
                TileGraphicChooserForm tileGraphicChooserForm = new TileGraphicChooserForm();
                tileGraphicChooserForm.Tileset = Tileset;
                int row = SelectedTileData.Layers[selectedLayer].Frames[selectedFrame].Row;
                int column = SelectedTileData.Layers[selectedLayer].Frames[selectedFrame].Column;
                tileGraphicChooserForm.SelectedTileLocation = (row, column);

                DialogResult tileGraphicChooserFormResult = tileGraphicChooserForm.ShowDialog();
                if (tileGraphicChooserFormResult == DialogResult.OK)
                {
                    SelectedTileData.Layers[selectedLayer].Frames[selectedFrame].Row = tileGraphicChooserForm.SelectedTileLocation.Row;
                    SelectedTileData.Layers[selectedLayer].Frames[selectedFrame].Column = tileGraphicChooserForm.SelectedTileLocation.Column;
                    Bitmap tileImage = Tileset.CreateTileImage(SelectedTileData);
                    Tileset.Tiles[selectedTileIndex].Image = tileImage;
                    tilesetTilesPictureBox.Invalidate();
                    OnTileSelected();
                }
                tileGraphicChooserForm.Dispose();
            };
            addFrameButton = new Button()
            {
                Text = "Add Frame",
                Size = new Size(100, 30),
                Location = new Point(5, 200)
            };
            addFrameButton.Click += (sender, e) =>
            {
                StopAnimationTimers();
                SelectedTileData.Layers[selectedLayer].Frames.Add(new FrameData()
                {
                    Row = 0,
                    Column = 0
                });
                OnTileSelected();
            };
            removeFrameButton = new Button()
            {
                Text = "Remove Frame",
                Size = new Size(100, 30),
                Location = new Point(5, 239)
            };
            removeFrameButton.Click += (sender, e) =>
            {
                int numberOfFrames = SelectedTileData.Layers[selectedLayer].Frames.Count;
                if (numberOfFrames > 1)
                {
                    StopAnimationTimers();
                    SelectedTileData.Layers[selectedLayer].Frames.RemoveAt(selectedFrame);
                    if (selectedFrame > numberOfFrames - 1)
                    {
                        selectedFrame--;
                    }
                    OnTileSelected();
                }
            };

            frameGroupBox.Controls.Add(selectFrameGraphicButton);
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

            // have to manually paint a border around this panel because spilt container panels don't support borders natively
            tilePreviewPanel.Paint += (sender, e) =>
            {
                e.Graphics.DrawRectangle(new Pen(Color.Black), new Rectangle(0, 0, tilePreviewPanel.Width - 1, tilePreviewPanel.Height - 5));
            };


            tilesetListBox.SelectedIndex = 0;
            boundsColorComboBox.SelectedIndex = 0;

            splitContainer2.IsSplitterFixed = true;
            splitContainer2.FixedPanel = FixedPanel.Panel2;


        }

        private string ConvertImageEffectText(string imageEffectText)
        {
            switch (imageEffectText)
            {
                case "NONE":
                    return "NONE";
                case "FLIP H":
                    return "FLIP_HORIZONTALLY";
                case "FLIP V":
                    return "FLIP_VERTICALLY";
                case "FLIP HV":
                    return "FLIP_HORIZONTALLY_AND_VERTICALLY";
                default:
                    return null;
            }
        }

        private void OnTilesetSelected()
        {
            if (Tileset.Tiles.Length > 0)
            {
                selectedTileIndex = 0;
                SelectedTile = Tileset.Tiles[0];
            }

            tileIndexInput.Minimum = 0;
            tileIndexInput.Maximum = Tileset.Tiles.Length - 1;
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

        private void StopAnimationTimers()
        {
            foreach (System.Windows.Forms.Timer timer in animationTimers)
            {
                timer.Stop();
            }
        }

        private void OnTileSelected()
        {
            StopAnimationTimers();

            nameTextbox.Text = SelectedTileData.Name;

            switch (SelectedTileData.TileType)
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

            for (int i = 0; i < SelectedTileData.Layers.Count; i++)
            {
                LayerData layerData = SelectedTileData.Layers[i];
                List<(FrameData frameData, Bitmap image)> layerFrames = new List<(FrameData frameData, Bitmap image)>();

                foreach (FrameData frameData in layerData.Frames)
                {
                    Bitmap layerImage = ImageUtils.MakeColorTransparent(Tileset.GetTilesetGraphicSubImage(frameData.Row, frameData.Column), Config.TransparentColor);
                    if (frameData.SpriteEffect != null)
                    {
                        switch (frameData.SpriteEffect)
                        {
                            case "FLIP_HORIZONTALLY":
                                layerImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                break;
                            case "FLIP_VERTICALLY":
                                layerImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                break;
                            case "FLIP_HORIZONTALLY_AND_VERTICALLY":
                                layerImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
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
                timer.Interval = 17; // closest I can get to 60 fps

                int layerIndex = i; // this is needed to prevent i from increasing by 1 when going into the tick method later, don't ask I don't really understand why it does that
                timer.Tick += (sender, e) =>
                {
                    int? delay = tileLayerPreviews[layerIndex][currentFrameIndexes[layerIndex]].frameData.Delay;
                    if (delay.HasValue)
                    {
                        if (frameTimers[layerIndex] > delay.Value)
                        {
                            frameTimers[layerIndex] = 0;
                            currentFrameIndexes[layerIndex]++;
                            if (currentFrameIndexes[layerIndex] >= tileLayerPreviews[layerIndex].Count)
                            {
                                currentFrameIndexes[layerIndex] = 0;
                            }
                            tilePreviewPictureBox.Invalidate();
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

            int numberOfLayers = tileLayerPreviews.Count;
            numberOfLayersLabel.Text = $"Number of Layers: {numberOfLayers}";
            selectedLayer = 0;
            selectLayerInput.Minimum = 1;
            selectLayerInput.Maximum = numberOfLayers;
            selectLayerInput.Value = 1;

            int numberOfFrames = tileLayerPreviews[selectedLayer].Count;
            numberOfFramesLabel.Text = $"Number of Frames: {numberOfFrames}";
            selectedFrame = 0;
            selectFrameInput.Minimum = 1;
            selectFrameInput.Maximum = numberOfFrames;
            selectFrameInput.Value = 1;

            tileIndexInput.Value = SelectedTile.Index;

            removeLayerButton.Enabled = numberOfLayers > 1;
            removeFrameButton.Enabled = numberOfFrames > 1;

            SetUpBoundsControls();
            SetUpDelayControls();
            SetUpImageEffectControls();
        }

        private void SetUpBoundsControls()
        {
            if (SelectedTileData.TileType == "NOT_PASSABLE")
            {
                boundsXInput.Enabled = true;
                boundsYInput.Enabled = true;
                boundsWidthInput.Enabled = true;
                boundsHeightInput.Enabled = true;

                boundsXInput.Maximum = Tileset.TileWidth;
                boundsYInput.Maximum = Tileset.TileHeight;
                boundsXInput.Minimum = 0;
                boundsYInput.Minimum = 0;
                boundsWidthInput.Minimum = 0;
                boundsHeightInput.Minimum = 0;


                if (SelectedTileData.Bounds != null)
                {
                    boundsWidthInput.Maximum = Tileset.TileWidth - SelectedTileData.Bounds.X;
                    boundsHeightInput.Maximum = Tileset.TileHeight - SelectedTileData.Bounds.Y;

                    boundsXInput.Value = SelectedTileData.Bounds.X;
                    boundsYInput.Value = SelectedTileData.Bounds.Y;
                    boundsWidthInput.Value = SelectedTileData.Bounds.Width;
                    boundsHeightInput.Value = SelectedTileData.Bounds.Height;
                }
                else
                {
                    Tileset.TilesetDataFile.Tiles[selectedTileIndex].Bounds = new BoundsData()
                    {
                        X = 0,
                        Y = 0,
                        Width = Tileset.TileWidth,
                        Height = Tileset.TileHeight
                    };
                    boundsXInput.Value = 0;
                    boundsYInput.Value = 0;
                    boundsWidthInput.Maximum = Tileset.TileWidth;
                    boundsHeightInput.Maximum = Tileset.TileHeight;
                    boundsWidthInput.Value = Tileset.TileWidth;
                    boundsHeightInput.Value = Tileset.TileHeight;
                }

            }
            else
            {
                boundsXInput.Value = 0;
                boundsYInput.Value = 0;
                boundsWidthInput.Value = 0;
                boundsHeightInput.Value = 0;
                boundsXInput.Enabled = false;
                boundsYInput.Enabled = false;
                boundsWidthInput.Enabled = false;
                boundsHeightInput.Enabled = false;
            }
        }

        private void SetUpDelayControls()
        {
            int numberOfFrames = tileLayerPreviews[selectedLayer].Count;
            if (numberOfFrames > 1)
            {
                delayInput.Enabled = true;

                if (SelectedTileData.Layers[selectedLayer].Frames[selectedFrame].Delay != null)
                {
                    delayInput.Value = SelectedTileData.Layers[selectedLayer].Frames[selectedFrame].Delay.Value;
                }
                else
                {
                    delayInput.Value = 0;
                }
            }
            else
            {
                delayInput.Value = 0;
                delayInput.Enabled = false;
            }
        }

        private void SetUpImageEffectControls()
        {
            if (SelectedTileData.Layers[selectedLayer].Frames[selectedFrame].SpriteEffect != null)
            {
                switch (SelectedTileData.Layers[selectedLayer].Frames[selectedFrame].SpriteEffect)
                {
                    case "NONE":
                        imageEffectComboBox.SelectedItem = "NONE";
                        break;
                    case "FLIP_HORIZONTALLY":
                        imageEffectComboBox.SelectedItem = "FLIP H";
                        break;
                    case "FLIP_VERTICALLY":
                        imageEffectComboBox.SelectedItem = "FLIP V";
                        break;
                    case "FLIP_HORIZONTALLY_AND_VERTICALLY":
                        imageEffectComboBox.SelectedItem = "FLIP HV";
                        break;
                }
            }
            else
            {
                imageEffectComboBox.SelectedItem = "NONE";
            }
        }

        public void AddTile()
        {
            TileData tileData = new TileData();
            tileData.Name = "NEW_TILE";
            tileData.TileType = "PASSABLE";
            LayerData layerData = new LayerData();
            FrameData frameData = new FrameData();
            frameData.Row = 0;
            frameData.Column = 0;
            layerData.Frames = new List<FrameData>();
            layerData.Frames.Add(frameData);
            tileData.Layers = new List<LayerData>();
            tileData.Layers.Add(layerData);
            Tileset.TilesetDataFile.Tiles.Add(tileData);

            // reload tileset with change
            Tileset.LoadTileset();
            SetupTilePanel();

            tileIndexInput.Maximum = Tileset.Tiles.Length - 1;
        }

    }
}
