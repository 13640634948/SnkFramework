using System;

namespace SnkFramework.NuGet
{
    namespace Asynchronous
    {
        public interface ISnkAsyncExecutor
        {
            object WaitWhile(Func<bool> predicate);
            bool IsMainThread { get; }
        }
    }
}