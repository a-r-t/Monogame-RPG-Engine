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
    public class MapEntityChangeVisibilityScriptAction : ScriptAction
    {
        protected MapEntity entityToChange;
        protected Visibility visibility;

        public MapEntityChangeVisibilityScriptAction(MapEntity entity, Visibility visibility)
        {
            this.entityToChange = entity;
            this.visibility = visibility;
        }

        public MapEntityChangeVisibilityScriptAction(Visibility visibility)
        {
            this.visibility = visibility;
        }

        public override void Setup()
        {
            if (this.entity == null)
            {
  
                throw new Exception("No entity specified!");
            }
        }

        public override ScriptState Execute()
        {
            if (visibility == Visibility.VISIBLE)
            {
                entity.IsHidden = false;
            }
            else
            {
                entity.IsHidden = true;
            }
            return ScriptState.COMPLETED;
        }
    }
}
