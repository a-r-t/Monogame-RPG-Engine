using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Return type from MapTileCollisionHandler collision checks
// Contains adjusted location (where player should be moved to if a collision occurred) and the entity the player collided with (if any)
namespace Engine.Scene
{
    public class MapCollisionCheckResult
    {
        public Point AdjustedLocation;
        public GameObject EntityCollidedWith;

        public MapCollisionCheckResult(Point adjustedLocationAfterCollisionCheck, GameObject entityCollidedWith)
        {
            AdjustedLocation = adjustedLocationAfterCollisionCheck;
            EntityCollidedWith = entityCollidedWith;
        }
    }
}
