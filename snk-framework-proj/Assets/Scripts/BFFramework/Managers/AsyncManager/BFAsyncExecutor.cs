using System;
using System.Threading;
using SnkFramework.NuGet.Asynchronous;
using UnityEngine;

namespace BFFramework.Runtime
{
    namespace Managers
    {
        internal class BFAsyncExecutor : ISnkAsyncExecutor
        {
            private SynchronizationContext syncContext;

            public BFAsyncExecutor()
            {
                syncContext = SynchronizationContext.Current;
            }

            public object WaitWhile(Func<bool> predicate)
            {
                //if (executor != null && IsMainThread)
                if (IsMainThread)
                    return new WaitWhile(predicate);

                throw new NotSupportedException("The function must execute on main thread.");
            }

            public bool IsMainThread => SynchronizationContext.Current == syncContext;
        }
    }
}