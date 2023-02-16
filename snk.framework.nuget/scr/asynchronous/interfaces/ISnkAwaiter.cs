using System.Runtime.CompilerServices;

namespace SnkFramework.NuGet
{
    namespace Asynchronous
    {
        public interface ISnkAwaiter : ICriticalNotifyCompletion
        {
            bool IsCompleted { get; }

            void GetResult();
        }

        public interface IAwaiter<T> : ISnkAwaiter
        {
            new T GetResult();
        }
    }
}