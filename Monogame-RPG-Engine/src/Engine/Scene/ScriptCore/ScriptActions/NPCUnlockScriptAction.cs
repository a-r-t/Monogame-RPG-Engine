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
    public class NPCUnlockScriptAction : ScriptAction
    {
        protected int? npcId = null;
        protected NPC npc;

        public NPCUnlockScriptAction()
        {
        }

        public NPCUnlockScriptAction(int npcId)
        {
            this.npcId = npcId;
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
            npc.Unlock();
            return ScriptState.COMPLETED;
        }
    }
}
