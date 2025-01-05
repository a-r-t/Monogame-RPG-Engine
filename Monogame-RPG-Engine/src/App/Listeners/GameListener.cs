using Monogame_RPG_Engine.Engine.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_RPG_Engine.App.Listeners
{
    public interface GameListener : BaseGameListener
    {
        void OnWin();
    }
}
