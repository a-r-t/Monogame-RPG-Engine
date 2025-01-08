using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace src.Engine.Scene.TilesetCore
{
    public class TilesetDataFile
    {
        [JsonPropertyName("properties")]
        public PropertiesData Properties { get; set; }

        [JsonPropertyName("tiles")]
        public List<TileData> Tiles { get; set; }

        public class BoundsData
        {
            [JsonPropertyName("x")]
            public int X { get; set; }

            [JsonPropertyName("y")]
            public int Y { get; set; }

            [JsonPropertyName("width")]
            public int Width { get; set; }

            [JsonPropertyName("height")]
            public int Height { get; set; }
        }

        public class FrameData
        {
            [JsonPropertyName("row")]
            public int Row { get; set; }

            [JsonPropertyName("col")]
            public int Column { get; set; }

            [JsonPropertyName("effect")]
            public string SpriteEffect { get; set; }

            [JsonPropertyName("delay")]
            public int? Delay { get; set; }
        }

        public class LayerData
        {
            [JsonPropertyName("layer")]
            public int Layer { get; set; }

            [JsonPropertyName("frames")]
            public List<FrameData> Frames { get; set; }
        }

        public class PropertiesData
        {
            [JsonPropertyName("tileWidth")]
            public int TileWidth { get; set; }

            [JsonPropertyName("tileHeight")]
            public int TileHeight { get; set; }

            [JsonPropertyName("tileScale")]
            public int TileScale { get; set; }

            [JsonPropertyName("tilesetImage")]
            public string TilesetImagePath { get; set; }
        }

        public class TileData
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("layers")]
            public List<LayerData> Layers { get; set; }

            [JsonPropertyName("tileType")]
            public string TileType { get; set; }

            [JsonPropertyName("bounds")]
            public BoundsData Bounds { get; set; }
        }
    }
}
