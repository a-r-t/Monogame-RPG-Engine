using Engine.Builders;
using Engine.Entity;
using Engine.Scene;
using Engine.ScriptActions;
using Engine.ScriptActions.Conditional;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Scripts
{
    public class DinoScript : Script
    {
        public override List<ScriptAction> LoadScriptActions()
        {
            List<ScriptAction> scriptActions = new List<ScriptAction>();
            scriptActions.Add(new LockPlayerScriptAction());
            scriptActions.Add(new TextboxScriptAction("Isn't my garden so lovely?"));

            scriptActions.Add(new ConditionalScriptAction()

                .AddConditionalScriptActionGroup(new ConditionalScriptActionGroup()
                    .AddRequirement(new FlagRequirement("hasTalkedToWalrus", true))
                    .AddRequirement(new FlagRequirement("hasTalkedToDinosaur", false))
                    
                    .AddScriptAction(new WaitScriptAction(70))
                    .AddScriptAction(new NPCFacePlayerScriptAction())
                    .AddScriptAction(new TextboxScriptAction()
                        .AddText("Oh, you're still here...")
                        .AddText("...You heard from Walrus that he saw me with your\nball?")
                        .AddText("Well, I saw him playing with it and was worried it would\nroll into my garden.")
                        .AddText("So I kicked it as far as I could into the forest to the left.")
                        .AddText("Now, if you'll excuse me, I have to go.")
                    )

                    .AddScriptAction(new NPCStandScriptAction(Direction.RIGHT))
                    .AddScriptAction(new NPCWalkScriptAction(Direction.DOWN, 36, 2))
                    .AddScriptAction(new NPCWalkScriptAction(Direction.RIGHT, 196, 2))

                    .AddScriptAction(new DynamicScriptAction(() =>
                    {
                        // change door to the open door map tile
                        Frame openDoorFrame = new FrameBuilder(map.Tileset.GetSubImage(4, 4), 0)
                            .WithScale(map.Tileset.TileScale)
                            .Build();

                        Point location = map.GetMapTile(17, 4).Location;

                        MapTile mapTile = new MapTileBuilder(openDoorFrame)
                            .WithTileType(TileType.NOT_PASSABLE)
                            .Build(location.X, location.Y);

                        map.SetMapTile(17, 4, mapTile);
                        return ScriptState.COMPLETED;
                    }))

                    .AddScriptAction(new NPCWalkScriptAction(Direction.UP, 50, 2))
                    .AddScriptAction(new NPCChangeVisibilityScriptAction(Visibility.HIDDEN))

                    .AddScriptAction(new DynamicScriptAction(() =>
                    {
                        // change door back to the closed door map tile
                        Frame doorFrame = new FrameBuilder(map.Tileset.GetSubImage(4, 3), 0)
                            .WithScale(map.Tileset.TileScale)
                            .Build();

                        Point location = map.GetMapTile(17, 4).Location;

                        MapTile mapTile = new MapTileBuilder(doorFrame)
                            .WithTileType(TileType.NOT_PASSABLE)
                            .Build(location.X, location.Y);

                        map.SetMapTile(17, 4, mapTile);
                        return ScriptState.COMPLETED;
                    }))

                    .AddScriptAction(new ChangeFlagScriptAction("hasTalkedToDinosaur", true))
                )
            );

            scriptActions.Add(new UnlockPlayerScriptAction());
            return scriptActions;
        }
    }
}
