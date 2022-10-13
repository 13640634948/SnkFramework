using System;

namespace SnkFramework.Core.Extensions
{
#nullable enable
    public static class SnkObjectExtensions
    {
        public static void DisposeIfDisposable(this object thing)
        {
            if (thing is IDisposable disposable)
                disposable.Dispose();
        }
    }
#nullable restore
}