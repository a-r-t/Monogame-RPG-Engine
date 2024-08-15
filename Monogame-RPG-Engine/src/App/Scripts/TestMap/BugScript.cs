using Engine.Scene;
using Engine.ScriptActions;
using Engine.ScriptActions.Conditional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Scripts
{
    public class BugScript : Script
    {
        public override List<ScriptAction> LoadScriptActions()
        {
            List<ScriptAction> scriptActions = new List<ScriptAction>();
            scriptActions.Add(new LockPlayerScriptAction());

            scriptActions.Add(new NPCLockScriptAction());

            scriptActions.Add(new NPCFacePlayerScriptAction());

            scriptActions.Add(new TextboxScriptAction()
                .AddText("Hello!")
                .AddText("Do you like bugs?", new string[] { "Yes", "No" })
            );


            scriptActions.Add(new ConditionalScriptAction()

                .AddConditionalScriptActionGroup(new ConditionalScriptActionGroup()
                    .AddRequirement(new CustomRequirement(() =>
                    {
                        int answer = this.ScriptActionOutputManager.GetFlagData<int>("TEXTBOX_OPTION_SELECTION");
                        return answer == 0;
                    }))
                    .AddScriptAction(new TextboxScriptAction()
                        .AddText("I knew you were a cool cat!")
                        .AddText("I'm going to let you in on a little secret...\nYou can push some rocks out of the way.")
                    )
                )

                .AddConditionalScriptActionGroup(new ConditionalScriptActionGroup()
                    .AddRequirement(new CustomRequirement(() =>
                    {
                        int answer = this.ScriptActionOutputManager.GetFlagData<int>("TEXTBOX_OPTION_SELECTION");
                        return answer == 1;
                    }))
                    .AddScriptAction(new TextboxScriptAction()
                        .AddText("Oh...uh...awkward...")
                    )
                )
            );

            scriptActions.Add(new NPCUnlockScriptAction());
            scriptActions.Add(new UnlockPlayerScriptAction());
            return scriptActions;
        }
    }
}
