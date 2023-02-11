using SnkFramework.NuGet.Features.Logging;

namespace SnkFramework.Runtime
{
    public class SnkLogHost
    {
        private static ISnkLogger _defaultLogger;

        public static ISnkLogger Default
        {
            get
            {
                if (_defaultLogger == null)
                    _defaultLogger = GetLog("default");
                return _defaultLogger;
            }
        }

        public static ISnkLogger GetLog(string categoryName)
        {
            if (Snk.IoCProvider.TryResolve<ISnkLoggerFactory>(out var loggerFactory))
                return loggerFactory.CreateLogger(categoryName);
            return null;
        }

        public static ISnkLogger GetLog<T>() => GetLog(typeof(T).FullName);
    }
}