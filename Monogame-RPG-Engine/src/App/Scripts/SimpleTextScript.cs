using Monogame_RPG_Engine.Engine.Scene;
using Monogame_RPG_Engine.Engine.Scene.ScriptCore;
using Monogame_RPG_Engine.Engine.Scene.ScriptCore.ScriptActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_RPG_Engine.App.Scripts
{
    public class SimpleTextScript : Script
    {
        private String[] textItems;

        public SimpleTextScript(string text)
        {
            this.textItems = new string[] { text };
        }

        public SimpleTextScript(string[] text)
        {
            this.textItems = text;
        }


        public override List<ScriptAction> LoadScriptActions()
        {
            List<ScriptAction> scriptActions = new List<ScriptAction>();
            scriptActions.Add(new LockPlayerScriptAction());
            scriptActions.Add(new TextboxScriptAction(textItems));
            scriptActions.Add(new UnlockPlayerScriptAction());
            return scriptActions;
        }
    }
}
