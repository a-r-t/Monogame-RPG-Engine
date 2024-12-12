using Engine.Scene;
using Engine.Scene.EntitiesCore;
using Engine.Scene.ScriptCore;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Scene.ScriptCore.ScriptActions
{
    public class NPCChangeVisibilityScriptAction : ScriptAction
    {
        protected int? npcId = null;
        protected NPC npc;
        protected Visibility visibility;

        public NPCChangeVisibilityScriptAction(Visibility visibility)
        {
            this.visibility = visibility;
        }

        public NPCChangeVisibilityScriptAction(int npcId, Visibility visibility)
        {
            this.npcId = npcId;
            this.visibility = visibility;
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
            if (visibility == Visibility.VISIBLE)
            {
                npc.IsHidden = false;
            }
            else
            {
                npc.IsHidden = true;
            }
            return ScriptState.COMPLETED;
        }
    }
}
