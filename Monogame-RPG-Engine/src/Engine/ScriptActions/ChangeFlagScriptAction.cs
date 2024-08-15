using Engine.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ScriptActions
{
    public class ChangeFlagScriptAction : ScriptAction
    {
        protected string flagName;
        protected bool flagState;

        public ChangeFlagScriptAction(string flagName, bool flagState)
        {
            this.flagName = flagName;
            this.flagState = flagState;
        }

        public override ScriptState Execute()
        {
            if (flagState)
            {
                this.map.FlagManager.SetFlag(flagName);
            }
            else
            {
                this.map.FlagManager.UnsetFlag(flagName);
            }
            return ScriptState.COMPLETED;
        }
    }
}
