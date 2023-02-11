using System;

namespace SnkFramework.Runtime
{
    namespace Core
    {
        public interface ISnkMainThreadDispatcher
        {
            //bool RequestMainThreadAction(Action action, bool maskExceptions = true);
            bool IsOnMainThread { get; }
        }
    }
}