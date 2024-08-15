using Engine.Builders;
using Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Engine.Extensions;
using Engine.Core;
using Engine.Entity;
using Microsoft.Xna.Framework.Input;

/*
	The all important GameObject class is what every "entity" used in this game should be based off of
	It encapsulates all the other class logic in the GameObject package to be a "one stop shop" for all entity needs
	This includes:
	1. displaying an image (as a sprite) to represent the entity
	2. animation logic for the sprite
	3. collision detection with a map
	4. performing proper draw logic based on camera movement
 */
namespace Engine.Scene
{
    public class GameObject : AnimatedSprite
    {
        // stores game object's start position
        // important to keep track of this as it's what allows the special draw logic to work
        protected float startPositionX, startPositionY;

        // how much game object's position has changed from start position over time
        // also important to keep track of for the special draw logic
        protected float amountMovedX, amountMovedY;

        // previous location the game object was in from the last frame
        protected float previousX, previousY;

        // the map instance this game object "belongs" to.
        protected Map map;

        public bool IsAffectedByTriggers { get; set; } = false;

        // gets x location taking into account map camera position
        public float CalibratedXLocation
        {
            get
            {
                if (map != null)
                {
                    return x.Round() - map.Camera.X.Round();
                }
                else
                {
                    return X.Round();
                }
            }
        }

        // gets y location taking into account map camera position
        public float CalibratedYLocation
        {
            get
            {
                if (map != null)
                {
                    return y.Round() - map.Camera.Y.Round();
                }
                else
                {
                    return Y.Round();
                }
            }
        }

        // gets bounds taking into account map camera position
        public Entity.Rectangle CalibratedBounds
        {
            get
            {
                if (map != null)
                {
                    return new Entity.Rectangle(
                        Bounds.X1.Round() - map.Camera.X.Round(),
                        Bounds.Y1.Round() - map.Camera.Y.Round(),
                        Bounds.Width,
                        Bounds.Height
                    );
                }
                else
                {
                    return Bounds;
                }
            }
        }

        public GameObject(SpriteSheet spriteSheet, float x, float y, string startingAnimation)
            : base(spriteSheet, x, y, startingAnimation)
        {
            startPositionX = x;
            startPositionY = y;
            previousX = x;
            previousY = y;
        }

        public GameObject(float x, float y, Dictionary<string, Frame[]> animations, string startingAnimation)
            : base(x, y, animations, startingAnimation)
        {
            startPositionX = x;
            startPositionY = y;
            previousX = x;
            previousY = y;
        }

        public GameObject(float x, float y, Frame[] frames)
            : base(x, y, frames)
        {
            startPositionX = x;
            startPositionY = y;
            previousX = x;
            previousY = y;
        }

        public GameObject(float x, float y, Frame frame)
            : base(x, y, frame)
        {
            startPositionX = x;
            startPositionY = y;
            previousX = x;
            previousY = y;
        }

        public GameObject(float x, float y)
            : base(x, y, new Frame(ImageUtils.CreateSolidImage(new Color(255, 0, 255), 1, 1), SpriteEffects.None, 1, null))
        {
            startPositionX = x;
            startPositionY = y;
            previousX = x;
            previousY = y;
        }

        public virtual void Update(KeyboardState keyboardState)
        {
            // update previous position to be the current position
            previousX = x;
            previousY = y;

            // call to animation logic
            base.Update();
        }

        // move game object along the x axis
        // will stop object from moving based on map collision logic (such as if it hits a solid tile)
        public float MoveXHandleCollision(float dx)
        {
            if (map != null)
            {
                return HandleCollisionX(dx);
            }
            else
            {
                MoveX(dx);
                return dx;
            }
        }

        // move game object along the y axis
        // will stop object from moving based on map collision logic (such as if it hits a solid tile)
        public float MoveYHandleCollision(float dy)
        {
            if (map != null)
            {
                return HandleCollisionY(dy);
            }
            else
            {
                MoveY(dy);
                return dy;
            }
        }

        // performs collision check logic for moving along the x axis against the map's tiles
        public float HandleCollisionX(float moveAmountX)
        {
            // determines amount to move (whole number)
            int amountToMove = (int)Math.Abs(moveAmountX);

            // gets decimal remainder from amount to move
            float moveAmountXRemainder = MathUtils.GetRemainder(moveAmountX);

            // determines direction that will be moved in based on if moveAmountX is positive or negative
            Direction direction = moveAmountX < 0 ? Direction.LEFT : Direction.RIGHT;

            // moves game object one pixel at a time until total move amount is reached
            // if at any point a map tile collision is determined to have occurred from the move,
            // move player back to right in front of the "solid" map tile's position, and stop attempting to move further
            float amountMoved = 0;
            bool hasCollided = false;
            GameObject entityCollidedWith = null;
            for (int i = 0; i < amountToMove; i++)
            {
                MoveX(direction.GetVelocity());
                MapCollisionCheckResult collisionCheckResult = MapCollisionHandler.GetAdjustedPositionAfterCollisionCheckX(this, map, direction);
                if (collisionCheckResult.AdjustedLocation != null)
                {
                    hasCollided = true;
                    entityCollidedWith = collisionCheckResult.EntityCollidedWith;
                    if (entityCollidedWith is not Trigger)
                    {
                        X = collisionCheckResult.AdjustedLocation.X;
                    }
                    break;
                }
                amountMoved = i + 1;
            }

            // if no collision occurred in the above steps, this deals with the decimal remainder from the original move amount (stored in moveAmountXRemainder)
            // it starts by moving the game object by that decimal amount
            // it then does one more check for a collision in the case that this added decimal amount was enough to change the rounding and move the game object to the next pixel over
            // if a collision occurs from this move, the player is moved back to right in front of the "solid" map tile's position
            // if a collision occurs with a trigger and entity is affected by triggers, the trigger is activated
            if (!hasCollided)
            {
                MoveX(moveAmountXRemainder * direction.GetVelocity());
                MapCollisionCheckResult collisionCheckResult = MapCollisionHandler.GetAdjustedPositionAfterCollisionCheckX(this, map, direction);
                if (collisionCheckResult.AdjustedLocation != null)
                {
                    hasCollided = true;
                    entityCollidedWith = collisionCheckResult.EntityCollidedWith;
                    if (entityCollidedWith is not Trigger)
                    {
                        float xLocationBeforeAdjustment = X;
                        X = collisionCheckResult.AdjustedLocation.X;
                        amountMoved += Math.Abs(xLocationBeforeAdjustment - X);
                    }
                }
                else
                {
                    amountMoved += moveAmountXRemainder;
                }
            }

            if (IsAffectedByTriggers && entityCollidedWith is Trigger && map.ActiveScript == null)
            {
                Trigger trigger = (Trigger)entityCollidedWith;
                if (trigger.TriggerScript != null)
                {
                    map.ActiveScript = trigger.TriggerScript;
                }
            }

            else
            {
                // call this method which a game object subclass can override to listen for collision events and react accordingly
                OnEndCollisionCheckX(hasCollided, direction, entityCollidedWith);
            }

            // returns the amount actually moved
            return amountMoved * direction.GetVelocity();
        }

        // performs collision check logic for moving along the x axis against the map's tiles
        public float HandleCollisionY(float moveAmountY)
        {
            // determines amount to move (whole number)
            int amountToMove = (int)Math.Abs(moveAmountY);

            // gets decimal remainder from amount to move
            float moveAmountYRemainder = MathUtils.GetRemainder(moveAmountY);

            // determines direction that will be moved in based on if moveAmountX is positive or negative
            Direction direction = moveAmountY < 0 ? Direction.UP : Direction.DOWN;

            // moves game object one pixel at a time until total move amount is reached
            // if at any point a map tile collision is determined to have occurred from the move,
            // move player back to right in front of the "solid" map tile's position, and stop attempting to move further
            float amountMoved = 0;
            bool hasCollided = false;
            GameObject entityCollidedWith = null;
            for (int i = 0; i < amountToMove; i++)
            {
                MoveY(direction.GetVelocity());
                MapCollisionCheckResult collisionCheckResult = MapCollisionHandler.GetAdjustedPositionAfterCollisionCheckY(this, map, direction);
                if (collisionCheckResult.AdjustedLocation != null)
                {
                    hasCollided = true;
                    entityCollidedWith = collisionCheckResult.EntityCollidedWith;
                    if (entityCollidedWith is not Trigger)
                    {
                        Y = collisionCheckResult.AdjustedLocation.Y;
                    }
                    break;
                }
                amountMoved = i + 1;
            }

            // if no collision occurred in the above steps, this deals with the decimal remainder from the original move amount (stored in moveAmountXRemainder)
            // it starts by moving the game object by that decimal amount
            // it then does one more check for a collision in the case that this added decimal amount was enough to change the rounding and move the game object to the next pixel over
            // if a collision occurs from this move, the player is moved back to right in front of the "solid" map tile's position
            // if a collision occurs with a trigger and entity is affected by triggers, the trigger is activated
            if (!hasCollided)
            {
                MoveY(moveAmountYRemainder * direction.GetVelocity());
                MapCollisionCheckResult collisionCheckResult = MapCollisionHandler.GetAdjustedPositionAfterCollisionCheckY(this, map, direction);
                if (collisionCheckResult.AdjustedLocation != null)
                {
                    hasCollided = true;
                    entityCollidedWith = collisionCheckResult.EntityCollidedWith;
                    if (entityCollidedWith is not Trigger)
                    {
                        float yLocationBeforeAdjustment = Y;
                        Y = collisionCheckResult.AdjustedLocation.Y;
                        amountMoved += Math.Abs(yLocationBeforeAdjustment - Y);
                    }
                }
                else
                {
                    amountMoved += moveAmountYRemainder;
                }
            }

            if (IsAffectedByTriggers && entityCollidedWith is Trigger && map.ActiveScript == null)
            {
                Trigger trigger = (Trigger)entityCollidedWith;
                if (trigger.TriggerScript != null)
                {
                    map.ActiveScript = trigger.TriggerScript;
                }
            }

            else
            {
                // call this method which a game object subclass can override to listen for collision events and react accordingly
                OnEndCollisionCheckY(hasCollided, direction, entityCollidedWith);
            }

            // returns the amount actually moved
            return amountMoved * direction.GetVelocity();
        }

        // game object subclass can override this method to listen for x axis collision events and react accordingly after calling "moveXHandleCollision"
        public virtual void OnEndCollisionCheckX(bool hasCollided, Direction direction, GameObject entityCollidedWith) { }

        // game object subclass can override this method to listen for y axis collision events and react accordingly after calling "moveYHandleCollision"
        public virtual void OnEndCollisionCheckY(bool hasCollided, Direction direction, GameObject entityCollidedWith) { }

        // set this game object's map to make it a "part of" the map, allowing calibrated positions and collision handling logic to work
        public virtual void SetMap(Map map)
        {
            this.map = map;
        }

        public override void Draw(GraphicsHandler graphicsHandler)
        {
            if (map != null)
            {
                graphicsHandler.DrawImage(
                    CurrentFrame.Image,
                    new Vector2(
                        CalibratedXLocation.Round(),
                        CalibratedYLocation.Round()
                    ),
                    spriteEffects: CurrentFrame.SpriteEffect,
                    scale: new Vector2(CurrentFrame.Scale, CurrentFrame.Scale)
                );
            }
            else
            {
                base.Draw(graphicsHandler);
            }
        }

        public override void DrawBounds(GraphicsHandler graphicsHandler, Color color)
        {
            if (map != null)
            {
                Entity.Rectangle calibratedBounds = CalibratedBounds;
                calibratedBounds.Color = color;
                calibratedBounds.Draw(graphicsHandler);
            }
            else
            {
                base.DrawBounds(graphicsHandler, color);
            }
        }
    }
}
