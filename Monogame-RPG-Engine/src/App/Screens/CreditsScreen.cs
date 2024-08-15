using App.Main;
using App.Maps;
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
using static App.Resources.FontsHelper;

// This is the class for the credits screen
namespace App.Screens
{
    public class CreditsScreen : Screen
    {
        protected ScreenCoordinator screenCoordinator;
        protected Map background;
        protected KeyLocker keyLocker = new KeyLocker();
        protected DynamicSpriteFontGraphic creditsLabel;
        protected DynamicSpriteFontGraphic createdByLabel;
        protected DynamicSpriteFontGraphic returnInstructionsLabel;

        public CreditsScreen(ScreenCoordinator screenCoordinator)
        {
            this.screenCoordinator = screenCoordinator;
        }

        public override void Initialize()
        {
            background = new TitleScreenMap(ContentLoader);
            background.AdjustCamera = false;
            keyLocker.LockKey(Keys.Space);

            creditsLabel = new DynamicSpriteFontGraphic("Credits", ContentLoader.LoadTrueTypeFont(TrueTypeFonts.TIMES_NEW_ROMAN), 32, new Vector2(15, 7), Color.White);
            createdByLabel = new DynamicSpriteFontGraphic("Created by Alex Thimineur", ContentLoader.LoadTrueTypeFont(TrueTypeFonts.TIMES_NEW_ROMAN), 22, new Vector2(130, 121), Color.White);
            returnInstructionsLabel = new DynamicSpriteFontGraphic("Press Space to return to the menu", ContentLoader.LoadTrueTypeFont(TrueTypeFonts.TIMES_NEW_ROMAN), 32, new Vector2(20, 532), Color.White);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            // update background map (to play tile animations)
            background.Update(keyboardState);

            if (keyboardState.IsKeyUp(Keys.Space))
            {
                keyLocker.UnlockKey(Keys.Space);
            }

            // if space is pressed, go back to main menu
            if (!keyLocker.IsKeyLocked(Keys.Space) && keyboardState.IsKeyDown(Keys.Space))
            {
                screenCoordinator.GameState = GameState.MENU;
            }
        }

        public override void Draw(GraphicsHandler graphicsHandler)
        {
            background.Draw(graphicsHandler);
            creditsLabel.Draw(graphicsHandler);
            createdByLabel.Draw(graphicsHandler);
            returnInstructionsLabel.Draw(graphicsHandler);
        }
    }
}
