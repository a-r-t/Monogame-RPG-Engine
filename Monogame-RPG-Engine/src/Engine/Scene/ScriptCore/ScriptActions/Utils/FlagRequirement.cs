using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_RPG_Engine.Engine.Scene.ScriptCore.ScriptActions.Utils
{
    public class FlagRequirement : Requirement
    {
        public string FlagName { get; private set; }
        public bool FlagValue { get; private set; }

        public FlagRequirement(string flagName, bool flagValue)
        {
            FlagName = flagName;
            FlagValue = flagValue;
        }
    }
}
