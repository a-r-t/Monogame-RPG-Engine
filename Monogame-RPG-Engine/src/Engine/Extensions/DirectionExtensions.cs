using Monogame_RPG_Engine.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_RPG_Engine.Engine.Extensions
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

        // used to make piecing together animation names based on direction easier
        public static string GetFacingDirectionSuffix(this Direction direction)
        {

            switch (direction)
            {
                case Direction.LEFT: return "LEFT";
                case Direction.RIGHT: return "RIGHT";
                case Direction.UP: return "UP";
                case Direction.DOWN: return "DOWN";
                default: return "";
            }
        }
    }
}
