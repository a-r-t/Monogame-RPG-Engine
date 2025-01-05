using Monogame_RPG_Engine.Engine.Scene;
using Monogame_RPG_Engine.Engine.Scene.ScriptCore;
using Monogame_RPG_Engine.Engine.Scene.ScriptCore.ScriptActions.Utils;
using System.Collections.Generic;

namespace Monogame_RPG_Engine.Engine.Scene.ScriptCore.ScriptActions.Loop
{
    public class LoopIndefiniteScriptAction : ScriptAction
    {
        public List<ScriptAction> ScriptActions { get; }
        public List<Requirement> Requirements { get; }
        public FlagStrategy FlagStrategy { get; set; }
        protected int currentScriptActionIndex;
        protected int previousScriptActionIndex;

        public LoopIndefiniteScriptAction()
        {
            ScriptActions = new List<ScriptAction>();
            Requirements = new List<Requirement>();
            FlagStrategy = FlagStrategy.AND;
        }

        public LoopIndefiniteScriptAction(FlagStrategy flagStrategy)
        {
            ScriptActions = new List<ScriptAction>();
            Requirements = new List<Requirement>();
            FlagStrategy = flagStrategy;
        }

        public LoopIndefiniteScriptAction AddScriptAction(ScriptAction scriptAction)
        {
            ScriptActions.Add(scriptAction);
            return this;
        }

        public LoopIndefiniteScriptAction AddRequirement(Requirement requirement)
        {
            Requirements.Add(requirement);
            return this;
        }

        public override void Setup()
        {
            currentScriptActionIndex = 0;
            previousScriptActionIndex = -1;
        }

        protected bool AreRequirementsMet()
        {
            List<bool> metRequirementStatuses = new List<bool>();
            foreach (Requirement requirement in Requirements)
            {
                bool requirementStatus = false;
                if (requirement is FlagRequirement) {
                    requirementStatus = IsFlagRequirementMet((FlagRequirement)requirement);
                }
                else if (requirement is CustomRequirement) {
                    requirementStatus = ((CustomRequirement)requirement).IsRequirementMet();
                }
                if (!requirementStatus && FlagStrategy == FlagStrategy.AND)
                {
                    return false;
                }
                else if (requirementStatus && FlagStrategy == FlagStrategy.OR)
                {
                    return true;
                }
                else
                {
                    metRequirementStatuses.Add(requirementStatus);
                }
            }
            // if strategy is AND, all requirements had to have been met up to this point to avoid the short circuit, so we know its true
            if (FlagStrategy == FlagStrategy.AND) {
                return true;
            }
            // if strategy is OR, no requirements had to have been met up to this point to avoid the short circuit, so we know its false
            else
            {
                return false;
            }
        }

        protected bool IsFlagRequirementMet(FlagRequirement flagRequirement)
        {
            bool currentFlagStatus = this.map.FlagManager.IsFlagSet(flagRequirement.FlagName);
            return flagRequirement.FlagValue == currentFlagStatus;
        }

        public override ScriptState Execute()
        {
            if (previousScriptActionIndex != currentScriptActionIndex)
            {
                previousScriptActionIndex = currentScriptActionIndex;

                // handle determining whether loop should stop or not
                if (currentScriptActionIndex == 0 && AreRequirementsMet())
                {
                    return ScriptState.COMPLETED;
                }

                ScriptActions[currentScriptActionIndex].Setup();
            }

            ScriptAction currentScriptAction = ScriptActions[currentScriptActionIndex];
            ScriptState scriptState = currentScriptAction.Execute();

            if (scriptState == ScriptState.COMPLETED)
            {
                currentScriptAction.Cleanup();
                currentScriptActionIndex++;

                if (currentScriptActionIndex >= ScriptActions.Count)
                {
                    currentScriptActionIndex = 0;
                    previousScriptActionIndex = -1;
                }
            }

            return ScriptState.RUNNING;
        }
    }
}
