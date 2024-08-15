using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Scene;
using Engine.ScriptActions.Conditional;

// This class is a base class for all scripts in the game -- all scripts should extend from it
// Scripts can be used to interact with map entities
// Each script defines a set of instructions that will be carried out by the game when it is set to active
// Some examples include interact scripts (such as talking to an NPC) and trigger scripts (scripts that activate when the player walks on them)
namespace Engine.Scene
{
    public abstract class Script
    {
        public List<ScriptAction> ScriptActions { get; private set; }
        private int currentScriptActionIndex;

        // this is set to true if script is currently being executed
        private bool isActive = false;
        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                if (value && HasScriptActions())
                {

                    isActive = value;
                    currentScriptActionIndex = 0;
                    ScriptActions[currentScriptActionIndex].Setup();
                }
                else
                {
                    isActive = value;
                }
            }
        }

        // if true, script should perform "setup" logic
        protected bool start = true;

        // references to the map entity the script is attached to
        protected MapEntity entity;

        // reference to the map instance which can be used in any script
        protected Map map;

        // reference to the player instance which can be used in any script
        protected Player player;

        public ScriptActionOutputManager ScriptActionOutputManager { get; private set; }

        public Script()
        {
            ScriptActionOutputManager = new ScriptActionOutputManager();
        }

        public Map GetMap() { return map; }
        public void SetMap(Map map) { this.map = map; }
        public Player GetPlayer() { return player; }
        public void SetPlayer(Player player) { this.player = player; }
        public MapEntity GetEntity() { return entity; }
        public void SetMapEntity(MapEntity entity) { this.entity = entity; }

        public void Initialize()
        {
            // load script actions from subclass
            ScriptActions = LoadScriptActions();

            // recursively iterate through all script actiohns and set the necessary properties on them
            // the recursive part is needed due to conditionals having nested script actions, and those conditionals can have nested conditionals, etc.
            Queue<ScriptAction> scriptActionsToInitialize = new Queue<ScriptAction>();
            foreach (ScriptAction scriptAction in ScriptActions)
            {
                scriptActionsToInitialize.Enqueue(scriptAction);
            }
            while (scriptActionsToInitialize.Count > 0)
            {
                ScriptAction scriptAction = scriptActionsToInitialize.Dequeue();
                scriptAction.SetMap(map);
                scriptAction.SetPlayer(player);
                scriptAction.SetEntity(entity);
                scriptAction.SetOutputManager(ScriptActionOutputManager);
                if (scriptAction is ConditionalScriptAction)
                {
                    ConditionalScriptAction conditionalScriptAction = (ConditionalScriptAction)scriptAction;
                    foreach (ConditionalScriptActionGroup conditionalScriptActionGroup in conditionalScriptAction.ConditionalScriptActionGroups)
                    {
                        foreach (ScriptAction conditionalScriptActionGroupScriptAction in conditionalScriptActionGroup.ScriptActions)
                        {
                            scriptActionsToInitialize.Enqueue(conditionalScriptActionGroupScriptAction);
                        }
                    }
                }
            }
        }

        public abstract List<ScriptAction> LoadScriptActions();

        private bool HasScriptActions()
        {
            return ScriptActions.Count > 0;
        }

        public void Update()
        {
            // Runs an execute cycle of the Script
            ScriptAction currentScriptAction = ScriptActions[currentScriptActionIndex];
            ScriptState scriptState = currentScriptAction.Execute();
            if (scriptState == ScriptState.COMPLETED)
            {
                currentScriptAction.Cleanup();
                currentScriptActionIndex++;

                if (currentScriptActionIndex < ScriptActions.Count)
                {
                    ScriptActions[currentScriptActionIndex].Setup();
                }
                else
                {
                    IsActive = false;
                    map.ActiveScript = null;
                }
            }
        }
    }
}
