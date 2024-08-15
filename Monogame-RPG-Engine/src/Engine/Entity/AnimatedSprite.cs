using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Engine.Scene;
using App.NPCs;

/*
	Represents an animated sprite
	Animations can either be passed in directly or loaded automatically in a subclass by overriding the getAnimations method
	This class contains logic for transitioning animations as well as playing out the frames in an animation in a loop
	Subclasses need to call down to this class's update method in order for animation logic to be performed
	While this calls does not extend from Sprite, it is set up in a way where it is still treated by other classes as if it is a singular sprite (based on value of currentFrame)
*/

namespace Engine.Entity
{
    public class AnimatedSprite : IntersectableRectangle
    {
        // location of entity
        protected float x;
        public virtual float X
        {
            get
            {
                return CurrentFrame.X;
            }
            set
            {
                x = value;
                CurrentFrame.X = value;
            }
        }

        protected float y;
        public virtual float Y
        {
            get
            {
                return CurrentFrame.Y;
            }
            set
            {
                y = value;
                CurrentFrame.Y = value;
            }
        }

        public virtual float X1
        {
            get
            {
                return CurrentFrame.X1;
            }
        }

        public virtual float X2
        {
            get
            {
                return CurrentFrame.X2;
            }
        }

        public virtual float Y1
        {
            get
            {
                return CurrentFrame.Y1;
            }
        }

        public virtual float Y2
        {
            get
            {
                return CurrentFrame.Y2;
            }
        }

        public virtual Utils.Point Location
        {
            get
            {
                return CurrentFrame.Location;
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public virtual float Scale
        {
            get
            {
                return CurrentFrame.Scale;
            }
            set
            {
                CurrentFrame.Scale = value;
            }
        }

        public virtual int Width
        {
            get
            {
                return CurrentFrame.Width;
            }
            set
            {
                CurrentFrame.Width = value;
            }
        }

        public virtual int Height
        {
            get
            {
                return CurrentFrame.Height;
            }
            set
            {
                CurrentFrame.Height = value;
            }
        }

        public virtual Rectangle Bounds
        {
            get
            {
                return CurrentFrame.Bounds;
            }
            set
            {
                CurrentFrame.Bounds = value;
            }
        }

        // maps animation name to an array of Frames representing one animation
        public Dictionary<string, Frame[]> Animations { get; private set; }

        // keeps track of current animation the sprite is using
        private string currentAnimationName = "";
        public string CurrentAnimationName
        {
            get
            {
                return currentAnimationName;
            }
            set
            {
                currentAnimationName = value;
                if (PreviousAnimationName != value)
                {
                    ResetAnimation();
                }
            }
        }
        public string PreviousAnimationName { get; private set; } = "";

        // keeps track of current frame number in an animation the sprite is using
        public int CurrentFrameIndex { get; set; }

        // if an animation has looped, this is set to true
        public bool HasAnimationLooped { get; private set; }

        private int frameDelayCounter;

        // gets the frame from current animation that the animated sprite class is currently using
        public Frame CurrentFrame
        {
            get
            {
                return Animations[CurrentAnimationName][CurrentFrameIndex];
            }
        }

        // gets the animation that the animated sprite class is currently using
        public Frame[] CurrentAnimation
        {
            get
            {
                return Animations[CurrentAnimationName];
            }
        }

        public AnimatedSprite(SpriteSheet spriteSheet, float x, float y, string startingAnimationName = "DEFAULT")
        {
            Animations = LoadAnimations(spriteSheet);
            CurrentAnimationName = startingAnimationName;
            CurrentFrameIndex = 0;
            X = x;
            Y = y;
        }

        public AnimatedSprite(float x, float y, Dictionary<string, Frame[]> animations, string startingAnimationName = "DEFAULT")
        {
            Animations = animations;
            CurrentAnimationName = startingAnimationName;
            CurrentFrameIndex = 0;
            X = x;
            Y = y;
        }

        public AnimatedSprite(float x, float y, Frame[] frames)
        {
            Animations = new Dictionary<string, Frame[]>
            {
                { "DEFAULT", frames }
            };
            CurrentAnimationName = "DEFAULT";
            CurrentFrameIndex = 0;
            X = x;
            Y = y;
        }

        public AnimatedSprite(float x, float y, Frame frame)
        {
            Animations = new Dictionary<string, Frame[]>
            {
                { "DEFAULT", new Frame[] { frame } }
            };
            CurrentAnimationName = "DEFAULT";
            CurrentFrameIndex = 0;
            X = x;
            Y = y;
        }

        public virtual void Initialize() { }

        public virtual void Update()
        {
            // if animation name has been changed (previous no longer equals current), setup for the new animation and start using it
            if (PreviousAnimationName != CurrentAnimationName)
            {
                ResetAnimation();
            }
            else
            {
                // if animation has more than one frame, check if it's time to transition to a new frame based on that frame's delay
                if (CurrentAnimation.Length > 1 && CurrentFrame.Delay > 0)
                {
                    frameDelayCounter--;

                    // if enough time has passed based on current frame's delay and it's time to transition to a new frame,
                    // update frame index to the next frame
                    // It will also wrap around back to the first frame index if it was already on the last frame index (the animation will loop)
                    if (frameDelayCounter == 0)
                    {
                        CurrentFrameIndex++;
                        if (CurrentFrameIndex >= Animations[CurrentAnimationName].Length)
                        {
                            CurrentFrameIndex = 0;
                            HasAnimationLooped = true;
                        }
                        frameDelayCounter = CurrentFrame.Delay;
                        SetLocation(x, y);
                    }
                }
            }
            PreviousAnimationName = CurrentAnimationName;
        }

        // resets current animation
        // this method is used when switching animations to "freshly load/reset" the new animation
        protected void ResetAnimation()
        {
            CurrentFrameIndex = 0;
            SetLocation(x, y);
            frameDelayCounter = CurrentFrame.Delay;
            HasAnimationLooped = false;
        }

        // Subclasses can override this method in order to add their own animations, which will be loaded in at initialization time
        public virtual Dictionary<string, Frame[]> LoadAnimations(SpriteSheet spriteSheet)
        {
            return null;
        }

        public virtual void Draw(GraphicsHandler graphicsHandler)
        {
            CurrentFrame.Draw(graphicsHandler);
        }

        public virtual void DrawBounds(GraphicsHandler graphicsHandler, Color color)
        {
            CurrentFrame.DrawBounds(graphicsHandler, color);
        }

        public virtual void SetLocation(float x, float y)
        {
            X = x;
            Y = y;
        }

        public virtual void MoveX(float dx)
        {
            X += dx;
        }

        public virtual void MoveRight(float dx)
        {
            X += dx;
        }

        public virtual void MoveLeft(float dx)
        {
            X -= dx;
        }

        public virtual void MoveY(float dy)
        {
            Y += dy;
        }

        public virtual void MoveDown(float dy)
        {
            Y += dy;
        }

        public virtual void MoveUp(float dy)
        {
            Y -= dy;
        }

        public virtual Rectangle GetIntersectRectangle()
        {
            return CurrentFrame.GetIntersectRectangle();
        }

        public virtual bool Intersects(IntersectableRectangle other)
        {
            return CurrentFrame.Intersects(other);
        }

        public virtual bool Touching(IntersectableRectangle other)
        {
            return CurrentFrame.Touching(other);
        }

        public virtual float GetAreaOverlapped(IntersectableRectangle other)
        {
            return CurrentFrame.GetAreaOverlapped(other);
        }

        public override string ToString()
        {
            return string.Format("Current Sprite: x={0} y={1} width={2} height={3} bounds=({4}, {5}, {6}, {7})", X, Y, Width, Height, Bounds.X1, Bounds.Y1, Bounds.Width, Bounds.Height);
        }

    }
}
