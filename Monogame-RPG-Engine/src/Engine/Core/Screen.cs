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
        private int screenWidth = 1;
        public int ScreenWidth
        {
            get
            {
                return screenWidth;
            }
            set
            {
                if (screenWidth > 0)
                {
                    screenWidth = value;
                    CreateRenderTarget();
                }
            }
        }
        private int screenHeight = 1;
        public int ScreenHeight
        {
            get
            {
                return screenHeight;
            }
            set
            {
                if (screenHeight > 0)
                {
                    screenHeight = value;
                    CreateRenderTarget();
                }
            }
        }

        public System.Drawing.Rectangle ScreenBounds
        {
            get
            {
                return new System.Drawing.Rectangle(ScreenX, ScreenY, ScreenWidth, ScreenHeight);
            }
        }

        private bool useRenderTarget = true;
        public bool UseRenderTarget
        {
            get
            {
                return useRenderTarget;
            }
            set
            {
                useRenderTarget = value;
                if (useRenderTarget)
                {
                    CreateRenderTarget();
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

        public void SetScreenBounds(int x, int y, int width, int height)
        {
            ScreenX = x;
            ScreenY = y;
            ScreenWidth = width;
            ScreenHeight = height;
        }

        // all screens share this global content loader for content that is designed to be used everywhere
        public static ContentLoader GlobalContentLoader;

        public Screen()
        {
            ContentLoader = ContentLoader.Create();
            SetScreenBounds(0, 0, ScreenManager.WindowWidth, ScreenManager.WindowHeight);

            // this allows for a subclass to override Draw while still ensuring the render target logic will be enforced in the Render method
            DrawReference = Draw;
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
                graphicsHandler.SetRenderTarget(renderTarget);
                if (ScreenBackgroundColor.HasValue)
                {
                    graphicsHandler.DrawFilledRectangle(new Microsoft.Xna.Framework.Rectangle(0, 0, ScreenWidth, ScreenHeight), ScreenBackgroundColor.Value);
                }
                DrawReference.Invoke(graphicsHandler);
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
