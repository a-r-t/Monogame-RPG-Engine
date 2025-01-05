using Monogame_RPG_Engine.Engine.Scene;
using Monogame_RPG_Engine.Engine.Scene.ScriptCore;
using Monogame_RPG_Engine.Engine.Scene.ScriptCore.ScriptActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_RPG_Engine.App.Scripts.TestMap
{
    public class LostBallScript : Script
    {
        public override List<ScriptAction> LoadScriptActions()
        {
            List<ScriptAction> scriptActions = new List<ScriptAction>();
            scriptActions.Add(new LockPlayerScriptAction());
            scriptActions.Add(new TextboxScriptAction()
                .AddText("Where did my ball go!?")
                .AddText("I left it right here before I took my 22 hour cat nap.")
                .AddText("Maybe Walrus has seen it.")
            );

            scriptActions.Add(new ChangeFlagScriptAction("hasLostBall", true));
            scriptActions.Add(new UnlockPlayerScriptAction());
            return scriptActions;
        }
    }
}
