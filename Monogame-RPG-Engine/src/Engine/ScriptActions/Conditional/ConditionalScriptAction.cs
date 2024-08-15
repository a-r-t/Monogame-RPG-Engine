using Engine.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ScriptActions.Conditional
{
    public class ConditionalScriptAction : ScriptAction
    {
        public List<ConditionalScriptActionGroup> ConditionalScriptActionGroups { get; private set; }
        protected int currentScriptActionGroupIndex;
        protected int currentScriptActionIndex;

        public ConditionalScriptAction()
        {
            ConditionalScriptActionGroups = new List<ConditionalScriptActionGroup>();
        }

        public ConditionalScriptAction(List<ConditionalScriptActionGroup> conditionalScriptActionGroups)
        {
            ConditionalScriptActionGroups = conditionalScriptActionGroups;
        }

        public ConditionalScriptAction AddConditionalScriptActionGroup(ConditionalScriptActionGroup conditionalScriptActionGroup)
        {
            ConditionalScriptActionGroups.Add(conditionalScriptActionGroup);
            return this;
        }

        public override void Setup()
        {
            bool groupRequirementMet = false;
            for (int i = 0; i < ConditionalScriptActionGroups.Count; i++)
            {
                ConditionalScriptActionGroup conditionalScriptActionGroup = ConditionalScriptActionGroups[i];

                if (AreRequirementsMet(conditionalScriptActionGroup))
                {
                    currentScriptActionGroupIndex = i;
                    currentScriptActionIndex = 0;
                    ConditionalScriptActionGroups[currentScriptActionGroupIndex].ScriptActions[currentScriptActionIndex].Setup();
                    groupRequirementMet = true;
                    break;
                }
            }
            if (!groupRequirementMet)
            {
                // this prevents a crash from occurring if no group requirements have been met
                // it just adds a fake group with a fake script action that does nothing
                // while there are other ways of fixing this, the other ways result in the script execution code being less efficient, which is not ideal for a game that needs to run as fast as possible
                ConditionalScriptActionGroups.Add(doNothingActionGroup);
                currentScriptActionGroupIndex = ConditionalScriptActionGroups.Count - 1;
                currentScriptActionIndex = 0;
            }
        }

        private static ConditionalScriptActionGroup doNothingActionGroup = new ConditionalScriptActionGroup()
            .AddScriptAction(new DoNothingScriptAction());

        protected bool AreRequirementsMet(ConditionalScriptActionGroup conditionalScriptActionGroup)
        {
            List<bool> metRequirementStatuses = new List<bool>();
            foreach (Requirement requirement in conditionalScriptActionGroup.Requirements)
            {
                bool requirementStatus = false;
                if (requirement is FlagRequirement)
                {
                    requirementStatus = IsFlagRequirementMet((FlagRequirement)requirement);
                }
                else if (requirement is CustomRequirement)
                {
                    requirementStatus = ((CustomRequirement)requirement).IsRequirementMet();
                }
                if (!requirementStatus && conditionalScriptActionGroup.FlagStrategy == FlagStrategy.AND)
                {
                    return false;
                }
                else if (requirementStatus && conditionalScriptActionGroup.FlagStrategy == FlagStrategy.OR)
                {
                    return true;
                }
                else
                {
                    metRequirementStatuses.Add(requirementStatus);
                }
            }

            // if strategy is AND, all requirements had to have been met up to this point to avoid the short circuit, so we know its true
            if (conditionalScriptActionGroup.FlagStrategy == FlagStrategy.AND)
            {
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
            string flagName = flagRequirement.FlagName;
            bool currentFlagStatus = this.map.FlagManager.IsFlagSet(flagName);
            return flagRequirement.FlagValue == currentFlagStatus;
        }

        public override ScriptState Execute()
        {
            // Runs an execute cycle of the Script
            List<ScriptAction> scriptActions = ConditionalScriptActionGroups[currentScriptActionGroupIndex].ScriptActions;
            ScriptAction currentScriptAction = scriptActions[currentScriptActionIndex];
            ScriptState scriptState = currentScriptAction.Execute();
            if (scriptState == ScriptState.COMPLETED)
            {
                currentScriptAction.Cleanup();
                currentScriptActionIndex++;

                if (currentScriptActionIndex < scriptActions.Count)
                {
                    scriptActions[currentScriptActionIndex].Setup();
                    return ScriptState.RUNNING;
                }
                else
                {
                    return ScriptState.COMPLETED;
                }
            }
            return ScriptState.RUNNING;
        }
    }
}
