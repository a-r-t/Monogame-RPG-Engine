using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Core
{
    public abstract class Screen
    {
        // each individual screen has access to its own content loader
        public ContentLoader ContentLoader { get; private set; }
        private RenderTarget2D renderTarget;

        // bounds of screen
        public int ScreenX { get; set; }
        public int ScreenY { get; set; }
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

        public Rectangle ScreenBounds
        {
            get
            {
                return new Rectangle(ScreenX, ScreenY, ScreenWidth, ScreenHeight);
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

        public void Render(GraphicsHandler graphicsHandler) 
        {
            if (useRenderTarget)
            {
                graphicsHandler.SetRenderTarget(renderTarget);
                DrawReference.Invoke(graphicsHandler);
                graphicsHandler.DrawRenderTarget(ScreenX, ScreenY);
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
