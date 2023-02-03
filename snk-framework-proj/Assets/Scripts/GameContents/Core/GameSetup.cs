using BFFramework.Runtime.Core;
using SnkFramework.Mvvm.Runtime.Presenters;

namespace GAME.Contents.Core
{
    public class GameSetup : BFGameSetup<GameApp>
    {
        protected override ISnkViewLoader CreateViewLoader() => new GameViewLoader();
    }
}