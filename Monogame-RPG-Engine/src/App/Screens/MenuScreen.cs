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

// This is the class for the main menu screen
namespace App.Screens
{
    public class MenuScreen : Screen
    {
        protected ScreenCoordinator screenCoordinator;
        protected int currentMenuItemHovered = 0; // current menu item being "hovered" over
        protected int menuItemSelected = -1;
        protected DynamicSpriteFontGraphic playGame;
        protected DynamicSpriteFontGraphic credits;
        protected Map background;
        protected int keyPressTimer;
        protected int pointerLocationX, pointerLocationY;
        protected KeyLocker keyLocker = new KeyLocker();

        public MenuScreen(ScreenCoordinator screenCoordinator)
        {
            this.screenCoordinator = screenCoordinator;
        }

        public override void Initialize()
        {
            background = new TitleScreenMap(ContentLoader);
            background.AdjustCamera = false;
            keyPressTimer = 0;
            menuItemSelected = -1;
            keyLocker.LockKey(Keys.Space);

            playGame = new DynamicSpriteFontGraphic("PLAY GAME", ContentLoader.LoadTrueTypeFont(TrueTypeFonts.ARIAL), 32, new Vector2(200, 125), new Color(49, 207, 240));
            playGame.OutlineColor = Color.Black;
            playGame.OutlineThickness = 1;
            credits = new DynamicSpriteFontGraphic("CREDITS", ContentLoader.LoadTrueTypeFont(TrueTypeFonts.ARIAL), 32, new Vector2(200, 225), new Color(49, 207, 240));
            credits.OutlineColor = Color.Black;
            credits.OutlineThickness = 1;
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            // update background map (to play tile animations)
            background.Update(keyboardState);

            // if down or up is pressed, change menu item "hovered" over (blue square in front of text will move along with currentMenuItemHovered changing)
            if (keyboardState.IsKeyDown(Keys.Down) && keyPressTimer == 0)
            {
                keyPressTimer = 14;
                currentMenuItemHovered++;
            }
            else if (keyboardState.IsKeyDown(Keys.Up) && keyPressTimer == 0)
            {
                keyPressTimer = 14;
                currentMenuItemHovered--;
            }
            else
            {
                if (keyPressTimer > 0)
                {
                    keyPressTimer--;
                }
            }

            // if down is pressed on last menu item or up is pressed on first menu item, "loop" the selection back around to the beginning/end
            if (currentMenuItemHovered > 1)
            {
                currentMenuItemHovered = 0;
            }
            else if (currentMenuItemHovered < 0)
            {
                currentMenuItemHovered = 1;
            }

            // sets location for blue square in front of text (pointerLocation) and also sets color of spritefont text based on which menu item is being hovered
            if (currentMenuItemHovered == 0)
            {
                playGame.Color = new Color(255, 215, 0);
                credits.Color = new Color(49, 207, 240);
                pointerLocationX = 170;
                pointerLocationY = 130;
            }
            else if (currentMenuItemHovered == 1)
            {
                playGame.Color = new Color(49, 207, 240);
                credits.Color = new Color(255, 215, 0);
                pointerLocationX = 170;
                pointerLocationY = 230;
            }

            // if space is pressed on menu item, change to appropriate screen based on which menu item was chosen
            if (keyboardState.IsKeyUp(Keys.Space))
            {
                keyLocker.UnlockKey(Keys.Space);
            }
            if (!keyLocker.IsKeyLocked(Keys.Space) && keyboardState.IsKeyDown(Keys.Space))
            {
                menuItemSelected = currentMenuItemHovered;
                if (menuItemSelected == 0)
                {
                    screenCoordinator.GameState = GameState.LEVEL;
                }
                else if (menuItemSelected == 1)
                {
                    screenCoordinator.GameState = GameState.CREDITS;
                }
            }
        }

        public override void Draw(GraphicsHandler graphicsHandler)
        {
            background.Draw(graphicsHandler);
            playGame.Draw(graphicsHandler);
            credits.Draw(graphicsHandler);
            graphicsHandler.DrawFilledRectangleWithBorder(new Rectangle(pointerLocationX, pointerLocationY, 20, 20), new Color(49, 207, 240), Color.Black, 2);
        }
    }
}
