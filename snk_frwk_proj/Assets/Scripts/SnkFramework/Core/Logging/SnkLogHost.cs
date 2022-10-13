using Microsoft.Extensions.Logging;
using SnkFramework.Runtime;

namespace SnkFramework.Core.Logging
{
#nullable enable
    public static class SnkLogHost
    {
        private static ILogger? _defaultLogger;

        public static ILogger? Default
        {
            get
            {
                return _defaultLogger ??= GetLog("Default");
            }
        }

        public static ILogger<T>? GetLog<T>()
        {
            if (Snk.IoCProvider.TryResolve<ILoggerFactory>(out var loggerFactory))
            {
                return loggerFactory.CreateLogger<T>();
            }

            return null;
        }

        public static ILogger? GetLog(string categoryName)
        {
            if (Snk.IoCProvider.TryResolve<ILoggerFactory>(out var loggerFactory))
            {
                return loggerFactory.CreateLogger(categoryName);
            }

            return null;
        }
    }
#nullable restore
}