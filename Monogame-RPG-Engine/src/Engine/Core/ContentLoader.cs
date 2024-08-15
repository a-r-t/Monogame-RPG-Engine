using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Core
{
    public class ContentLoader : ContentManager
    {
        private ContentLoader(IServiceProvider serviceProvider, string rootDirectory)
            : base(serviceProvider, rootDirectory)
        {

        }

        public Texture2D LoadTexture(string texturePath)
        {
            return Load<Texture2D>(texturePath);
        }

        public SpriteFont LoadSpriteFont(string spriteFontPath)
        {
            return Load<SpriteFont>(spriteFontPath);
        }

        // used for fontstashsharp's dynamic sprite fonts
        // have to manually handle loading/unloading/cacheing since these are not compatible with the content pipeline
        private Dictionary<string, byte[]> trueTypeFonts = new Dictionary<string, byte[]>();

        public byte[] LoadTrueTypeFont(string trueTypeFontPath)
        {
            if (trueTypeFonts.ContainsKey("trueTypeFontPath"))
            {
                return trueTypeFonts[trueTypeFontPath];
            }
            return File.ReadAllBytes(trueTypeFontPath);
        }

        public static ContentLoader Create()
        {
            return new ContentLoader(GameLoop.GameServiceContainer, GameLoop.ContentManager.RootDirectory);
        }

        public static ContentLoader Create(IServiceProvider serviceProvider, string rootDirectory)
        {
            return new ContentLoader(serviceProvider, rootDirectory);
        }

        public override void Unload()
        {
            base.Unload();
            trueTypeFonts.Clear();
        }
    }
}
