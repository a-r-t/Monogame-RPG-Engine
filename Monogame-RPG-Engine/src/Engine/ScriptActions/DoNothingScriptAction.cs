using Engine.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// This is only used to prevent a crash from happening with the ConditionalScriptAction if none of its group's requirements are met
// It is not needed otherwise
namespace Engine.ScriptActions
{
    public class DoNothingScriptAction : ScriptAction
    {
    }
}
