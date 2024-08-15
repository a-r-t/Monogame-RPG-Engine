using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Utils
{
    public class KeyLocker
    {
        private HashSet<Keys> lockedKeys = new HashSet<Keys>();

        // lock a key
        public void LockKey(Keys key)
        {
            lockedKeys.Add(key);
        }

        // unlock a key
        public void UnlockKey(Keys key)
        {
            lockedKeys.Remove(key);
        }

        // check if a key is currently locked
        public bool IsKeyLocked(Keys key)
        {
            return lockedKeys.Contains(key);
        }
    }
}
