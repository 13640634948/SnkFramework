using System;
using System.Threading.Tasks;

namespace SnkFramework.Mvvm.Runtime
{
    namespace View
    {
        public interface ISnkMainThreadAsyncDispatcher : ISnkMainThreadDispatcher
        {
            Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true);
            Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true);
        }
    }
}