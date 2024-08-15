using Engine.Entity;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Scene
{
    public class MapEntity : GameObject
    {
        public MapEntityStatus MapEntityStatus { get; set; } = MapEntityStatus.ACTIVE;

        // if true, enemy cannot go out of camera's update range
        public bool IsUpdateOffScreen { get; set; }

        // if true, entity will no longer be updated or drawn on the map
        public bool IsHidden { get; set; } = false;

        // if given an existence flag, and that flag gets set, the entity will no longer exist until the flag is unset
        public string ExistenceFlag { get; set; }

        // script that executes when entity is interacted with by the player
        private Script interactScript;
        public Script InteractScript
        {
            get
            {
                return interactScript;
            }
            set
            {
                interactScript = value;
                interactScript.SetMapEntity(this);
            }
        }

        // set to true if you do not want this entity to be able to be collided with by other entities
        // example of why you would use this: this is an entity that you want to overlap the player, but you do not want the player to be blocked by/have to worry about this entity's collision bounds when trying to move
        public bool IsUncollidable { get; set; } = false;

        public bool Exists
        {
            get
            {
                return ExistenceFlag == null || !map.FlagManager.IsFlagSet(ExistenceFlag);
            }
        }

        public MapEntity(float x, float y, SpriteSheet spriteSheet, String startingAnimation)
            : base(spriteSheet, x, y, startingAnimation)
        {
        }

        public MapEntity(float x, float y, Dictionary<string, Frame[]> animations, string startingAnimation)
            : base(x, y, animations, startingAnimation)
        {
        }

        public MapEntity(float x, float y, Frame[] frames)
            : base(x, y, frames)
        {
        }

        public MapEntity(float x, float y, Frame frame)
            : base(x, y, frame)
        {
        }

        public MapEntity(float x, float y)
            : base(x, y)
        {
        }


    }
}
