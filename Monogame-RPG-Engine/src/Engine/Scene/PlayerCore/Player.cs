using Engine.SpriteGraphics;
using Engine.Utils;
using Microsoft.Xna.Framework.Input;
using Engine.Scene.EntitiesCore;

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

            // if walk left key is pressed, move player to the left
            if (keyboardState.IsKeyDown(MOVE_LEFT_KEY))
            {
                moveAmountX -= walkSpeed;
                FacingDirection = Direction.LEFT;
                CurrentWalkingXDirection = Direction.LEFT;
            }

            // if walk right key is pressed, move player to the right
            else if (keyboardState.IsKeyDown(MOVE_RIGHT_KEY))
            {
                moveAmountX += walkSpeed;
                FacingDirection = Direction.RIGHT;
                CurrentWalkingXDirection = Direction.RIGHT;
            }
            else
            {
                CurrentWalkingXDirection = Direction.NONE;
            }

            LastWalkingYDirection = CurrentWalkingYDirection;

            if (keyboardState.IsKeyDown(MOVE_UP_KEY))
            {
                moveAmountY -= walkSpeed;
                FacingDirection = Direction.UP;
                CurrentWalkingYDirection = Direction.UP;
            }
            else if (keyboardState.IsKeyDown(MOVE_DOWN_KEY))
            {
                moveAmountY += walkSpeed;
                FacingDirection = Direction.DOWN;
                CurrentWalkingYDirection = Direction.DOWN;
            }
            else
            {
                CurrentWalkingYDirection = Direction.NONE;
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
                CurrentAnimationName = $"STAND_{GetFacingDirectionSuffix()}";
            }
            else if (PlayerState == PlayerState.WALKING)
            {
                CurrentAnimationName = $"WALK_{GetFacingDirectionSuffix()}";
            }
        }

        // used to make piecing together animation names based on facing direction easier
        protected string GetFacingDirectionSuffix()
        {

            switch (FacingDirection)
            {
                case Direction.LEFT: return "LEFT";
                case Direction.RIGHT: return "RIGHT";
                case Direction.UP: return "UP";
                case Direction.DOWN: return "DOWN";
                default: return "";
            }
        }

        public override void OnEndCollisionCheckX(bool hasCollided, Direction direction, GameObject entityCollidedWith)
        {

        }

        public override void OnEndCollisionCheckY(bool hasCollided, Direction direction, GameObject entityCollidedWith)
        {

        }

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
            CurrentAnimationName = $"STAND_{GetFacingDirectionSuffix()}";
        }

        public void Unlock()
        {
            isLocked = false;
            PlayerState = PlayerState.STANDING;
            CurrentAnimationName = $"STAND_{GetFacingDirectionSuffix()}";
        }

        // used by other files or scripts to force player to stand
        public void Stand(Direction direction)
        {
            PlayerState = PlayerState.STANDING;
            FacingDirection = direction;
            CurrentAnimationName = $"STAND_{GetFacingDirectionSuffix()}";
        }

        // used by other files or scripts to force player to walk
        public void Walk(Direction direction, float speed)
        {
            PlayerState = PlayerState.WALKING;
            FacingDirection = direction;
            CurrentAnimationName = $"WALK_{GetFacingDirectionSuffix()}";

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
