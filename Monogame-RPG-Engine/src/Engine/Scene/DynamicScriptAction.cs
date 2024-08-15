using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Scene;

namespace Engine.Scene
{
    public class DynamicScriptAction : ScriptAction
    {
        protected Action setupAction;
        protected Func<ScriptState> executeFunc;
        protected Action cleanupAction;

        public DynamicScriptAction()
        {

        }

        public DynamicScriptAction(Func<ScriptState> executeFunc)
        {
            this.executeFunc = executeFunc;
        }

        public DynamicScriptAction(Action setupAction, Func<ScriptState> executeFunc)
        {
            this.setupAction = setupAction;
            this.executeFunc = executeFunc;
        }

        public DynamicScriptAction(Func<ScriptState> executeFunc, Action cleanupAction)
        {
            this.executeFunc = executeFunc;
            this.cleanupAction = cleanupAction;
        }

        public DynamicScriptAction(Action setupAction, Func<ScriptState> executeFunc, Action cleanupAction)
        {
            this.setupAction = setupAction;
            this.executeFunc = executeFunc;
            this.cleanupAction = cleanupAction;
        }

        public override void Setup() 
        {
            if (setupAction != null)
            {
                setupAction.Invoke();
            }
        }

        public override ScriptState Execute()
        {
            if (executeFunc != null)
            {
                return executeFunc.Invoke();
            }
            return ScriptState.COMPLETED;
        }

        public override void Cleanup() 
        {
            if (cleanupAction != null)
            {
                cleanupAction.Invoke();
            }
        }
    }
}
