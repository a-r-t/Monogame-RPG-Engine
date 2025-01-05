using Monogame_RPG_Engine.App.Resources;
using Monogame_RPG_Engine.Engine.Builders;
using Monogame_RPG_Engine.Engine.Core;
using Monogame_RPG_Engine.Engine.SpriteGraphics;
using Monogame_RPG_Engine.Engine.Scene;
using Monogame_RPG_Engine.Engine.Utils;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monogame_RPG_Engine.Engine.Scene.EntitiesCore;

// This class is for the walrus NPC
namespace Monogame_RPG_Engine.App.NPCs
{
    public class Walrus : NPC
    {
        public Walrus(int id, Point location, ContentLoader contentLoader)
            : base(id, location.X, location.Y, new SpriteSheet(contentLoader.LoadTexture(GraphicsHelper.WALRUS), 24, 24), "STAND_LEFT")
        {
        }

        public override Dictionary<string, Frame[]> LoadAnimations(SpriteSheet spriteSheet)
        {
            return new Dictionary<string, Frame[]>() {
                {
                    "STAND_LEFT", new Frame[] {
                        new FrameBuilder(spriteSheet.GetSprite(0, 0))
                                .WithScale(3)
                                .WithBounds(7, 13, 11, 7)
                                .WithSpriteEffect(SpriteEffects.FlipHorizontally)
                                .Build()
                    }
                },
                {
                    "STAND_RIGHT", new Frame[] {
                        new FrameBuilder(spriteSheet.GetSprite(0, 0))
                                .WithScale(3)
                                .WithBounds(7, 13, 11, 7)
                                .Build()
                    }
                },
                {
                    "STAND_DOWN", new Frame[] {
                        new FrameBuilder(spriteSheet.GetSprite(1, 0))
                                .WithScale(3)
                                .WithBounds(7, 13, 11, 7)
                                .Build()
                    }
                },
                {
                    "STAND_UP", new Frame[] {
                        new FrameBuilder(spriteSheet.GetSprite(2, 0))
                                .WithScale(3)
                                .WithBounds(7, 13, 11, 7)
                                .Build()
                    }
                }
            };
        }
    }
}
