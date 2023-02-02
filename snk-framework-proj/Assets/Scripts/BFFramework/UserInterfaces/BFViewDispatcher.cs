using System;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.View;
using SnkFramework.Runtime;
using SnkFramework.Runtime.Core;

namespace BFFramework.Runtime.UserInterface
{
    public class BFViewDispatcher : SnkViewDispatcher
    {
        private ISnkMainThreadAsyncDispatcher _mainThreadDispatcher=> Snk.IoCProvider.Resolve<ISnkMainThreadAsyncDispatcher>();
        
        public override Task ExecuteOnUIThreadAsync(Func<Task> action, bool maskExceptions = true)
            => _mainThreadDispatcher?.ExecuteOnMainThreadAsync(action, maskExceptions);
    }
}