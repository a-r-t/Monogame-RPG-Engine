using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Scene
{
    public class FlagManager
    {
        protected Dictionary<string, bool> flags = new Dictionary<string, bool>();

        public void AddFlag(string flagName)
        {
            flags.Add(flagName, false);
        }

        public void AddFlag(string flagName, bool startingValue)
        {
            flags.Add(flagName, startingValue);
        }

        public void SetFlag(string flagName)
        {
            if (flags.ContainsKey(flagName))
            {
                flags[flagName] = true;
            }
        }

        public void UnsetFlag(string flagName)
        {
            if (flags.ContainsKey(flagName))
            {
                flags[flagName] = false;
            }
        }

        public void Reset()
        {
            foreach (string key in flags.Keys.ToList())
            {
                flags[key] = false;
            }
        }

        public bool IsFlagSet(string flagName)
        {
            if (flags.ContainsKey(flagName))
            {
                return flags[flagName];
            }
            return false;
        }
    }
}


