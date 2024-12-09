using Engine.Scene;
using Engine.ScriptActions.Utils;
using System.Collections.Generic;

namespace Engine.ScriptActions.Loop
{
    public class LoopFixedScriptAction : ScriptAction
    {
        public List<ScriptAction> ScriptActions { get; }
        public List<Requirement> Requirements { get; }
        public FlagStrategy FlagStrategy { get; set; }
        protected int currentScriptActionIndex;
        protected int previousScriptActionIndex;
        protected int numberOfIterations;
        protected int currentIteration;

        public LoopFixedScriptAction(int numberOfIterations)
        {
            ScriptActions = new List<ScriptAction>();
            this.numberOfIterations = numberOfIterations;
        }

        public LoopFixedScriptAction AddScriptAction(ScriptAction scriptAction)
        {
            ScriptActions.Add(scriptAction);
            return this;
        }

        public override void Setup()
        {
            currentIteration = 0;
            currentScriptActionIndex = 0;
            previousScriptActionIndex = -1;
        }

        public override ScriptState Execute()
        {
            if (previousScriptActionIndex != currentScriptActionIndex)
            {
                previousScriptActionIndex = currentScriptActionIndex;

                // handle iterations
                if (currentScriptActionIndex == 0 && currentIteration >= numberOfIterations)
                {
                    return ScriptState.COMPLETED;
                }

                if (currentScriptActionIndex == 0)
                {
                    currentIteration++;
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
