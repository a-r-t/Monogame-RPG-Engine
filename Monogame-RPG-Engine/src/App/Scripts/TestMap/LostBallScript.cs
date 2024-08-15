using Engine.Scene;
using Engine.ScriptActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Scripts
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
