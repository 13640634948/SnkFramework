using Microsoft.Extensions.Logging;

namespace MvvmCross.UnityEngine.Logging
{
    public class UnityLoggerFactory : ILoggerFactory
    {
        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            throw new System.NotImplementedException();
        }

        public void AddProvider(ILoggerProvider provider)
        {
            throw new System.NotImplementedException();
        }
    }
}