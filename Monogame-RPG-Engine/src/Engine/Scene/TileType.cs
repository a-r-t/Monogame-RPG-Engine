using System;
using System.Collections.Generic;
using System.Text;

// Represents different tile types a MapTile or EnhancedMapTile can have, which affects how entities interact with it collision wise
namespace Engine.Scene
{
    public enum TileType
    {
        PASSABLE, NOT_PASSABLE
    }
}
