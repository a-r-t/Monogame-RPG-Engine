using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_RPG_Engine.Engine.Scene.ScriptCore
{
    public class ScriptActionOutput
    {
        public object Data { get; private set; }

        public ScriptActionOutput(Object data)
        {
            Data = data;
        }

    }
}
