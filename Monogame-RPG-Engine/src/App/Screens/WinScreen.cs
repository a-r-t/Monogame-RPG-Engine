using App.Main;
using App.Maps;
using Engine.Core;
using Engine.FontGraphics;
using Engine.Scene;
using Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App.Resources.FontsHelper;

// This class is for the win level screen
namespace App.Screens
{
    public class WinScreen : Screen
    {
        protected DynamicSpriteFontGraphic winMessage;
        protected DynamicSpriteFontGraphic instructions;
        protected KeyLocker keyLocker = new KeyLocker();
        protected PlayLevelScreen playLevelScreen;

        public WinScreen(PlayLevelScreen playLevelScreen)
        {
            this.playLevelScreen = playLevelScreen;
        }

        public override void Initialize()
        {
            keyLocker.LockKey(Keys.Space);
            keyLocker.LockKey(Keys.Escape);
            winMessage = new DynamicSpriteFontGraphic("You win!", ContentLoader.LoadTrueTypeFont(TrueTypeFonts.ARIAL), 32, new Vector2(350, 239), Color.White);
            instructions = new DynamicSpriteFontGraphic("Press Space to play again or Escape to go back to the main menu", ContentLoader.LoadTrueTypeFont(TrueTypeFonts.ARIAL), 22, new Vector2(120, 279), Color.White);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyUp(Keys.Space))
            {
                keyLocker.UnlockKey(Keys.Space);
            }
            if (keyboardState.IsKeyUp(Keys.Escape))
            {
                keyLocker.UnlockKey(Keys.Escape);
            }

            // if space is pressed, reset level. if escape is pressed, go back to main menu
            if (keyboardState.IsKeyDown(Keys.Space) && !keyLocker.IsKeyLocked(Keys.Space))
            {
                playLevelScreen.ResetLevel();
            }
            else if (keyboardState.IsKeyDown(Keys.Escape) && !keyLocker.IsKeyLocked(Keys.Escape))
            {
                playLevelScreen.GoBackToMenu();
            }
        }

        public override void Draw(GraphicsHandler graphicsHandler)
        {
            graphicsHandler.DrawFilledRectangle(0, 0, ScreenManager.ScreenWidth, ScreenManager.ScreenHeight, Color.Black);
            winMessage.Draw(graphicsHandler);
            instructions.Draw(graphicsHandler);
        }
    }
}
