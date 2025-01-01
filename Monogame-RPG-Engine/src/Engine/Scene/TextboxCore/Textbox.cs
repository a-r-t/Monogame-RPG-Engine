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
        public int X { get; set; } = 16;
        public int BottomY { get; set; } = 460;
        public int TopY { get; set; } = 22;
        public int FontX { get; set; } = 35;
        public int FontBottomY { get; set; } = 475;
        public int FontTopY { get; set; } = 37;
        public int Width { get; set; } = 750;
        public int Height { get; set; } = 100;

        public Color FillColor { get; set; } = Color.White;
        public Color BorderColor { get; set; } = Color.Black;
        public int BorderThickness { get; set; } = 2;
        public Color TextColor { get; set; } = Color.Black;

        // how much spacing to apply between new lines in text drawn in textbox
        public int SpaceBetweenLines { get; set; } = 10;

        private TextboxPositionMode textboxPositionMode;
        public TextboxPositionMode TextboxPositionMode
        {
            get
            {
                return textboxPositionMode;
            }
            set
            {
                textboxPositionMode = value;
                OptionsBox.TextboxPositionMode = value;
            }
        }

        // core vars that make textbox work
        private Queue<TextboxItem> textQueue;
        private TextboxItem currentTextItem;
        private DynamicSpriteFontGraphic text = null;
        private KeyLocker keyLocker = new KeyLocker();
        public Keys InteractKey { get; set; } = Keys.Space;
        public TrueTypeFont TextboxFontGraphic { get; set; }
        public int TextboxFontSize { get; set; } = 30;

        private Map map;

        public int LastOptionIndexSelected { get; set; } = -1;

        public OptionsBox OptionsBox { get; set; }

        public Textbox(ContentLoader contentLoader)
        {
            this.textQueue = new Queue<TextboxItem>();
            TextboxFontGraphic = contentLoader.LoadTrueTypeFont(TrueTypeFonts.ARIAL);
            OptionsBox = new OptionsBox(contentLoader);
            TextboxPositionMode = TextboxPositionMode.DYNAMIC_BOTTOM_PREFERRED;
        }

        public void SetMap(Map map)
        {
            this.map = map;
            OptionsBox.SetMap(map);
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

                // if camera is at bottom of screen, text is drawn at top of screen instead of the bottom like usual
                // to prevent it from covering the player
                int fontY = GetValueBasedOnPositionMode(FontBottomY, FontTopY);

                // create text spritefont that will be drawn in textbox
                text = new DynamicSpriteFontGraphic(currentTextItem.Text, TextboxFontGraphic, TextboxFontSize, new Vector2(FontX, fontY), TextColor);

                // if there are options associated with this text item, prepare option spritefont text to be drawn in options textbox
                if (currentTextItem.HasOptions())
                {
                    OptionsBox.Initialize(currentTextItem);
                }
            }

            // if interact key is pressed, remove the current text from the queue to prepare for the next text item to be displayed
            if (keyboardState.IsKeyDown(InteractKey) && !keyLocker.IsKeyLocked(InteractKey))
            {
                keyLocker.LockKey(InteractKey);
                textQueue.Dequeue();

                // if an option was selected, set output manager flag to the index of the selected option
                // a script can then look at output manager later to see which option was selected and do with that information what it wants
                if (currentTextItem.HasOptions())
                {
                    LastOptionIndexSelected = OptionsBox.SelectedOptionIndex;
                    if (map != null)
                    {
                        map.ActiveScript.ScriptActionOutputManager.AddFlag("TEXTBOX_OPTION_SELECTION", OptionsBox.SelectedOptionIndex);
                    }
                }
            }
            else if (keyboardState.IsKeyUp(InteractKey))
            {
                keyLocker.UnlockKey(InteractKey);
            }

            if (currentTextItem.Options != null)
            {
                OptionsBox.Update(keyboardState);
            }
        }

        private int GetValueBasedOnPositionMode(int bottom, int top)
        {
            if (TextboxPositionMode == TextboxPositionMode.TOP)
            {
                return top;
            }
            else if (TextboxPositionMode == TextboxPositionMode.BOTTOM)
            {
                return bottom;
            }
            else
            {
                if (map == null)
                {
                    return bottom;
                }
                else
                {
                    if (TextboxPositionMode == TextboxPositionMode.DYNAMIC_BOTTOM_PREFERRED)
                    {
                        return !map.Camera.IsAtBottomOfMap() ? bottom : top;
                    }
                    else
                    {
                        return !map.Camera.IsAtTopOfMap() ? top : bottom;
                    }
                }
            }
        }

        public void Draw(GraphicsHandler graphicsHandler)
        {
            // draw textbox
            // if camera is at bottom of screen, textbox is drawn at top of screen instead of the bottom like usual
            // to prevent it from covering the player
            int y = GetValueBasedOnPositionMode(BottomY, TopY);
            graphicsHandler.DrawFilledRectangleWithBorder(new Rectangle(X, y, Width, Height), FillColor, BorderColor, BorderThickness);

            if (text != null)
            {
                // draw text in textbox
                text.DrawWithParsedNewLines(graphicsHandler, SpaceBetweenLines);

                if (currentTextItem.HasOptions())
                {
                    OptionsBox.Draw(graphicsHandler);
                }
            }
        }
    }
}
