namespace SnkFramework.NuGet.Features
{
    namespace Logging
    {
        public interface ISnkLoggerFactory
        {
            void AddLoggerProvider(ISnkLoggerProvider loggerProvider);

            ISnkLogger CreateLogger(string categoryName);

            ISnkLogger CreateLogger<T>();
        }
    }
}

