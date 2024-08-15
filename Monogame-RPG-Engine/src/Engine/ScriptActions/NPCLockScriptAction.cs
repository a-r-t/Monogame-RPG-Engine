using Engine.Scene;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ScriptActions
{
    public class NPCLockScriptAction : ScriptAction
    {
        protected NPC npc;

        public NPCLockScriptAction()
        {
        }

        public NPCLockScriptAction(int npcId)
        {
            this.npc = map.GetNPCById(npcId);
        }

        public override void Setup()
        {
            if (this.npc == null)
            {
                this.npc = (NPC)entity;
            }
        }

        public override ScriptState Execute()
        {
            npc.Lock();
            return ScriptState.COMPLETED;
        }
    }
}
