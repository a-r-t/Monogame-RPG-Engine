using App.Resources;
using Engine.Builders;
using Engine.Core;
using Engine.Entity;
using Engine.Scene;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Players
{
    public class Cat : Player
    {
        public Cat(float x, float y, ContentLoader contentLoader)
            : base(new SpriteSheet(contentLoader.LoadTexture(GraphicsHelper.CAT), 24, 24), x, y, "STAND_RIGHT")
        {
            walkSpeed = 2.3f;
        }

        public override Dictionary<string, Frame[]> LoadAnimations(SpriteSheet spriteSheet)
        {
            return new Dictionary<string, Frame[]>() {
                {
                    "STAND_RIGHT", new Frame[]
                    {
                        new FrameBuilder(spriteSheet.GetSprite(0, 0))
                                .WithScale(3)
                                .WithBounds(6, 12, 12, 7)
                                .Build()
                    }
                },
                {
                    "STAND_LEFT", new Frame[]
                    {
                        new FrameBuilder(spriteSheet.GetSprite(0, 0))
                                .WithScale(3)
                                .WithSpriteEffect(SpriteEffects.FlipHorizontally)
                                .WithBounds(6, 12, 12, 7)
                                .Build()
                    }
                },
                {
                    "WALK_RIGHT", new Frame[]
                    {
                        new FrameBuilder(spriteSheet.GetSprite(1, 0), 14)
                                .WithScale(3)
                                .WithBounds(6, 12, 12, 7)
                                .Build(),
                        new FrameBuilder(spriteSheet.GetSprite(1, 1), 14)
                                .WithScale(3)
                                .WithBounds(6, 12, 12, 7)
                                .Build(),
                        new FrameBuilder(spriteSheet.GetSprite(1, 2), 14)
                                .WithScale(3)
                                .WithBounds(6, 12, 12, 7)
                                .Build(),
                        new FrameBuilder(spriteSheet.GetSprite(1, 3), 14)
                                .WithScale(3)
                                .WithBounds(6, 12, 12, 7)
                                .Build()
                    }
                },
                {
                    "WALK_LEFT", new Frame[]
                    {
                        new FrameBuilder(spriteSheet.GetSprite(1, 0), 14)
                                .WithScale(3)
                                .WithSpriteEffect(SpriteEffects.FlipHorizontally)
                                .WithBounds(6, 12, 12, 7)
                                .Build(),
                        new FrameBuilder(spriteSheet.GetSprite(1, 1), 14)
                                .WithScale(3)
                                .WithSpriteEffect(SpriteEffects.FlipHorizontally)
                                .WithBounds(6, 12, 12, 7)
                                .Build(),
                        new FrameBuilder(spriteSheet.GetSprite(1, 2), 14)
                                .WithScale(3)
                                .WithSpriteEffect(SpriteEffects.FlipHorizontally)
                                .WithBounds(6, 12, 12, 7)
                                .Build(),
                        new FrameBuilder(spriteSheet.GetSprite(1, 3), 14)
                                .WithScale(3)
                                .WithSpriteEffect(SpriteEffects.FlipHorizontally)
                                .WithBounds(6, 12, 12, 7)
                                .Build()
                    }
                }
            };
        }
    }
}
