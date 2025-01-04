using Engine.SpriteGraphics;
using Engine.Utils;
using Microsoft.Xna.Framework.Input;
using Engine.Scene.EntitiesCore;
using Engine.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Scene.PlayerCore
{
    public abstract class Player : GameObject
    {
        // values that affect player movement
        // these should be set in a subclass
        protected float walkSpeed = 0;
        protected int interactionRange = 1;
        public Direction CurrentWalkingXDirection { get; private set; }
        public Direction CurrentWalkingYDirection { get; private set; }
        public Direction LastWalkingXDirection { get; private set; }
        public Direction LastWalkingYDirection { get; private set; }

        // values used to handle player movement
        protected float moveAmountX, moveAmountY;
        protected float lastAmountMovedX, lastAmountMovedY;

        // values used to keep track of player's current state
        public PlayerState PlayerState { get; set; }
        protected PlayerState previousPlayerState;
        public Direction FacingDirection { get; set; }
        protected Direction lastMovementDirection;

        // define keys
        protected KeyLocker keyLocker = new KeyLocker();
        protected Keys MOVE_LEFT_KEY = Keys.Left;
        protected Keys MOVE_RIGHT_KEY = Keys.Right;
        protected Keys MOVE_UP_KEY = Keys.Up;
        protected Keys MOVE_DOWN_KEY = Keys.Down;
        public Keys INTERACT_KEY { get; private set; } = Keys.Space;
        private List<Keys> movementXKeysPressed = new List<Keys>();
        private List<Keys> movementYKeysPressed = new List<Keys>();
        private List<Keys> movementKeysPressed = new List<Keys>();


        protected bool isLocked = false;

        public Player(SpriteSheet spriteSheet, float x, float y, string startingAnimationName)
            : base(spriteSheet, x, y, startingAnimationName)
        {
            FacingDirection = Direction.RIGHT;
            PlayerState = PlayerState.STANDING;
            previousPlayerState = PlayerState;
            IsAffectedByTriggers = true;
        }

        public override void Update(KeyboardState keyboardState)
        {
            if (!isLocked)
            {
                moveAmountX = 0;
                moveAmountY = 0;

                // if player is currently playing through level (has not won or lost)
                // update player's state and current actions, which includes things like determining how much it should move each frame and if its walking or jumping
                do
                {
                    previousPlayerState = PlayerState;
                    HandlePlayerState(keyboardState);
                } while (previousPlayerState != PlayerState);

                // move player with respect to map collisions based on how much player needs to move this frame
                lastAmountMovedY = base.MoveYHandleCollision(moveAmountY);
                lastAmountMovedX = base.MoveXHandleCollision(moveAmountX);
            }

            HandlePlayerAnimation();

            UpdateLockedKeys(keyboardState);

            // update player's animation
            base.Update();
        }

        // based on player's current state, call appropriate player state handling method
        protected void HandlePlayerState(KeyboardState keyboardState)
        {
            switch (PlayerState)
            {
                case PlayerState.STANDING:
                    PlayerStanding(keyboardState);
                    break;
                case PlayerState.WALKING:
                    PlayerWalking(keyboardState);
                    break;
            }
        }

        // player STANDING state logic
        protected void PlayerStanding(KeyboardState keyboardState)
        {
            if (!keyLocker.IsKeyLocked(INTERACT_KEY) && keyboardState.IsKeyDown(INTERACT_KEY))
            {
                keyLocker.LockKey(INTERACT_KEY);
                map.EntityInteract(this);
            }

            // if a walk key is pressed, player enters WALKING state
            if (IsMovementKeyPressed(keyboardState))
            {
                PlayerState = PlayerState.WALKING;
            }
        }

        // player WALKING state logic
        protected void PlayerWalking(KeyboardState keyboardState)
        {
            if (!keyLocker.IsKeyLocked(INTERACT_KEY) && keyboardState.IsKeyDown(INTERACT_KEY))
            {
                keyLocker.LockKey(INTERACT_KEY);
                map.EntityInteract(this);
            }

            LastWalkingXDirection = CurrentWalkingXDirection;

            // if player is walking left or right, put keys in the movementKeysPressed list
            if (keyboardState.IsKeyDown(MOVE_LEFT_KEY) && !movementKeysPressed.Contains(MOVE_LEFT_KEY))
            {
                movementKeysPressed.Add(MOVE_LEFT_KEY);
            }
            else if (keyboardState.IsKeyDown(MOVE_RIGHT_KEY) && !movementKeysPressed.Contains(MOVE_RIGHT_KEY))
            {
                movementKeysPressed.Add(MOVE_RIGHT_KEY);
            }
            else
            {
                CurrentWalkingXDirection = Direction.NONE;
            }

            // if either left or right is no longer being pressed, remove the key from the movementKeysPressed list
            if (keyboardState.IsKeyUp(MOVE_LEFT_KEY))
            {
                movementKeysPressed.Remove(MOVE_LEFT_KEY);
            }
            if (keyboardState.IsKeyUp(MOVE_RIGHT_KEY))
            {
                movementKeysPressed.Remove(MOVE_RIGHT_KEY);
            }

            LastWalkingYDirection = CurrentWalkingYDirection;

            // if player is walking up or down, put keys in the movementKeysPressed list
            if (keyboardState.IsKeyDown(MOVE_UP_KEY) && !movementKeysPressed.Contains(MOVE_UP_KEY))
            {
                movementKeysPressed.Add(MOVE_UP_KEY);
            }
            else if (keyboardState.IsKeyDown(MOVE_DOWN_KEY) && !movementKeysPressed.Contains(MOVE_DOWN_KEY))
            {
                movementKeysPressed.Add(MOVE_DOWN_KEY);
            }
            else
            {
                CurrentWalkingYDirection = Direction.NONE;
            }

            // if either up or down is no longer being pressed, remove the key from the movementKeysPressed list
            if (keyboardState.IsKeyUp(MOVE_UP_KEY))
            {
                movementKeysPressed.Remove(MOVE_UP_KEY);
            }
            if (keyboardState.IsKeyUp(MOVE_DOWN_KEY))
            {
                movementKeysPressed.Remove(MOVE_DOWN_KEY);
            }

            // figure out which key was last pressed on x (left/right) and y (up/down)
            Keys? lastMovementXKeyPressed = null;
            Keys? lastMovementYKeyPressed = null;
            foreach (Keys key in movementKeysPressed)
            {
                if (key == MOVE_LEFT_KEY || key == MOVE_RIGHT_KEY)
                {
                    lastMovementXKeyPressed = key;
                }
                else if (key == MOVE_UP_KEY || key == MOVE_DOWN_KEY)
                {
                    lastMovementYKeyPressed = key;
                }
            }
            // figure out which movement key was last pressed (regardless of axis)
            Keys? lastMovementKeyPressed = movementKeysPressed.LastOrDefault();

            // based on last movement key pressed on x axis, move player either left or right
            if (lastMovementXKeyPressed.HasValue)
            {
                if (lastMovementXKeyPressed.Value == MOVE_LEFT_KEY)
                {
                    moveAmountX -= walkSpeed;
                    CurrentWalkingXDirection = Direction.LEFT;
                }
                else if (lastMovementXKeyPressed.Value == MOVE_RIGHT_KEY)
                {
                    moveAmountX += walkSpeed;
                    CurrentWalkingXDirection = Direction.RIGHT;
                }
            }

            // based on last movement key pressed on y axis, move player either up or down
            if (lastMovementYKeyPressed.HasValue)
            {
                if (lastMovementYKeyPressed.Value == MOVE_UP_KEY)
                {
                    moveAmountY -= walkSpeed;
                    CurrentWalkingYDirection = Direction.UP;
                }
                else if (lastMovementYKeyPressed.Value == MOVE_DOWN_KEY)
                {
                    moveAmountY += walkSpeed;
                    CurrentWalkingYDirection = Direction.DOWN;
                }
            }
            
            // have player face the direction of the last movement key pressed
            if (lastMovementKeyPressed.HasValue)
            {
                if (lastMovementKeyPressed == MOVE_LEFT_KEY)
                {
                    FacingDirection = Direction.LEFT;
                }
                else if (lastMovementKeyPressed == MOVE_RIGHT_KEY)
                {
                    FacingDirection = Direction.RIGHT;
                }
                else if (lastMovementKeyPressed == MOVE_UP_KEY)
                {
                    FacingDirection = Direction.UP;
                }
                else if (lastMovementKeyPressed == MOVE_DOWN_KEY)
                {
                    FacingDirection = Direction.DOWN;
                }
            }
            
            if (!IsMovementKeyPressed(keyboardState))
            {
                PlayerState = PlayerState.STANDING;
            }
        }

        protected bool IsMovementKeyPressed(KeyboardState keyboardState)
        {
            return keyboardState.IsKeyDown(MOVE_LEFT_KEY) || keyboardState.IsKeyDown(MOVE_RIGHT_KEY) || keyboardState.IsKeyDown(MOVE_UP_KEY) || keyboardState.IsKeyDown(MOVE_DOWN_KEY);
        }

        protected void UpdateLockedKeys(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyUp(INTERACT_KEY) && !isLocked)
            {
                keyLocker.UnlockKey(INTERACT_KEY);
            }
        }

        // anything extra the player should do based on interactions can be handled here
        protected virtual void HandlePlayerAnimation()
        {
            if (PlayerState == PlayerState.STANDING)
            {
                // sets animation to a STAND animation based on which way player is facing
                CurrentAnimationName = $"STAND_{FacingDirection.GetFacingDirectionSuffix()}";
            }
            else if (PlayerState == PlayerState.WALKING)
            {
                CurrentAnimationName = $"WALK_{FacingDirection.GetFacingDirectionSuffix()}";
            }
        }

        public override void OnEndCollisionCheckX(bool hasCollided, Direction direction, GameObject entityCollidedWith) { }

        public override void OnEndCollisionCheckY(bool hasCollided, Direction direction, GameObject entityCollidedWith) { }

        public Rectangle GetInteractionRange()
        {
            return new Rectangle(
                Bounds.X1 - interactionRange,
                Bounds.Y1 - interactionRange,
                Bounds.Width + (interactionRange * 2),
                Bounds.Height + (interactionRange * 2));
        }

        public void Lock()
        {
            isLocked = true;
            PlayerState = PlayerState.STANDING;
            CurrentAnimationName = $"STAND_{FacingDirection.GetFacingDirectionSuffix()}";
        }

        public void Unlock()
        {
            isLocked = false;
            PlayerState = PlayerState.STANDING;
            CurrentAnimationName = $"STAND_{FacingDirection.GetFacingDirectionSuffix()}";
        }

        // used by other files or scripts to force player to stand
        public void Stand(Direction direction)
        {
            PlayerState = PlayerState.STANDING;
            FacingDirection = direction;
            CurrentAnimationName = $"STAND_{FacingDirection.GetFacingDirectionSuffix()}";
        }

        // used by other files or scripts to force player to walk
        public void Walk(Direction direction, float speed)
        {
            PlayerState = PlayerState.WALKING;
            FacingDirection = direction;
            CurrentAnimationName = $"WALK_{FacingDirection.GetFacingDirectionSuffix()}";

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

        // Uncomment this to have game draw player's bounds to make it easier to visualize
        /*
        public override void Draw(GraphicsHandler graphicsHandler)
        {
            base.Draw(graphicsHandler);
            DrawBounds(graphicsHandler, new Color(255, 0, 0, 100));
        }
        */

    }
}
