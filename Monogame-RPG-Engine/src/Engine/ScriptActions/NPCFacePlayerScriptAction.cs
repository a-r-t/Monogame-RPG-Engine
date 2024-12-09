using Engine.Scene;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ScriptActions
{
    public class NPCFacePlayerScriptAction : ScriptAction
    {
        protected int? npcId = null;
        protected NPC npc;

        public NPCFacePlayerScriptAction()
        {
        }

        public NPCFacePlayerScriptAction(int npcId)
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
            npc.FacePlayer(this.player);
            return ScriptState.COMPLETED;
        }
    }
}
