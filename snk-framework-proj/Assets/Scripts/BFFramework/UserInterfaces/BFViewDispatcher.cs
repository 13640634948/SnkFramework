using System;
using System.Threading.Tasks;
using GAME.Contents;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Runtime.Core;

namespace BFFramework.Runtime.UserInterface
{
    public class BFViewDispatcher : SnkViewDispatcher
    {
        private Lazy<ISnkMainThreadAsyncDispatcher> _mainThreadDispatcher = new Lazy<ISnkMainThreadAsyncDispatcher>(
            Game.Resolve<ISnkMainThreadAsyncDispatcher>);

        private ISnkMainThreadAsyncDispatcher mainThreadDispatcher => _mainThreadDispatcher.Value;
        
        public override Task ExecuteOnUIThreadAsync(Func<Task> action, bool maskExceptions = true)
            => mainThreadDispatcher?.ExecuteOnMainThreadAsync(action, maskExceptions);
    }
}