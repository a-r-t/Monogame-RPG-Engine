using Engine.Core;
using Engine.FontGraphics;
using Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App.Resources.FontsHelper;

// Represents the game's textbox
// Will display the text it is given to its textQueue
// Each String in the textQueue will be displayed in the textbox, and hitting the interact key will cycle between additional Strings in the queue
// Use the newline character in a String in the textQueue to break the text up into a second line if needed
// Also supports adding options for a player to select from
namespace Engine.Scene
{
    public class Textbox
    {
        // whether textbox is shown or not
        public bool IsActive { get; set; }

        // textbox constants
        protected const int x = 22;
        protected const int bottomY = 460;
        protected const int topY = 22;
        protected const int fontX = 35;
        protected const int fontBottomY = 475;
        protected const int fontTopY = 37;
        protected const int width = 750;
        protected const int height = 100;

        // options textbox constants
        protected const int optionX = 680;
        protected const int optionBottomY = 350;
        protected const int optionTopY = 130;
        protected const int optionWidth = 92;
        protected const int optionHeight = 100;
        protected const int fontOptionX = 706;
        protected const int fontOptionBottomYStart = 368;
        protected const int fontOptionTopYStart = 148;
        protected const int fontOptionSpacing = 35;
        protected const int optionPointerX = 690;
        protected const int optionPointerYBottomStart = 378;
        protected const int optionPointerYTopStart = 158;

        // core vars that make textbox work
        private Queue<TextboxItem> textQueue;
        private TextboxItem currentTextItem;
        protected int selectedOptionIndex = 0;
        private DynamicSpriteFontGraphic text = null;
        private List<DynamicSpriteFontGraphic> options = null;
        private KeyLocker keyLocker = new KeyLocker();
        public Keys InteractKey { get; set; } = Keys.Space;
        private string textboxFont = TrueTypeFonts.ARIAL;
        private int textboxFontSize = 30;

        private Map map;

        public Textbox(Map map)
        {
            this.map = map;
            this.textQueue = new Queue<TextboxItem>();
        }

        public void AddText(string text)
        {
            if (textQueue.Count == 0)
            {
                keyLocker.LockKey(InteractKey);
            }
            textQueue.Enqueue(new TextboxItem(text));
        }

        public void AddText(string[] text)
        {
            if (textQueue.Count == 0)
            {
                keyLocker.LockKey(InteractKey);
            }
            foreach (string textItem in text)
            {
                textQueue.Enqueue(new TextboxItem(textItem));
            }
        }

        public void AddText(TextboxItem text)
        {
            if (textQueue.Count == 0)
            {
                keyLocker.LockKey(InteractKey);
            }
            textQueue.Enqueue(text);
        }

        public void AddText(TextboxItem[] text)
        {
            if (textQueue.Count == 0)
            {
                keyLocker.LockKey(InteractKey);
            }
            foreach (TextboxItem textItem in text)
            {
                textQueue.Enqueue(textItem);
            }
        }

        // returns whether the textQueue is out of items to display or not
        // useful for scripts to know when to complete
        public bool IsTextQueueEmpty()
        {
            return textQueue.Count == 0;
        }

        public void LoadContent()
        {
        }

        public void Update(KeyboardState keyboardState)
        {
            // if textQueue has more text to display and the interact key button was pressed previously, display new text
            if (textQueue.Count > 0 && keyLocker.IsKeyLocked(InteractKey))
            {
                currentTextItem = textQueue.Peek();
                options = null;

                // if camera is at bottom of screen, text is drawn at top of screen instead of the bottom like usual
                // to prevent it from covering the player
                int fontY = !map.Camera.IsAtBottomOfMap() ? fontBottomY : fontTopY;

                // create text spritefont that will be drawn in textbox
                text = new DynamicSpriteFontGraphic(currentTextItem.Text, map.ContentLoader.LoadTrueTypeFont(textboxFont), textboxFontSize, new Vector2(fontX, fontY), Color.Black);

                // if there are options associated with this text item, prepare option spritefont text to be drawn in options textbox
                if (currentTextItem.Options != null)
                {
                    // if camera is at bottom of screen, text is drawn at top of screen instead of the bottom like usual
                    // to prevent it from covering the player
                    int fontOptionY = !map.Camera.IsAtBottomOfMap() ? fontOptionBottomYStart : fontOptionTopYStart;

                    options = new List<DynamicSpriteFontGraphic>();
                    // for each option, crate option text spritefont that will be drawn in options textbox
                    for (int i = 0; i < currentTextItem.Options.Count; i++)
                    {
                        options.Add(new DynamicSpriteFontGraphic(currentTextItem.Options[i], map.ContentLoader.LoadTrueTypeFont(textboxFont), textboxFontSize, new Vector2(fontOptionX, fontOptionY + (i * fontOptionSpacing)), Color.Black));
                    }
                    selectedOptionIndex = 0;
                }

            }
            // if interact key is pressed, remove the current text from the queue to prepare for the next text item to be displayed
            if (keyboardState.IsKeyDown(InteractKey) && !keyLocker.IsKeyLocked(InteractKey))
            {
                keyLocker.LockKey(InteractKey);
                textQueue.Dequeue();

                // if an option was selected, set output manager flag to the index of the selected option
                // a script can then look at output manager later to see which option was selected and do with that information what it wants
                if (options != null)
                {
                    map.ActiveScript.ScriptActionOutputManager.AddFlag("TEXTBOX_OPTION_SELECTION", selectedOptionIndex);
                }
            }
            else if (keyboardState.IsKeyUp(InteractKey))
            {
                keyLocker.UnlockKey(InteractKey);
            }

            if (options != null)
            {
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    if (selectedOptionIndex < options.Count - 1)
                    {
                        selectedOptionIndex++;
                    }
                }
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    if (selectedOptionIndex > 0)
                    {
                        selectedOptionIndex--;
                    }
                }
            }
        }

        public void Draw(GraphicsHandler graphicsHandler)
        {
            // draw textbox
            // if camera is at bottom of screen, textbox is drawn at top of screen instead of the bottom like usual
            // to prevent it from covering the player
            int y = !map.Camera.IsAtBottomOfMap() ? bottomY : topY;
            graphicsHandler.DrawFilledRectangleWithBorder(new Rectangle(x, y, width, height), Color.White, Color.Black, 2);

            if (text != null)
            {
                // draw text in textbox
                text.DrawWithParsedNewLines(graphicsHandler, 10);

                if (options != null)
                {
                    // draw options textbox
                    // if camera is at bottom of screen, textbox is drawn at top of screen instead of the bottom like usual
                    // to prevent it from covering the player
                    int optionY = !map.Camera.IsAtBottomOfMap() ? optionBottomY : optionTopY;
                    graphicsHandler.DrawFilledRectangleWithBorder(new Rectangle(optionX, optionY, optionWidth, optionHeight), Color.White, Color.Black, 2);

                    // draw each option text
                    foreach (DynamicSpriteFontGraphic option in options)
                    {
                        option.Draw(graphicsHandler);
                    }

                    // the start y location of the option pointer depends on whether the options textbox is on top or bottom of screen
                    int optionPointerYStart = !map.Camera.IsAtBottomOfMap() ? optionPointerYBottomStart : optionPointerYTopStart;
                    // draw option selection indicator (small black rectangle)
                    graphicsHandler.DrawFilledRectangle(optionPointerX, optionPointerYStart + (selectedOptionIndex * fontOptionSpacing), 10, 10, Color.Black);
                }
            }
        }
    }
}
