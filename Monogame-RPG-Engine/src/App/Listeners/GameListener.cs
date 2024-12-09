using Engine.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Listeners
{
    public interface GameListener : BaseGameListener
    {
        void OnWin();
    }
}
