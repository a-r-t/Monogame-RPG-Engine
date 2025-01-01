using Engine.Core;
using Engine.FontGraphics;
using Engine.Scene.MapCore;
using Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App.Resources.FontsHelper;

namespace Engine.Scene.TextboxCore
{
    public class OptionsBox
    {
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

        public Color FillColor { get; set; } = Color.White;
        public Color BorderColor { get; set; } = Color.Black;
        public int BorderThickness { get; set; } = 2;
        public Color TextColor { get; set; } = Color.Black;

        public int SelectorWidth { get; set; } = 10;
        public int SelectorHeight { get; set; } = 10;
        public Color SelectorColor { get; set; } = Color.Black;

        public int SelectedOptionIndex { get; set; } = 0;

        private KeyLocker keyLocker = new KeyLocker();

        private List<DynamicSpriteFontGraphic> options = null;

        public Keys MoveSelectionUpwardKey { get; set; } = Keys.Up;
        public Keys MoveSelectionDownwardKey { get; set; } = Keys.Down;

        public TrueTypeFont TextboxFontGraphic { get; set; }
        public int TextboxFontSize { get; set; } = 30;

        public TextboxPositionMode TextboxPositionMode { get; set; }

        private Map map;

        public OptionsBox(ContentLoader contentLoader)
        {
            TextboxFontGraphic = contentLoader.LoadTrueTypeFont(TrueTypeFonts.ARIAL);
        }

        public void SetMap(Map map)
        {
            this.map = map;
        }

        public void Initialize(TextboxItem currentTextItem)
        {
            // if camera is at bottom of screen, text is drawn at top of screen instead of the bottom like usual
            // to prevent it from covering the player
            int fontOptionY = GetValueBasedOnPositionMode(FontOptionBottomYStart, FontOptionTopYStart);

            options = new List<DynamicSpriteFontGraphic>();
            // for each option, crate option text spritefont that will be drawn in options textbox
            for (int i = 0; i < currentTextItem.Options.Count; i++)
            {
                options.Add(new DynamicSpriteFontGraphic(currentTextItem.Options[i], TextboxFontGraphic, TextboxFontSize, new Vector2(FontOptionX, fontOptionY + (i * FontOptionSpacing)), TextColor));
            }
            SelectedOptionIndex = 0;
            keyLocker.LockKey(MoveSelectionUpwardKey);
            keyLocker.LockKey(MoveSelectionDownwardKey);
        }

        public void Update(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(MoveSelectionDownwardKey) && !keyLocker.IsKeyLocked(MoveSelectionDownwardKey))
            {
                keyLocker.LockKey(MoveSelectionDownwardKey);
                if (SelectedOptionIndex < options.Count - 1)
                {
                    SelectedOptionIndex++;
                }
            }
            if (keyboardState.IsKeyDown(MoveSelectionUpwardKey) && !keyLocker.IsKeyLocked(MoveSelectionUpwardKey))
            {
                keyLocker.LockKey(MoveSelectionUpwardKey);
                if (SelectedOptionIndex > 0)
                {
                    SelectedOptionIndex--;
                }
            }
            if (keyboardState.IsKeyUp(MoveSelectionDownwardKey))
            {
                keyLocker.UnlockKey(MoveSelectionDownwardKey);
            }
            if (keyboardState.IsKeyUp(MoveSelectionUpwardKey))
            {
                keyLocker.UnlockKey(MoveSelectionUpwardKey);
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

        public virtual void Draw(GraphicsHandler graphicsHandler)
        {
            // draw options textbox
            // if camera is at bottom of screen, textbox is drawn at top of screen instead of the bottom like usual
            // to prevent it from covering the player
            int optionY = GetValueBasedOnPositionMode(OptionBottomY, OptionTopY);
            graphicsHandler.DrawFilledRectangleWithBorder(new Rectangle(OptionX, optionY, OptionWidth, OptionHeight), FillColor, BorderColor, BorderThickness);

            // draw each option text
            foreach (DynamicSpriteFontGraphic option in options)
            {
                option.Draw(graphicsHandler);
            }

            DrawSelector(graphicsHandler);
        }

        public virtual void DrawSelector(GraphicsHandler graphicsHandler)
        {
            // the start y location of the option pointer depends on whether the options textbox is on top or bottom of screen
            int optionPointerYStart = GetValueBasedOnPositionMode(OptionPointerYBottomStart, OptionPointerYTopStart);
            // draw option selection indicator (small black rectangle)
            graphicsHandler.DrawFilledRectangle(OptionPointerX, optionPointerYStart + (SelectedOptionIndex * FontOptionSpacing), SelectorWidth, SelectorHeight, SelectorColor);
        }
    }
}
