using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_RPG_Engine.App.Resources
{
    public static class SoundsHelper
    {
        public static class SoundEffects
        {
            private static readonly string pathPrefix = "Sounds/SoundEffects";
            public static readonly string MENU_SELECT = $"{pathPrefix}/menu-select";

        }

        public static class Songs
        {
            private static readonly string pathPrefix = "Sounds/Songs";
            public static readonly string MENU_BACKGROUND = $"{pathPrefix}/menu-music";
        }
    }
}
