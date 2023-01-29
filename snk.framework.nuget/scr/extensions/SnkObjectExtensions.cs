using System;

namespace SnkFramework.NuGet
{
    namespace Extensions
    {
        public static class SnkObjectExtensions
        {
            public static void DisposeIfDisposable(this object thing)
            {
                if (thing is IDisposable disposable)
                    disposable.Dispose();
            }
        }
    }
}



