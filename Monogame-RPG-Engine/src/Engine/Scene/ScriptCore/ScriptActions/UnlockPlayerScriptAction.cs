using Monogame_RPG_Engine.Engine.Scene;
using Monogame_RPG_Engine.Engine.Scene.ScriptCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_RPG_Engine.Engine.Scene.ScriptCore.ScriptActions
{
    public class UnlockPlayerScriptAction : ScriptAction
    {
        public override ScriptState Execute()
        {
            this.player.Unlock();
            return ScriptState.COMPLETED;
        }
    }
}
