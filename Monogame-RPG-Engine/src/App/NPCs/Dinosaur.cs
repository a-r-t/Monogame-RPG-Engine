using App.Resources;
using Engine.Builders;
using Engine.Core;
using Engine.SpriteGraphics;
using Engine.Scene;
using Engine.Utils;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Scene.EntitiesCore;

// This class is for the dinosaur NPC
namespace App.NPCs
{
    public class Dinosaur : NPC
    {
        public Dinosaur(int id, Point location, ContentLoader contentLoader)
            : base(id, location.X, location.Y, new SpriteSheet(contentLoader.LoadTexture(GraphicsHelper.DINOSAUR), 14, 17), "STAND_LEFT")
        {
        }

        public override Dictionary<string, Frame[]> LoadAnimations(SpriteSheet spriteSheet)
        {
            return new Dictionary<string, Frame[]>() {
                {
                    "STAND_LEFT", new Frame[] {
                        new FrameBuilder(spriteSheet.GetSprite(0, 0))
                                .WithScale(3)
                                .WithBounds(4, 5, 5, 10)
                                .Build()
                    }
                },
                {
                    "STAND_RIGHT", new Frame[] {
                        new FrameBuilder(spriteSheet.GetSprite(0, 0))
                                .WithScale(3)
                                .WithBounds(4, 5, 5, 10)
                                .WithSpriteEffect(SpriteEffects.FlipHorizontally)
                                .Build()
                    }
                },
                {
                    "STAND_UP", new Frame[] {
                        new FrameBuilder(spriteSheet.GetSprite(2, 0))
                                .WithScale(3)
                                .WithBounds(4, 5, 5, 10)
                                .Build()
                    }
                },
                {
                    "STAND_DOWN", new Frame[] {
                        new FrameBuilder(spriteSheet.GetSprite(4, 0))
                                .WithScale(3)
                                .WithBounds(4, 5, 5, 10)
                                .Build()
                    }
                },
                {
                    "WALK_LEFT", new Frame[] {
                        new FrameBuilder(spriteSheet.GetSprite(1, 0), 14)
                                .WithScale(3)
                                .WithBounds(4, 5, 5, 10)
                                .Build(),
                        new FrameBuilder(spriteSheet.GetSprite(1, 1), 14)
                                .WithScale(3)
                                .WithBounds(4, 5, 5, 10)
                                .Build()
                    }
                },
                {
                    "WALK_RIGHT", new Frame[] {
                        new FrameBuilder(spriteSheet.GetSprite(1, 0), 14)
                                .WithScale(3)
                                .WithSpriteEffect(SpriteEffects.FlipHorizontally)
                                .WithBounds(4, 5, 5, 10)
                                .Build(),
                        new FrameBuilder(spriteSheet.GetSprite(1, 1), 14)
                                .WithScale(3)
                                .WithSpriteEffect(SpriteEffects.FlipHorizontally)
                                .WithBounds(4, 5, 5, 10)
                                .Build()
                    }
                },
                {
                    "WALK_UP", new Frame[] {
                        new FrameBuilder(spriteSheet.GetSprite(3, 0), 14)
                                .WithScale(3)
                                .WithBounds(4, 5, 5, 10)
                                .Build(),
                        new FrameBuilder(spriteSheet.GetSprite(3, 1), 14)
                                .WithScale(3)
                                .WithBounds(4, 5, 5, 10)
                                .Build(),
                        new FrameBuilder(spriteSheet.GetSprite(3, 2), 14)
                                .WithScale(3)
                                .WithBounds(4, 5, 5, 10)
                                .Build(),
                        new FrameBuilder(spriteSheet.GetSprite(3, 3), 14)
                                .WithScale(3)
                                .WithBounds(4, 5, 5, 10)
                                .Build()
                    }
                },
                {
                    "WALK_DOWN", new Frame[] {
                        new FrameBuilder(spriteSheet.GetSprite(5, 0), 14)
                                .WithScale(3)
                                .WithBounds(4, 5, 5, 10)
                                .Build(),
                        new FrameBuilder(spriteSheet.GetSprite(5, 1), 14)
                                .WithScale(3)
                                .WithBounds(4, 5, 5, 10)
                                .Build(),
                        new FrameBuilder(spriteSheet.GetSprite(5, 2), 14)
                                .WithScale(3)
                                .WithBounds(4, 5, 5, 10)
                                .Build(),
                        new FrameBuilder(spriteSheet.GetSprite(5, 3), 14)
                                .WithScale(3)
                                .WithBounds(4, 5, 5, 10)
                                .Build()
                    }
                }
            };
        }
    }
}
