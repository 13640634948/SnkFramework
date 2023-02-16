using System;

namespace SnkFramework.NuGet
{
    namespace Asynchronous
    {
        public interface ISnkProgressCallbackable<TProgress>
        {
            void OnCallback(Action<ISnkProgressResult<TProgress>> callback);

            void OnProgressCallback(Action<TProgress> callback);
        }

        public interface ISnkProgressCallbackable<TProgress, TResult>
        {
            void OnCallback(Action<ISnkProgressResult<TProgress, TResult>> callback);

            void OnProgressCallback(Action<TProgress> callback);
        }
    }
}