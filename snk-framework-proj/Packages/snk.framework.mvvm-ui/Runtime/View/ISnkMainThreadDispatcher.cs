using System;

namespace SnkFramework.Mvvm.Runtime
{
    namespace View
    {
        public interface ISnkMainThreadDispatcher
        {
            //bool RequestMainThreadAction(Action action, bool maskExceptions = true);
            bool IsOnMainThread { get; }
        }
    }
}