using Engine.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ScriptActions
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
