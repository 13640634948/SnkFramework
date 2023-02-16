namespace SnkFramework.NuGet
{
    namespace Asynchronous
    {
        public interface ISnkProgressPromise<TProgress> : ISnkPromise
        {
            TProgress Progress { get; }

            void UpdateProgress(TProgress progress);
        }

        public interface ISnkProgressPromise<TProgress, TResult> : ISnkProgressPromise<TProgress>, ISnkPromise<TResult>
        {
        }
    }
}