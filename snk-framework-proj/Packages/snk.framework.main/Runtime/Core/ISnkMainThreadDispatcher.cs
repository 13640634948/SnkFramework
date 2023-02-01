using System;
using System.Threading.Tasks;

namespace SnkFramework.Runtime.Core.Setup
{
    public interface ISnkMainThreadDispatcher
    {
        Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true);
        Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true);
        bool IsOnMainThread { get; }
    }
}