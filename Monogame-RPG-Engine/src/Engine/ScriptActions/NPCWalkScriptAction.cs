using Engine.Scene;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ScriptActions
{
    public class NPCWalkScriptAction : ScriptAction
    {
        protected int? npcId = null;
        protected NPC npc;
        protected Direction direction;
        protected float distance;
        protected float speed;
        protected float amountMoved;

        public NPCWalkScriptAction(Direction direction, float distance, float speed)
        {
            this.direction = direction;
            this.distance = distance;
            this.speed = speed;
        }

        public NPCWalkScriptAction(int npcId, Direction direction, float distance, float speed)
        {
            this.npcId = npcId;
            this.direction = direction;
            this.distance = distance;
            this.speed = speed;
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
            amountMoved = 0;
        }

        public override ScriptState Execute()
        {
            npc.Walk(direction, speed);
            amountMoved += speed;
            if (amountMoved < distance)
            {
                return ScriptState.RUNNING;
            }
            return ScriptState.COMPLETED;
        }
    }
}
