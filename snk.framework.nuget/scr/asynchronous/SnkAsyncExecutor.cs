using System;

namespace SnkFramework.NuGet
{
    namespace Asynchronous
    {
        public abstract class SnkAsyncExecutor
        {
            protected static ISnkAsyncExecutor s_instance;

            public bool IsMainThread => s_instance.IsMainThread;

            public static object WaitWhile(Func<bool> predicate)
                => s_instance.WaitWhile(predicate);

            public static void RegiestAsyncExecutor<TAsyncExecutor>()
                where TAsyncExecutor : class, ISnkAsyncExecutor, new()
            {
                s_instance = new TAsyncExecutor();
            }
        }
    }
}