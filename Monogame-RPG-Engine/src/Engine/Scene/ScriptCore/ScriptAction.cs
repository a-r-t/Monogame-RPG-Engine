using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monogame_RPG_Engine.Engine.Scene;
using Monogame_RPG_Engine.Engine.Scene.EntitiesCore;
using Monogame_RPG_Engine.Engine.Scene.MapCore;
using Monogame_RPG_Engine.Engine.Scene.PlayerCore;

namespace Monogame_RPG_Engine.Engine.Scene.ScriptCore
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
