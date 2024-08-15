using Engine.Scene;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ScriptActions
{
    public class NPCUnlockScriptAction : ScriptAction
    {
        protected NPC npc;

        public NPCUnlockScriptAction()
        {
        }

        public NPCUnlockScriptAction(int npcId)
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
            npc.Unlock();
            return ScriptState.COMPLETED;
        }
    }
}
