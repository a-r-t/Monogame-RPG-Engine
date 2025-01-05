using Basic_Map_Editor.Models;
using Monogame_RPG_Engine.Engine.Core;
using Monogame_RPG_Engine.Engine.Scene.MapCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Basic_Map_Editor.Components
{
    public partial class EditorControlPanel : UserControl
    {
        private List<string> mapNames;
        private ComboBox mapNamesComboBox;
        private TilePicker tilePicker;
        private MapBuilder mapBuilder;
        private Map selectedMap;

        public EditorControlPanel(SelectedTileIndexHolder selectedTileIndexHolder, MapBuilder mapBuilder)
        {
            InitializeComponent();

            BackColor = Color.CornflowerBlue;
            Location = new Point(0, 0);

            mapNames = EditorMaps.getMapNames();

            this.mapBuilder = mapBuilder;

            Panel mapChoosePanel = new Panel();
            mapChoosePanel.Size = new Size(200, 80);
            mapChoosePanel.BackColor = Color.CornflowerBlue;

            Label mapLabel = new Label();
            mapLabel.Location = new Point(5, 0);
            mapLabel.Text = "Choose a Map:";
            mapLabel.Size = new Size(100, 40);
            mapChoosePanel.Controls.Add(mapLabel);

            mapNamesComboBox = new ComboBox();
            mapNamesComboBox.Size = new Size(190, 40);
            mapNamesComboBox.Location = new Point(5, 30);
            mapNames.Sort(StringComparer.OrdinalIgnoreCase);
            foreach (string mapName in mapNames)
            {
                mapNamesComboBox.Items.Add(mapName);
            }
            mapNamesComboBox.Click += (sender, e) => 
            {
                SetMap();
            };
            mapChoosePanel.Controls.Add(mapNamesComboBox);
            selectedMap = EditorMaps.getMapByName(mapNamesComboBox.SelectedItem.ToString());

            Controls.Add(mapChoosePanel);
            mapChoosePanel.Dock = DockStyle.Top;

            tilePicker = new TilePicker(selectedTileIndexHolder);
            Panel tilePickerScroll = new Panel();
            tilePickerScroll.setViewportView(tilePicker);
            tilePickerScroll.Location = new Point(5, 78);
            tilePickerScroll.Size = new Size(190, 394);
            tilePickerScroll.Controls.Add(tilePicker);
            Controls.Add(tilePickerScroll);
            tilePickerScroll.Dock = DockStyle.Fill;
            tilePicker.setTileset(GetSelectedMap(), GetSelectedMap().Tileset);


            Panel mapButtonsPanel = new Panel();
            mapButtonsPanel.Size = new Size(200, 95);
            mapButtonsPanel.BackColor = Color.CornflowerBlue;

            Button setMapDimensionsButton = new Button();
            setMapDimensionsButton.Size = new Size(190, 40);
            setMapDimensionsButton.Location = new Point(5, 5);
            setMapDimensionsButton.Text = "Set Map Dimensions";
            setMapDimensionsButton.Click += (e, sender) =>
            {
                new ChangeMapSizeWindow(getSelectedMap()).Show();
                mapBuilder.refreshTileBuilder();
            };
                
            mapButtonsPanel.Controls.Add(setMapDimensionsButton);

            Button saveMapButton = new Button();
            saveMapButton.Size = new Size(190, 40);
            saveMapButton.Location = new Point(5, 50);
            saveMapButton.Text = "Save Map";
            saveMapButton.Click += (e, sender) =>
            {
                WriteSelectedMapToFile();
            };

            mapButtonsPanel.Controls.Add(saveMapButton);

            Controls.Add(mapButtonsPanel);
            mapButtonsPanel.Dock = DockStyle.Bottom;
        }

        public Map GetSelectedMap()
        {
            return selectedMap;
        }

        public void WriteSelectedMapToFile()
        {
            Map map = GetSelectedMap();
            string fileName = GetSelectedMap().MapFileName;
            try
            {
                StreamWriter fileWriter = new StreamWriter(Config.MAP_FILES_PATH + fileName);
                fileWriter.Write(map.Width + " " + map.Height + "\n");
                MapTile[] mapTiles = map.MapTiles;
                for (int i = 0; i < map.Height; i++)
                {
                    for (int j = 0; j < map.Width; j++)
                    {
                        fileWriter.Write((mapTiles[j + map.Width * i].TileIndex).ToString());
                        if (j < map.Width - 1)
                        {
                            fileWriter.Write(" ");
                        }
                        else if (j >= map.Width - 1 && i < map.Height - 1)
                        {
                            fileWriter.Write("\n");
                        }
                    }
                }
                fileWriter.Close();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("Unable to save map file! That's really not great!");
            }
        }
        public void SetMap()
        {
            selectedMap = EditorMaps.getMapByName(mapNamesComboBox.SelectedItem.ToString());
            tilePicker.setTileset(selectedMap, selectedMap.Tileset);
            mapBuilder.setMap(selectedMap);
        }


    }
}
