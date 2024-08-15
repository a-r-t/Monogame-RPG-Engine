using App.Resources;
using Engine.Builders;
using Engine.Core;
using Engine.Entity;
using Engine.Scene;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// This class is for the special rock in the map that can be moved around by the player
// when the player walks into it, it will be "pushed" forward in the same direction the player was moving in
namespace App.EnhancedMapTiles
{
    public class PushableRock : EnhancedMapTile
    {
        public PushableRock(Point location, ContentLoader contentLoader)
            : base(location.X, location.Y, new SpriteSheet(contentLoader.LoadTexture(GraphicsHelper.ROCK), 16, 16), TileType.NOT_PASSABLE)
        {

        }

        public override void Update(Player player)
        {
            base.Update(player);
            if (player.Touching(this) && player.PlayerState == PlayerState.WALKING)
            {
                if (player.CurrentWalkingXDirection == Direction.LEFT)
                {
                    if (CanMoveLeft(player))
                    {
                        MoveXHandleCollision(-1);
                    }
                }
                else if (player.CurrentWalkingXDirection == Direction.RIGHT)
                {
                    if (CanMoveRight(player))
                    {
                        MoveXHandleCollision(1);
                    }
                }
                if (player.CurrentWalkingYDirection == Direction.UP)
                {
                    if (CanMoveUp(player))
                    {
                        MoveYHandleCollision(-1);
                    }
                }
                else if (player.CurrentWalkingYDirection == Direction.DOWN)
                {
                    if (CanMoveDown(player))
                    {
                        MoveYHandleCollision(1);
                    }
                }
            }
        }

        private bool CanMoveLeft(Player player)
        {
            return player.Bounds.X1 <= Bounds.X2 + 1 && player.Bounds.X2 > Bounds.X2 && CanMoveX(player);
        }

        private bool CanMoveRight(Player player)
        {
            return player.Bounds.X2 + 1 >= Bounds.X1 && player.Bounds.X1 < Bounds.X1 && CanMoveX(player);
        }

        private bool CanMoveX(Player player)
        {
            return (player.Bounds.Y1 <= Bounds.Y2 && player.Bounds.Y2 >= Bounds.Y2) ||
                    (player.Bounds.Y2 >= Bounds.Y1 && player.Bounds.Y1 <= Bounds.Y1) ||
                    (player.Bounds.Y2 <= Bounds.Y2 && player.Bounds.Y1 >= Bounds.Y1);
        }

        private bool CanMoveUp(Player player)
        {
            return player.Bounds.Y1 <= Bounds.Y2 + 1 && player.Bounds.Y2 > Bounds.Y2 && CanMoveY(player);
        }

        private bool CanMoveDown(Player player)
        {
            return player.Bounds.Y2 + 1 >= Bounds.Y1 && player.Bounds.Y1 < Bounds.Y1 && CanMoveY(player);
        }

        private bool CanMoveY(Player player)
        {
            return (player.Bounds.X1 <= Bounds.X2 && player.Bounds.X2 >= Bounds.X2) ||
                    (player.Bounds.X2 >= Bounds.X1 && player.Bounds.X1 <= Bounds.X1) ||
                    (player.Bounds.X2 <= Bounds.X2 && player.Bounds.X1 >= Bounds.X1);
        }

        protected override GameObject LoadBottomLayer(SpriteSheet spriteSheet)
        {
            Frame frame = new FrameBuilder(spriteSheet.GetSubImage(0, 0))
                    .WithScale(3)
                    .Build();
            return new GameObject(x, y, frame);
        }
    }
}
