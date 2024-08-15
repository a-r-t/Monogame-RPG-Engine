using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Resources
{
    public static class FontsHelper
    {
        public static class SpriteFonts
        {

        }

        public static class BitmapFonts
        {

        }

        public static class TrueTypeFonts
        {
            private static readonly string pathPrefix = "Content/Fonts/TrueTypeFonts";
            public static readonly string ARIAL = $"{pathPrefix}/Arial.ttf";
            public static readonly string TIMES_NEW_ROMAN = $"{pathPrefix}/Times New Roman.ttf";
        }
    }
}
