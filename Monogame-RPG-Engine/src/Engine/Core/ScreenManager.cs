using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

/*
 * The game engine uses this class to start off the cascading Screen updating/drawing
 * The idea is an external class should be allowed to set its own Screen to this class's currentScreen variable,
 * and then that class can handle coordinating which Screen to show.
 */
namespace Monogame_RPG_Engine.Engine.Core
{
    public class ScreenManager
    {
        // the current screen that is loaded
        private Screen currentScreen;

        // gets bounds of currentScreen -- can be called from anywhere in an application
        public static Rectangle WindowBounds { get; private set; } = new Rectangle(0, 0, 0, 0);

        // gets width of currentScreen -- can be called from anywhere in an application
        public static int WindowWidth
        {
            get
            {
                return WindowBounds.Width;
            }
        }

        // gets height of currentScreen -- can be called from anywhere in an application
        public static int WindowHeight
        {
            get
            {
                return WindowBounds.Height;
            }
        }

        public void Initialize(Rectangle screenBounds)
        {
            WindowBounds = screenBounds;
            SetCurrentScreen(new DefaultScreen());
        }

        // attach an external Screen class here for the ScreenManager to start calling its update/draw cycles
        public void SetCurrentScreen(Screen screen)
        {
            screen.Initialize();
            currentScreen = screen;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            currentScreen.Update(gameTime, keyboardState);
        }

        public void Draw(GraphicsHandler graphicsHandler)
        {
            currentScreen.Render(graphicsHandler);
        }
    }
}
