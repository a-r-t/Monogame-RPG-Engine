using Engine.Core;
using Engine.FontGraphics;
using Engine.Scene.MapCore;
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
namespace Engine.Scene.TextboxCore
{
    public class Textbox
    {
        // whether textbox is shown or not
        public bool IsActive { get; set; }

        // textbox constants
        public int X { get; set; } = 22;
        public int BottomY { get; set; } = 460;
        public int TopY { get; set; } = 22;
        public int FontX { get; set; } = 35;
        public int FontBottomY { get; set; } = 475;
        public int FontTopY { get; set; } = 37;
        public int Width { get; set; } = 750;
        public int Height { get; set; } = 100;

        // options textbox constants
        public int OptionX { get; set; } = 680;
        public int OptionBottomY { get; set; } = 350;
        public int OptionTopY { get; set; } = 130;
        public int OptionWidth { get; set; } = 92;
        public int OptionHeight { get; set; } = 100;
        public int FontOptionX { get; set; } = 706;
        public int FontOptionBottomYStart { get; set; } = 368;
        public int FontOptionTopYStart { get; set; } = 148;
        public int FontOptionSpacing { get; set; } = 35;
        public int OptionPointerX { get; set; } = 690;
        public int OptionPointerYBottomStart { get; set; } = 378;
        public int OptionPointerYTopStart { get; set; } = 158;

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
        private TrueTypeFont textboxFontGraphic;

        private ContentLoader contentLoader;
        private Map map;

        public int LastOptionIndexSelected = -1;

        public Textbox(ContentLoader contentLoader)
        {
            this.textQueue = new Queue<TextboxItem>();
            this.contentLoader = contentLoader;
            textboxFontGraphic = contentLoader.LoadTrueTypeFont(textboxFont);
        }

        public void SetMap(Map map)
        {
            this.map = map;
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

        public void Update(KeyboardState keyboardState)
        {
            // if textQueue has more text to display and the interact key button was pressed previously, display new text
            if (textQueue.Count > 0 && keyLocker.IsKeyLocked(InteractKey))
            {
                currentTextItem = textQueue.Peek();
                options = null;

                // if camera is at bottom of screen, text is drawn at top of screen instead of the bottom like usual
                // to prevent it from covering the player
                int fontY = map != null && !map.Camera.IsAtBottomOfMap() ? FontBottomY : FontTopY;

                // create text spritefont that will be drawn in textbox
                text = new DynamicSpriteFontGraphic(currentTextItem.Text, textboxFontGraphic, textboxFontSize, new Vector2(FontX, fontY), Color.Black);

                // if there are options associated with this text item, prepare option spritefont text to be drawn in options textbox
                if (currentTextItem.Options != null)
                {
                    // if camera is at bottom of screen, text is drawn at top of screen instead of the bottom like usual
                    // to prevent it from covering the player
                    int fontOptionY = map != null && !map.Camera.IsAtBottomOfMap() ? FontOptionBottomYStart : FontOptionTopYStart;

                    options = new List<DynamicSpriteFontGraphic>();
                    // for each option, crate option text spritefont that will be drawn in options textbox
                    for (int i = 0; i < currentTextItem.Options.Count; i++)
                    {
                        options.Add(new DynamicSpriteFontGraphic(currentTextItem.Options[i], textboxFontGraphic, textboxFontSize, new Vector2(FontOptionX, fontOptionY + (i * FontOptionSpacing)), Color.Black));
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
                    LastOptionIndexSelected = selectedOptionIndex;
                    if (map != null)
                    {
                        map.ActiveScript.ScriptActionOutputManager.AddFlag("TEXTBOX_OPTION_SELECTION", selectedOptionIndex);
                    }
                }
            }
            else if (keyboardState.IsKeyUp(InteractKey))
            {
                keyLocker.UnlockKey(InteractKey);
            }

            if (options != null)
            {
                if (keyboardState.IsKeyDown(Keys.Down) && !keyLocker.IsKeyLocked(Keys.Down))
                {
                    keyLocker.LockKey(Keys.Down);
                    if (selectedOptionIndex < options.Count - 1)
                    {
                        selectedOptionIndex++;
                    }
                }
                if (keyboardState.IsKeyDown(Keys.Up) && !keyLocker.IsKeyLocked(Keys.Up))
                {
                    keyLocker.LockKey(Keys.Up);
                    if (selectedOptionIndex > 0)
                    {
                        selectedOptionIndex--;
                    }
                }
                if (keyboardState.IsKeyUp(Keys.Down))
                {
                    keyLocker.UnlockKey(Keys.Down);
                }
                if (keyboardState.IsKeyUp(Keys.Up))
                {
                    keyLocker.UnlockKey(Keys.Up);
                }
            }
        }

        public void Draw(GraphicsHandler graphicsHandler)
        {
            // draw textbox
            // if camera is at bottom of screen, textbox is drawn at top of screen instead of the bottom like usual
            // to prevent it from covering the player
            int y = map != null && !map.Camera.IsAtBottomOfMap() ? BottomY : TopY;
            graphicsHandler.DrawFilledRectangleWithBorder(new Rectangle(X, y, Width, Height), Color.White, Color.Black, 2);

            if (text != null)
            {
                // draw text in textbox
                text.DrawWithParsedNewLines(graphicsHandler, 10);

                if (options != null)
                {
                    // draw options textbox
                    // if camera is at bottom of screen, textbox is drawn at top of screen instead of the bottom like usual
                    // to prevent it from covering the player
                    int optionY = map != null && !map.Camera.IsAtBottomOfMap() ? OptionBottomY : OptionTopY;
                    graphicsHandler.DrawFilledRectangleWithBorder(new Rectangle(OptionX, optionY, OptionWidth, OptionHeight), Color.White, Color.Black, 2);

                    // draw each option text
                    foreach (DynamicSpriteFontGraphic option in options)
                    {
                        option.Draw(graphicsHandler);
                    }

                    // the start y location of the option pointer depends on whether the options textbox is on top or bottom of screen
                    int optionPointerYStart = map != null && !map.Camera.IsAtBottomOfMap() ? OptionPointerYBottomStart : OptionPointerYTopStart;
                    // draw option selection indicator (small black rectangle)
                    graphicsHandler.DrawFilledRectangle(OptionPointerX, optionPointerYStart + (selectedOptionIndex * FontOptionSpacing), 10, 10, Color.Black);
                }
            }
        }
    }
}
