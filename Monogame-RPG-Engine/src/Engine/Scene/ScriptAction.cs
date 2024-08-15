using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Scene;

namespace Engine.Scene
{
    public class ScriptAction
    {
        protected Map map;
        protected Player player;
        protected MapEntity entity;
        protected ScriptActionOutputManager outputManager;

        public virtual void Setup() { }

        public virtual ScriptState Execute()
        {
            return ScriptState.COMPLETED;
        }

        public virtual void Cleanup() { }

        public void SetMap(Map map)
        {
            this.map = map;
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
        }

        public void SetEntity(MapEntity entity)
        {
            this.entity = entity;
        }

        public void SetOutputManager(ScriptActionOutputManager outputManager)
        {
            this.outputManager = outputManager;
        }
    }
}
