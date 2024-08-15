using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Scene
{
    public class ScriptActionOutputManager
    {
        protected Dictionary<string, ScriptActionOutput> output = new Dictionary<string, ScriptActionOutput>();

        public void AddFlag(string flagName, string data)
        {
            if (output.ContainsKey(flagName))
            {
                output[flagName] = new ScriptActionOutput(data);
            }
            else
            {
                output.Add(flagName, new ScriptActionOutput(data));
            }
        }

        public void AddFlag(string flagName, int data)
        {
            if (output.ContainsKey(flagName))
            {
                output[flagName] = new ScriptActionOutput(data);
            }
            else
            {
                output.Add(flagName, new ScriptActionOutput(data));
            }
        }

        public void AddFlag(string flagName, bool data)
        {
            if (output.ContainsKey(flagName))
            {
                output[flagName] = new ScriptActionOutput(data);
            }
            else
            {
                output.Add(flagName, new ScriptActionOutput(data));
            }
        }

        public void AddFlag(string flagName, Keys data)
        {
            if (output.ContainsKey(flagName))
            {
                output[flagName] = new ScriptActionOutput(data);
            }
            else
            {
                output.Add(flagName, new ScriptActionOutput(data));
            }
        }

        public void AddFlag(string flagName, object data)
        {
            if (output.ContainsKey(flagName))
            {
                output[flagName] = new ScriptActionOutput(data);
            }
            else
            {
                output.Add(flagName, new ScriptActionOutput(data));
            }
        }

        public T GetFlagData<T>(string flagName)
        {
            return (T)output[flagName].Data;
        }
    }
}
