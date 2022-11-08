using Microsoft.Extensions.Logging;

namespace MvvmCross.Unity.Logging
{
    public class UnityLoggerFactory : ILoggerFactory
    {
        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
            => new UnityLogger();

        public void AddProvider(ILoggerProvider provider)
        {
        }
    }
}