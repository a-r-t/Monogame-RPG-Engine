using App.Resources;
using Engine.Builders;
using Engine.Core;
using Engine.Entity;
using Engine.Scene;
using Engine.Utils;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// This class is for the walrus NPC
namespace App.NPCs
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
                }
            };
        }
    }
}
