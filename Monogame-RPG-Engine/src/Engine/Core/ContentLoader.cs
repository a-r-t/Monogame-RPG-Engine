using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Monogame_RPG_Engine.Engine.FontGraphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Monogame_RPG_Engine.Engine.Core
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

        public TrueTypeFont LoadTrueTypeFont(string trueTypeFontPath)
        {
            if (trueTypeFonts.ContainsKey("trueTypeFontPath"))
            {
                return new TrueTypeFont(trueTypeFonts[trueTypeFontPath]);
            }
            return new TrueTypeFont(File.ReadAllBytes(trueTypeFontPath));
        }

        public SoundEffect LoadSoundEffect(string soundEffectPath)
        {
            return Load<SoundEffect>(soundEffectPath);
        }

        public Song LoadSong(string songPath)
        {
            return Load<Song>(songPath);
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
