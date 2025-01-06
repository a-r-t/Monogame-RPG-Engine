using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor
{
    // this is really hacky, but basically I need these paths that go from the bin folder all the way to the other project in the solution so the map editor can actually access/edit the map files and such
    public static class Config
    {
        public static string GameMapFilesPath = @".\..\..\..\..\Monogame-RPG-Engine\Content\Resources\MapFiles";
        public static string TilesetFilesPath = @".\..\..\..\..\Monogame-RPG-Engine\Content\Resources\TilesetFiles";
        public static string GraphicsPath = @".\..\..\..\..\Monogame-RPG-Engine\Content\Resources\Graphics";
        public static Color TransparentColor = Color.Magenta;
    }
}
