using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Extensions
{
    public static class DirectionExtensions
    {
        public static int GetVelocity(this Direction direction)
        {
            switch (direction)
            {
                case Direction.LEFT:
                case Direction.UP:
                    return -1;
                case Direction.RIGHT:
                case Direction.DOWN:
                    return 1;
                default:
                    return 0;
            }
        }
    }
}
