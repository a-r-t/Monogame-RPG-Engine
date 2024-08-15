using Engine.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ScriptActions.Conditional
{
    public class ConditionalScriptActionGroup
    {
        public List<ScriptAction> ScriptActions { get; private set; }
        public List<Requirement> Requirements { get; private set; }
        public FlagStrategy FlagStrategy { get; set; }

        public ConditionalScriptActionGroup()
        {
            ScriptActions = new List<ScriptAction>();
            Requirements = new List<Requirement>();
            FlagStrategy = FlagStrategy.AND;
        }

        public ConditionalScriptActionGroup(FlagStrategy flagStrategy)
        {
            ScriptActions = new List<ScriptAction>();
            Requirements = new List<Requirement>();
            FlagStrategy = flagStrategy;
        }

        public ConditionalScriptActionGroup AddScriptAction(ScriptAction scriptAction)
        {
            ScriptActions.Add(scriptAction);
            return this;
        }

        public ConditionalScriptActionGroup AddRequirement(Requirement requirement)
        {
            Requirements.Add(requirement);
            return this;
        }
    }
}
