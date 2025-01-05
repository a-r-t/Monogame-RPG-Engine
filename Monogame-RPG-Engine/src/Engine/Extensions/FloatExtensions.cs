using System;
using System.Collections.Generic;
using System.Text;

namespace Monogame_RPG_Engine.Engine.Extensions
{
    public static class FloatExtensions
    {
        public static int Round(this float f)
        {
            return (int)Math.Round(f, MidpointRounding.AwayFromZero);
        }
    }
}
