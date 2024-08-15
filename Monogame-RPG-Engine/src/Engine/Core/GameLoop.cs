using App.Resources;
using Engine.Extensions;
using Engine.FontGraphics;
using Engine.Utils;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Screens;
using System;
using static App.Resources.FontsHelper;

namespace Engine.Core
{
    public class GameLoop : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GraphicsHandler graphicsHandler;
        public ScreenManager ScreenManager { get; private set; }
        public static GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
        public static ContentManager ContentManager { get; private set; }
        public static GameServiceContainer GameServiceContainer { get; private set; }
        public static int ViewportWidth { get; private set; }
        public static int ViewportHeight { get; private set; }
        public static GameWindow GameWindow { get; private set; }
        private RenderTarget2D renderTarget;
        private FrameCounter frameCounter;
        private bool showFPS = false;
        private KeyLocker keyLocker = new KeyLocker();
        private DynamicSpriteFontGraphic fpsLabel;
        private ContentLoader contentLoader;

        public GameLoop()
        {
            graphics = new GraphicsDeviceManager(this);
            GraphicsDeviceManager = graphics;

            // holding on to these graphics settings in the hopes I can figure out how to best utilize them later on, but right now they make the game choppy
            // graphics.PreferMultiSampling = true;
            // graphics.SynchronizeWithVerticalRetrace = false;

            Content.RootDirectory = "Content";
            IsFixedTimeStep = true;
            IsMouseVisible = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / Config.FPS);
            ContentManager = Content;
            GameServiceContainer = Services;
            graphics.PreferredBackBufferWidth = Config.GAME_WINDOW_WIDTH;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = Config.GAME_WINDOW_HEIGHT;   // set this value to the desired height of your window
            graphics.ApplyChanges();
            ViewportWidth = GraphicsDevice.Viewport.Width;
            ViewportHeight = GraphicsDevice.Viewport.Height;

            // holding on to window resize event code just in case I ever decide to allow this
            // this.Window.AllowUserResizing = true;
            // this.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
            
            GameWindow = Window;

            ScreenManager = new ScreenManager();
            ScreenManager.Initialize(new Rectangle(0, 0, Config.GAME_WINDOW_WIDTH, Config.GAME_WINDOW_HEIGHT));

            frameCounter = new FrameCounter();

            this.contentLoader = ContentLoader.Create();
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphicsHandler = new GraphicsHandler(GraphicsDevice, spriteBatch);

            renderTarget = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);

            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            fpsLabel = new DynamicSpriteFontGraphic("FPS", contentLoader.LoadTrueTypeFont(TrueTypeFonts.ARIAL), 12, new Vector2(4, 3), Color.Black);

        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            ScreenManager.Update(gameTime, keyboardState);

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            frameCounter.Update(deltaTime);

            if (keyboardState.IsKeyDown(Keys.G) && !keyLocker.IsKeyLocked(Keys.G))
            {
                showFPS = !showFPS;
                keyLocker.LockKey(Keys.G);
            }
            else if (keyLocker.IsKeyLocked(Keys.G) && keyboardState.IsKeyUp(Keys.G))
            {
                keyLocker.UnlockKey(Keys.G);
            }
            fpsLabel.Text = $"FPS: {frameCounter.AverageFramesPerSecond.Round()}";

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the entire scene in the given render target.
        /// </summary>
        /// <returns>A texture2D with the scene drawn in it.</returns>
        protected void DrawSceneToTexture(RenderTarget2D renderTarget)
        {
            // Set the render target
            GraphicsDevice.SetRenderTarget(renderTarget);

            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            // Draw the scene
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

            ScreenManager.Draw(graphicsHandler);

            if (showFPS)
            {
                fpsLabel.Draw(graphicsHandler);
            }

            spriteBatch.End();

            // Drop the render target
            GraphicsDevice.SetRenderTarget(null);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            DrawSceneToTexture(renderTarget);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            spriteBatch.Draw(renderTarget, new Rectangle(0, 0, Config.GAME_WINDOW_WIDTH, Config.GAME_WINDOW_HEIGHT), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}