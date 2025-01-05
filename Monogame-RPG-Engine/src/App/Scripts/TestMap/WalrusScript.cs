using Monogame_RPG_Engine.Engine.Scene;
using Monogame_RPG_Engine.Engine.Scene.ScriptCore;
using Monogame_RPG_Engine.Engine.Scene.ScriptCore.ScriptActions;
using Monogame_RPG_Engine.Engine.Scene.ScriptCore.ScriptActions.Conditional;
using Monogame_RPG_Engine.Engine.Scene.ScriptCore.ScriptActions.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_RPG_Engine.App.Scripts.TestMap
{
    public class WalrusScript : Script
    {
        public override List<ScriptAction> LoadScriptActions()
        {
            List<ScriptAction> scriptActions = new List<ScriptAction>();
            scriptActions.Add(new LockPlayerScriptAction());

            scriptActions.Add(new NPCFacePlayerScriptAction());

            scriptActions.Add(new ConditionalScriptAction()

                .AddConditionalScriptActionGroup(new ConditionalScriptActionGroup()
                    .AddRequirement(new FlagRequirement("hasTalkedToWalrus", false))
                    .AddScriptAction(new TextboxScriptAction()
                        .AddText("Hi Cat!")
                        .AddText("...oh, you lost your ball?")
                        .AddText("Hmmm...my walrus brain remembers seeing Dino with\nit last. Maybe you can check with him?")
                    )
                    .AddScriptAction(new ChangeFlagScriptAction("hasTalkedToWalrus", true))
                )

                .AddConditionalScriptActionGroup(new ConditionalScriptActionGroup()
                    .AddRequirement(new FlagRequirement("hasTalkedToWalrus", true))
                    .AddScriptAction(new TextboxScriptAction()
                        .AddText("I sure love doing walrus things!")
                    )
                )
            );

            scriptActions.Add(new UnlockPlayerScriptAction());
            return scriptActions;
        }
    }
}
