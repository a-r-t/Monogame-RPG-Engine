using Monogame_RPG_Engine.Engine.Scene;
using Monogame_RPG_Engine.Engine.Scene.EntitiesCore;
using Monogame_RPG_Engine.Engine.Scene.ScriptCore;
using Monogame_RPG_Engine.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_RPG_Engine.Engine.Scene.ScriptCore.ScriptActions
{
    public class NPCStandScriptAction : ScriptAction
    {
        protected int? npcId = null;
        protected NPC npc;
        protected Direction facingDirection;

        public NPCStandScriptAction(Direction facingDirection)
        {
            this.facingDirection = facingDirection;
        }

        public NPCStandScriptAction(int npcId, Direction facingDirection)
        {
            this.npcId = npcId;
            this.facingDirection = facingDirection;
        }

        public override void Setup()
        {
            if (!this.npcId.HasValue)
            {
                if (this.entity != null)
                {
                    this.npc = (NPC)entity;
                }
                else
                {
                    throw new Exception("No NPC entity specified!");
                }
            }
            else
            {
                this.npc = map.GetNPCById(npcId.Value);
                if (this.npc == null)
                {
                    throw new Exception("NPC with id " + npcId + " not found!");
                }
            }
        }

        public override ScriptState Execute()
        {
            npc.Stand(facingDirection);
            return ScriptState.COMPLETED;
        }
    }
}
