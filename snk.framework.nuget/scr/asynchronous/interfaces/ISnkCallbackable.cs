using System;

namespace SnkFramework.NuGet
{
    namespace Asynchronous
    {
        public interface ISnkCallbackable
        {
            void OnCallback(Action<ISnkAsyncResult> callback);
        }

        public interface ISnkCallbackable<TResult>
        {
            void OnCallback(Action<ISnkAsyncResult<TResult>> callback);
        }
    }
}