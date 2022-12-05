using System;

namespace SnkFramework.Mvvm.Runtime
{
    namespace View
    {
        public interface ISnkMainThreadDispatcher
        {
            bool IsOnMainThread { get; }
            void ExecuteOnMainThread(Action action, bool maskExceptions = true);
        }
    }
}