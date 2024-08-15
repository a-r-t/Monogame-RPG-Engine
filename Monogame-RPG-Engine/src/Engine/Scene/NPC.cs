using Engine.Core;
using Engine.Entity;
using Engine.Utils;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

// This class is a base class for all npcs in the game -- all npcs should extend from it
namespace Engine.Scene
{
    public class NPC : MapEntity
    {
        public int Id { get; set; } = 0;
        public bool IsLocked { get; set; } = false;

        public NPC(int id, float x, float y, SpriteSheet spriteSheet, string startingAnimation)
            : base(x, y, spriteSheet, startingAnimation)
        {
            Id = id;
        }

        public NPC(int id, float x, float y, Dictionary<string, Frame[]> animations, string startingAnimation)
            : base(x, y, animations, startingAnimation)
        {
            Id = id;
        }

        public NPC(int id, float x, float y, Frame[] frames)
            : base(x, y, frames)
        {
            Id = id;
        }

        public NPC(int id, float x, float y, Frame frame)
            : base(x, y, frame)
        {
            Id = id;
        }

        public NPC(int id, float x, float y)
            : base(x, y)
        {
            Id = id;
        }

        public void FacePlayer(Player player)
        {
            // if npc's center point is to the right of the player's center point, npc needs to face left
            // else if npc's center point is to the left of the player's center point, npc needs to face right
            float centerPoint = Bounds.X + (Bounds.Width / 2);
            float playerCenterPoint = player.Bounds.X + (player.Bounds.Width / 2);
            if (centerPoint < playerCenterPoint)
            {
                CurrentAnimationName = "STAND_RIGHT";
            }
            else if (centerPoint >= playerCenterPoint)
            {
                CurrentAnimationName = "STAND_LEFT";
            }
        }

        public void Stand(Direction direction)
        {
            if (direction == Direction.RIGHT)
            {
                CurrentAnimationName = "STAND_RIGHT";
            }
            else if (direction == Direction.LEFT)
            {
                CurrentAnimationName = "STAND_LEFT";
            }
        }

        public void Walk(Direction direction, float speed)
        {
            if (direction == Direction.RIGHT)
            {
                CurrentAnimationName = "WALK_RIGHT";
            }
            else if (direction == Direction.LEFT)
            {
                CurrentAnimationName = "WALK_LEFT";
            }
            else
            {
                if (CurrentAnimationName.Contains("RIGHT"))
                {
                    CurrentAnimationName = "WALK_RIGHT";
                }
                else
                {
                    CurrentAnimationName = "WALK_LEFT";
                }
            }
            if (direction == Direction.UP)
            {
                MoveY(-speed);
            }
            else if (direction == Direction.DOWN)
            {
                MoveY(speed);
            }
            else if (direction == Direction.LEFT)
            {
                MoveX(-speed);
            }
            else if (direction == Direction.RIGHT)
            {
                MoveX(speed);
            }
        }

        public void Update(Player player)
        {
            if (!IsLocked)
            {
                PerformAction(player);
            }
            base.Update();
        }

        public void Lock()
        {
            IsLocked = true;
        }

        public void Unlock()
        {
            IsLocked = false;
        }

        protected virtual void PerformAction(Player player) { }

        public override void Draw(GraphicsHandler graphicsHandler)
        {
            base.Draw(graphicsHandler);
        }
    }
}
