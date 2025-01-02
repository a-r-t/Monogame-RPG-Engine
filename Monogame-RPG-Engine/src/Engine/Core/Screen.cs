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
        public int X { get; set; }
        public int Y { get; set; }
        private int width = 1;
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                if (width > 0)
                {
                    width = value;
                    CreateRenderTarget();
                }
            }
        }
        private int height = 1;
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                if (height > 0)
                {
                    height = value;
                    CreateRenderTarget();
                }
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(X, Y, Width, Height);
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
            renderTarget = new RenderTarget2D(GameLoop.GraphicsDeviceInstance, Width, Height);
        }

        public void SetBounds(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        // all screens share this global content loader for content that is designed to be used everywhere
        public static ContentLoader GlobalContentLoader;

        public Screen()
        {
            ContentLoader = ContentLoader.Create();
            SetBounds(0, 0, ScreenManager.WindowWidth, ScreenManager.WindowHeight);
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
                graphicsHandler.DrawRenderTarget(X, Y);
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
