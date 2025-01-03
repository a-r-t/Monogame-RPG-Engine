using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Engine.Core
{
    public abstract class Screen
    {
        // each individual screen has access to its own content loader
        public ContentLoader ContentLoader { get; private set; }

        // screen render target instance
        private RenderTarget2D renderTarget;

        // location for screen to be drawn
        public int ScreenX { get; set; }
        public int ScreenY { get; set; }

        // bounds of screen for render target
        public int ScreenWidth { get; private set; } = 1;
        public int ScreenHeight { get; private set; } = 1;

        public System.Drawing.Rectangle ScreenBounds
        {
            get
            {
                return new System.Drawing.Rectangle(ScreenX, ScreenY, ScreenWidth, ScreenHeight);
            }
            set
            {
                ScreenX = value.X;
                ScreenY = value.Y;
                SetScreenDimensions(value.Width, value.Height);
            }
        }

        // wrapper for the above auto property setter
        public void SetScreenBounds(int x, int y, int width, int height)
        {
            ScreenBounds = new System.Drawing.Rectangle(x, y, width, height);
        }

        private bool useRenderTarget;
        public bool UseRenderTarget
        {
            get
            {
                return useRenderTarget;
            }
            set
            {
                useRenderTarget = value;
                if (value)
                {
                    SetScreenBounds(ScreenX, ScreenY, ScreenWidth, ScreenHeight);
                }
                else
                {
                    renderTarget = null;
                }
            }
        }

        public Microsoft.Xna.Framework.Color? ScreenBackgroundColor { get; set; } = null;

        // create a new render target using screen bounds
        public void CreateRenderTarget()
        {
            renderTarget = new RenderTarget2D(GameLoop.GraphicsDeviceInstance, ScreenWidth, ScreenHeight);
        }

        public void SetScreenDimensions(int width, int height)
        {
            if (width > 0 && height > 0)
            {
                ScreenWidth = width;
                ScreenHeight = height;
                // if render target is turned on, AND if render target has not yet been created OR the current render target's dimensions are different than the desired ones, recreate the render target instance
                // this limits creating a new render target instance to only when necessary, which is better for overall game performance
                if (useRenderTarget && (renderTarget == null || renderTarget.Width != width || renderTarget.Height != height))
                {
                    CreateRenderTarget();
                }
            }
            else
            {
                throw new Exception($"Unable to create Screen render target of size (w: {width}, h: {height}) -- invalid dimensions (note: width and height MUST be greater than 0).");
            }
        }

        // all screens share this global content loader for content that is designed to be used everywhere
        public static ContentLoader GlobalContentLoader;

        public Screen()
        {
            ContentLoader = ContentLoader.Create();
            SetScreenBounds(0, 0, ScreenManager.WindowWidth, ScreenManager.WindowHeight);

            // this allows for a subclass to override Draw while still ensuring the render target logic will be enforced in the Render method
            DrawReference = Draw;

            UseRenderTarget = true;
        }

        static Screen()
        {
            GlobalContentLoader = ContentLoader.Create();
        }

        public virtual void Initialize() { }
        public virtual void LoadContent() { }
        public virtual void UnloadContent()
        {
            ContentLoader.Unload();
        }
        public virtual void Update(GameTime gameTime, KeyboardState keyboardState) { }

        protected virtual void Draw(GraphicsHandler graphicsHandler) { }

        private Action<GraphicsHandler> DrawReference = (graphicsHandler) => { };

        // if render target is in use, it sets the render target first, then draws the screen's content to that render target, and then draws the entire render target
        public void Render(GraphicsHandler graphicsHandler, Microsoft.Xna.Framework.Color? color = null)
        {
            if (useRenderTarget)
            {
                // setup render target
                graphicsHandler.SetRenderTarget(renderTarget);

                // fill render target background if ScreenBackgroundColor is set
                if (ScreenBackgroundColor.HasValue)
                {
                    graphicsHandler.DrawFilledRectangle(new Microsoft.Xna.Framework.Rectangle(0, 0, ScreenWidth, ScreenHeight), ScreenBackgroundColor.Value);
                }

                // apply draw content from screen class to render target
                DrawReference.Invoke(graphicsHandler);

                // draw finished render target at appropriate location
                graphicsHandler.DrawRenderTarget(ScreenX, ScreenY, color);
            }
            else
            {
                DrawReference.Invoke(graphicsHandler);
            }
        }

        // warning: if you need to call this, you likely have an asset loaded globally that shouldn't be
        public static void UnloadGlobalContent()
        {
            GlobalContentLoader.Unload();
        }
    }
}
