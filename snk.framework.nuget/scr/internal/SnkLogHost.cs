using SnkFramework.NuGet.Basic;

namespace SnkFramework.NuGet
{
    internal class SnkLogHost
    {
        /*
        private static ILogger? _defaultLogger;

        public static ILogger? Default => _defaultLogger ??= GetLog("Default");

        public static ILogger<T>? GetLog<T>() =>
            Mvx.IoCProvider?.TryResolve<ILoggerFactory>(out var loggerFactory) == true
                ? loggerFactory.CreateLogger<T>()
                : null;

        public static ILogger? GetLog(string categoryName) =>
            Mvx.IoCProvider?.TryResolve<ILoggerFactory>(out var loggerFactory) == true
                ? loggerFactory.CreateLogger(categoryName)
                : null;
        */

        private static ISnkLogger _defaultLogger;

        public static ISnkLogger Default
        {
            get
            {
                if( _defaultLogger == null)
                    _defaultLogger = Snk.Get<ISnkLogger>();
                return _defaultLogger;
            }
        }
    }
}

