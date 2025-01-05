using Monogame_RPG_Engine.Engine.Core;
using Monogame_RPG_Engine.Engine.Extensions;
using Monogame_RPG_Engine.Engine.Scene.PlayerCore;
using Monogame_RPG_Engine.Engine.SpriteGraphics;
using Monogame_RPG_Engine.Engine.Utils;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// This class is a base class for all npcs in the game -- all npcs should extend from it
namespace Monogame_RPG_Engine.Engine.Scene.EntitiesCore
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

        private struct Proximity
        {
            public CardinalDirection CardinalDirection { get; }
            public float? Distance { get; }

            public Proximity(CardinalDirection cardinalDirection, float? distance)
            {
                CardinalDirection = cardinalDirection;
                Distance = distance;
            }
        }

        public void FacePlayer(Player player)
        {
            List<Proximity> proximities = new List<Proximity>();
            bool isNorthOfPlayer = Bounds.Y2 <= player.Bounds.Y;
            float? northDistance = isNorthOfPlayer ? Math.Abs(Bounds.Y2 - player.Bounds.Y) : null;
            proximities.Add(new Proximity(CardinalDirection.NORTH, northDistance));
            bool isSouthOfPlayer = Bounds.Y >= player.Bounds.Y2;
            float? southDistance = isSouthOfPlayer ? Math.Abs(Bounds.Y - player.Bounds.Y2) : null;
            proximities.Add(new Proximity(CardinalDirection.SOUTH, southDistance));

            bool isEastOfPlayer = Bounds.X2 <= player.Bounds.X;
            float? eastDistance = isEastOfPlayer ? Math.Abs(Bounds.X2 - player.Bounds.X) : null;
            proximities.Add(new Proximity(CardinalDirection.EAST, eastDistance));

            bool isWestOfPlayer = Bounds.X >= player.Bounds.X2;
            float? westDistance = isWestOfPlayer ? Math.Abs(Bounds.X - player.Bounds.X2) : null;
            proximities.Add(new Proximity(CardinalDirection.WEST, westDistance));

            Proximity closest = proximities[0];
            foreach (Proximity proximity in proximities.Skip(1))
            {
                if (proximity.Distance.HasValue && (closest.Distance == null || closest.Distance > proximity.Distance.Value))
                {
                    closest = proximity;
                }
            }

            switch (closest.CardinalDirection)
            {
                case CardinalDirection.NORTH:
                    CurrentAnimationName = "STAND_DOWN";
                    break;
                case CardinalDirection.SOUTH:
                    CurrentAnimationName = "STAND_UP";
                    break;
                case CardinalDirection.EAST:
                    CurrentAnimationName = "STAND_RIGHT";
                    break;
                case CardinalDirection.WEST:
                    CurrentAnimationName = "STAND_LEFT";
                    break;
            }
        }

        public void Stand(Direction direction)
        {
            CurrentAnimationName = $"STAND_{direction.GetFacingDirectionSuffix()}";
        }

        public void Walk(Direction direction, float speed)
        {
            CurrentAnimationName = $"WALK_{direction.GetFacingDirectionSuffix()}";

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
