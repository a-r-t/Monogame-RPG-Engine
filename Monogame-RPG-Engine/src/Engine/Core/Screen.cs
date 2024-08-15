using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Core
{
    public abstract class Screen
    {
        // each individual screen has access to its own content loader
        public ContentLoader ContentLoader { get; private set; }

        // all screens share this global content loader for content that is designed to be used everywhere
        public static ContentLoader GlobalContentLoader;

        public Screen()
        {
            ContentLoader = ContentLoader.Create();
        }

        static Screen()
        {
            GlobalContentLoader = ContentLoader.Create();
        }

        public virtual void Initialize() { }
        public virtual void LoadContent() { }
        public virtual void UnloadContent()
        {
            ContentLoader.Unload();
        }
        public virtual void Update(GameTime gameTime, KeyboardState keyboardState) { }
        public virtual void Draw(GraphicsHandler graphicsHandler) { }

        // warning: if you need to call this, you likely have an asset loaded globally that shouldn't be
        public static void UnloadGlobalContent()
        {
            GlobalContentLoader.Unload();
        }
    }
}
