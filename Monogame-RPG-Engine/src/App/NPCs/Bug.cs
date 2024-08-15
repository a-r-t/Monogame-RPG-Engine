using App.Resources;
using Engine.Builders;
using Engine.Core;
using Engine.Entity;
using Engine.Extensions;
using Engine.Scene;
using Engine.Utils;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.NPCs
{
    public class Bug : NPC
    {
        private int totalAmountMoved = 0;
        private Direction direction = Direction.RIGHT;
        private float speed = 1;

        public Bug(int id, Point location, ContentLoader contentLoader)
            : base(id, location.X, location.Y, new SpriteSheet(contentLoader.LoadTexture(GraphicsHelper.BUG), 24, 15), "WALK_RIGHT")
        {
        }

        // this code makes the bug npc walk back and forth (left to right)
        protected override void PerformAction(Player player)
        {
            // if bug has not yet moved 90 pixels in one direction, move bug forward
            if (totalAmountMoved < 90)
            {
                float amountMoved = MoveXHandleCollision(speed * direction.GetVelocity());
                totalAmountMoved += (int)Math.Abs(amountMoved);
            }

            // else if bug has already moved 90 pixels in one direction, flip the bug's direction
            else
            {
                totalAmountMoved = 0;
                if (direction == Direction.LEFT)
                {
                    direction = Direction.RIGHT;
                }
                else
                {
                    direction = Direction.LEFT;
                }
            }

            // based off of the bugs current walking direction, set its animation to match
            if (direction == Direction.RIGHT)
            {
                CurrentAnimationName = "WALK_RIGHT";
            }
            else
            {
                CurrentAnimationName = "WALK_LEFT";
            }
        }

        public override Dictionary<string, Frame[]> LoadAnimations(SpriteSheet spriteSheet)
        {
            return new Dictionary<string, Frame[]>() {
                {
                    "STAND_LEFT", new Frame[] {
                        new FrameBuilder(spriteSheet.GetSprite(0, 0))
                            .WithScale(2)
                            .WithBounds(3, 5, 18, 7)
                            .Build()
                    }
                },
                {
                    "STAND_RIGHT", new Frame[] {
                        new FrameBuilder(spriteSheet.GetSprite(0, 0))
                            .WithScale(2)
                            .WithBounds(3, 5, 18, 7)
                            .WithSpriteEffect(SpriteEffects.FlipHorizontally)
                            .Build()
                    }
                },
                {
                    "WALK_LEFT", new Frame[] {
                        new FrameBuilder(spriteSheet.GetSprite(0, 0), 8)
                            .WithScale(2)
                            .WithBounds(3, 5, 18, 7)
                            .Build(),
                        new FrameBuilder(spriteSheet.GetSprite(0, 1), 8)
                            .WithScale(2)
                            .WithBounds(3, 5, 18, 7)
                            .Build()
                    }
                },
                {
                    "WALK_RIGHT", new Frame[] {
                        new FrameBuilder(spriteSheet.GetSprite(0, 0), 8)
                                .WithScale(2)
                                .WithSpriteEffect(SpriteEffects.FlipHorizontally)
                                .WithBounds(3, 5, 18, 7)
                                .Build(),
                        new FrameBuilder(spriteSheet.GetSprite(0, 1), 8)
                                .WithScale(2)
                                .WithSpriteEffect(SpriteEffects.FlipHorizontally)
                                .WithBounds(3, 5, 18, 7)
                                .Build()
                    }
                }
            };
        }
    }
}
