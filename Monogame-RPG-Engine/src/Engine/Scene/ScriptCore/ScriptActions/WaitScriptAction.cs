using Monogame_RPG_Engine.Engine.Scene;
using Monogame_RPG_Engine.Engine.Scene.ScriptCore;
using Monogame_RPG_Engine.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_RPG_Engine.Engine.Scene.ScriptCore.ScriptActions
{
    public class WaitScriptAction : ScriptAction
    {
        protected int framesToWait;
        protected int counter;

        public WaitScriptAction(int framesToWait)
        {
            this.framesToWait = framesToWait;
        }

        public override void Setup()
        {
            this.counter = framesToWait;
        }

        public override ScriptState Execute()
        {
            counter--;
            if (counter > 0)
            {
                return ScriptState.RUNNING;
            }
            return ScriptState.COMPLETED;
        }
    }
}
