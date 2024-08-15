using Engine.Scene;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ScriptActions
{
    public class NPCStandScriptAction : ScriptAction
    {
        protected NPC npc;
        protected Direction facingDirection;

        public NPCStandScriptAction(Direction facingDirection)
        {
            this.facingDirection = facingDirection;
        }

        public NPCStandScriptAction(int npcId, Direction facingDirection)
        {
            this.npc = map.GetNPCById(npcId);
            this.facingDirection = facingDirection;
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
            npc.Stand(facingDirection);
            return ScriptState.COMPLETED;
        }
    }
}
