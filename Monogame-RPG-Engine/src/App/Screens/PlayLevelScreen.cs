using App.Listeners;
using App.Main;
using App.Maps;
using App.Players;
using Engine.Core;
using Engine.FontGraphics;
using Engine.Scene;
using Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// This class is for the win level screen
namespace App.Screens
{
    public class PlayLevelScreen : Screen, GameListener
    {
        protected ScreenCoordinator screenCoordinator;
        protected Map map;
        protected Player player;
        protected Textbox textbox;
        protected PlayLevelScreenStates PlayLevelScreenState { get; private set; }
        protected WinScreen winScreen;
        protected FlagManager flagManager;

        public PlayLevelScreen(ScreenCoordinator screenCoordinator)
        {
            this.screenCoordinator = screenCoordinator;
        }

        public override void Initialize()
        {
            // setup state
            flagManager = new FlagManager();
            flagManager.AddFlag("hasLostBall", false);
            flagManager.AddFlag("hasTalkedToWalrus", false);
            flagManager.AddFlag("hasTalkedToDinosaur", false);
            flagManager.AddFlag("hasFoundBall", false);

            // define/setup map
            map = new TestMap(ContentLoader);
            map.FlagManager = flagManager;

            // setup player
            player = new Cat(map.PlayerStartPosition.X, map.PlayerStartPosition.Y, ContentLoader);
            player.SetMap(map);
            PlayLevelScreenState = PlayLevelScreenStates.RUNNING;
            player.FacingDirection = Direction.LEFT;

            map.Player = player;


            map.Listeners.Add(this);

            // preloads all scripts ahead of time rather than loading them dynamically
            // both are supported, however preloading is recommended
            map.PreloadScripts();

            textbox = new Textbox(ContentLoader);
            textbox.LoadContent();
            textbox.setMap(map);
            textbox.InteractKey = player.INTERACT_KEY;
            map.Textbox = textbox;

            winScreen = new WinScreen(this);
            winScreen.Initialize();
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            // based on screen state, perform specific actions
            switch (PlayLevelScreenState)
            {
                // if level is "running" update player and map to keep game logic for the platformer level going
                case PlayLevelScreenStates.RUNNING:
                    player.Update(keyboardState);
                    map.Update(keyboardState);
                    if (textbox.IsActive)
                    {
                        textbox.Update(keyboardState);
                    }
                    break;
                // if level has been completed, bring up level cleared screen
                case PlayLevelScreenStates.LEVEL_COMPLETED:
                    winScreen.Update(gameTime, keyboardState);
                    break;
            }
        }

        public override void Draw(GraphicsHandler graphicsHandler)
        {
            // based on screen state, draw appropriate graphics
            switch (PlayLevelScreenState)
            {
                case PlayLevelScreenStates.RUNNING:
                    map.Draw(player, graphicsHandler);
                    if (textbox.IsActive)
                    {
                        textbox.Draw(graphicsHandler);
                    }
                    break;
                case PlayLevelScreenStates.LEVEL_COMPLETED:
                    winScreen.Draw(graphicsHandler);
                    break;
            }
        }

        public void ResetLevel()
        {
            Initialize();
            LoadContent();
        }

        public void GoBackToMenu()
        {
            screenCoordinator.GameState = GameState.MENU;

        }

        public void OnWin()
        {
            PlayLevelScreenState = PlayLevelScreenStates.LEVEL_COMPLETED;
        }

        // This enum represents the different states this screen can be in
        public enum PlayLevelScreenStates
        {
            RUNNING, LEVEL_COMPLETED
        }
    }
}
