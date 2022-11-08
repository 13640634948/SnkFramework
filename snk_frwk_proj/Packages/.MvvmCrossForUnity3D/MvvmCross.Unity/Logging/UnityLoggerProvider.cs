using Microsoft.Extensions.Logging;

namespace MvvmCross.Unity.Logging
{
    public class UnityLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
            => new UnityLogger();
        
        public void Dispose()
        {
            
        }

    }
}