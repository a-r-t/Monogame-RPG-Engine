using Engine.Scene;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ScriptActions
{
    public class NPCChangeVisibilityScriptAction : ScriptAction
    {
        protected NPC npc;
        protected Visibility visibility;

        public NPCChangeVisibilityScriptAction(Visibility visibility)
        {
            this.visibility = visibility;
        }

        public NPCChangeVisibilityScriptAction(int npcId, Visibility visibility)
        {
            this.npc = map.GetNPCById(npcId);
            this.visibility = visibility;
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
