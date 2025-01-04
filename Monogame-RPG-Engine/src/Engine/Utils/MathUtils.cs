using Microsoft.Xna.Framework;
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

        public static Vector2 GetNormalizedVector(float x, float y)
        {
            Vector2 vector = new Vector2(x, y);
            if (vector.Length() > 0)
            {
                vector.Normalize();
            }
            return vector;
        }

        public static Vector2 GetNormalizedVector(Vector2 vector)
        {
            if (vector.Length() > 0)
            {
                vector.Normalize();
            }
            return vector;
        }
    }
}
