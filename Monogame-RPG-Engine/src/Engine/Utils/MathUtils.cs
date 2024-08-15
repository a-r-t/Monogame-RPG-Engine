using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Utils
{
    public class MathUtils
    {
        public static float GetRemainder(float number)
        {
            return number > 0 ? (float)Math.Abs(number - Math.Floor(number)) : (float)Math.Abs(number - Math.Ceiling(number));
        }
    }
}
