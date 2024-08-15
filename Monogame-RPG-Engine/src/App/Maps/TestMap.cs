using App.EnhancedMapTiles;
using App.NPCs;
using App.Scripts;
using App.Tilesets;
using Engine.Core;
using Engine.Entity;
using Engine.Scene;
using Engine.Utils;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Maps
{
    public class TestMap : Map
    {

        public TestMap(ContentLoader contentLoader)
            : base("test_map.txt", new CommonTileset(contentLoader), contentLoader)
        {
            PlayerStartPosition = GetMapTile(17, 20).Location;
        }

        protected override List<EnhancedMapTile> LoadEnhancedMapTiles()
        {
            List<EnhancedMapTile> enhancedMapTiles = new List<EnhancedMapTile>();

            PushableRock pushableRock = new PushableRock(GetMapTile(2, 7).Location, ContentLoader);
            enhancedMapTiles.Add(pushableRock);

            return enhancedMapTiles;
        }

        protected override List<NPC> LoadNPCs()
        {
            List<NPC> npcs = new List<NPC>();

            Walrus walrus = new Walrus(1, GetMapTile(4, 28).Location.SubtractY(40), ContentLoader);
            walrus.InteractScript = new WalrusScript();
            npcs.Add(walrus);

            Dinosaur dinosaur = new Dinosaur(2, GetMapTile(13, 4).Location, ContentLoader);
            dinosaur.ExistenceFlag = "hasTalkedToDinosaur";
            dinosaur.InteractScript = new DinoScript();
            npcs.Add(dinosaur);

            Bug bug = new Bug(3, GetMapTile(7, 12).Location.SubtractX(20), ContentLoader);
            bug.InteractScript = new BugScript();
            npcs.Add(bug);

            return npcs;
        }

        protected override List<Trigger> LoadTriggers()
        {
            List<Trigger> triggers = new List<Trigger>();
            triggers.Add(new Trigger(790, 1030, 100, 10, new LostBallScript(), "hasLostBall"));
            triggers.Add(new Trigger(790, 960, 10, 80, new LostBallScript(), "hasLostBall"));
            triggers.Add(new Trigger(890, 960, 10, 80, new LostBallScript(), "hasLostBall"));
            return triggers;
        }

        protected override void LoadScripts()
        {
            GetMapTile(21, 19).InteractScript = new SimpleTextScript("Cat's house");

            GetMapTile(7, 26).InteractScript = new SimpleTextScript("Walrus's house");

            GetMapTile(20, 4).InteractScript = new SimpleTextScript("Dino's house");

            GetMapTile(2, 6).InteractScript = new TreeScript();
        }
    }
}
