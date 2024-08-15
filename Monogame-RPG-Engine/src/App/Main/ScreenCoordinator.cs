using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Engine.Core;
using App.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace App.Main
{
    public class ScreenCoordinator : Screen
    {
        // currently shown Screen
        private Screen currentScreen = new DefaultScreen();

        // keep track of gameState so ScreenCoordinator knows which Screen to show
        public GameState GameState { get; set; }
        protected GameState previousGameState;

        public ScreenCoordinator()
        {
            GameState = GameState.MENU;
            previousGameState = GameState;
            UpdateCurrentScreen();
        }

        public override void Initialize()
        {
            // start game off with Menu Screen
            GameState = GameState.MENU;
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            do
            {
                if (previousGameState != GameState)
                {
                    currentScreen.UnloadContent();
                    UpdateCurrentScreen();
                }
                previousGameState = GameState;

                currentScreen.Update(gameTime, keyboardState);
            } while (previousGameState != GameState);
        }

        private void UpdateCurrentScreen()
        {
            switch (GameState)
            {
                case GameState.MENU:
                    currentScreen = new MenuScreen(this);
                    break;
                case GameState.LEVEL:
                    currentScreen = new PlayLevelScreen(this);
                    break;
                case GameState.CREDITS:
                    currentScreen = new CreditsScreen(this);
                    break;
            }
            currentScreen.Initialize();
            currentScreen.LoadContent();
        }

        public override void Draw(GraphicsHandler graphicsHandler)
        {
            currentScreen.Draw(graphicsHandler);
        }
    }
}
