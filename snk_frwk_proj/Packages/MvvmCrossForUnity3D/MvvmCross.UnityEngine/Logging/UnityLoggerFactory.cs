using Microsoft.Extensions.Logging;

namespace MvvmCross.UnityEngine.Logging
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