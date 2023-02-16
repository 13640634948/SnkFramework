using System;

namespace SnkFramework.NuGet
{
    namespace Asynchronous
    {
        public interface ISnkSynchronizable
        {
            bool WaitForDone();

            object WaitForResult(int millisecondsTimeout = 0);

            object WaitForResult(TimeSpan timeout);
        }

        public interface ISnkSynchronizable<TResult> : ISnkSynchronizable
        {
            new TResult WaitForResult(int millisecondsTimeout = 0);

            new TResult WaitForResult(TimeSpan timeout);
        }
    }
}