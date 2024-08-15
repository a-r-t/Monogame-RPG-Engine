using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Extensions
{
    public static class FloatExtensions
    {
        public static int Round(this float f)
        {
            return (int)Math.Round(f, MidpointRounding.AwayFromZero);
        }
    }
}
