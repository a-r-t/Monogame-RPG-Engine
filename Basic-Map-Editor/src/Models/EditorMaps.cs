using Monogame_RPG_Engine.App.Maps;
using Monogame_RPG_Engine.Engine.Core;
using Monogame_RPG_Engine.Engine.Scene.MapCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_Map_Editor.Models
{
    public class EditorMaps
    {
        public static List<string> GetMapNames()
        {
            return new List<string>() {
                "TestMap",
                "TitleScreen"
            };
        }

        public static Map GetMapByName(string mapName)
        {
            switch (mapName)
            {
                case "TestMap":
                    return new TestMap();
                case "TitleScreen":
                    return new TitleScreenMap();
                default:
                    throw new Exception("Unrecognized map name");
            }
        }
    }
}
