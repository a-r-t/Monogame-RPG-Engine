using Monogame_RPG_Engine.Engine.Core;
using System.Diagnostics;

namespace Monogame_RPG_Engine.App.Main
{
    public class Run
    {
        public static void Main(string[] args)
        {
            GameLoop game = new GameLoop();
            game.ScreenManager.SetCurrentScreen(new ScreenCoordinator());
            game.Run();
        }
    }
}
