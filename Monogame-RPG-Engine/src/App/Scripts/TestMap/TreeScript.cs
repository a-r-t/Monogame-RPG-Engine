using Engine.Scene;
using Engine.ScriptActions;
using Engine.ScriptActions.Conditional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Scripts
{
    public class TreeScript : Script
    {
        public override List<ScriptAction> LoadScriptActions()
        {
            List<ScriptAction> scriptActions = new List<ScriptAction>();
            scriptActions.Add(new LockPlayerScriptAction());

            scriptActions.Add(new ConditionalScriptAction()

                .AddConditionalScriptActionGroup(new ConditionalScriptActionGroup()
                    .AddRequirement(new FlagRequirement("hasTalkedToDinosaur", true))
                    .AddRequirement(new FlagRequirement("hasFoundBall", false))
                    .AddRequirement(new CustomRequirement(() =>
                    {
                        // ensures player is directly underneath tree trunk tile
                        // this prevents the script from working if the player tries to interact with it from the side

                        // if player is not below tree trunk tile, player location is not valid and this conditional script will not be reached
                        if (player.Bounds.Y1 <= entity.Bounds.Y2)
                        {
                            return false;
                        }

                        // if code gets here, it means player is below tree trunk tile and player location is valid, so this conditional script will continue
                        return true;
                    }))
                    .AddScriptAction(new TextboxScriptAction()
                        .AddText("...")
                        .AddText("I found my ball inside of the tree!\nYippee!")
                    )
                    .AddScriptAction(new ChangeFlagScriptAction("hasFoundBall", true))
                )
            );

            scriptActions.Add(new UnlockPlayerScriptAction());
            return scriptActions;
        }
    }
}
