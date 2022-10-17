using Microsoft.Extensions.Logging;

namespace MvvmCross.UnityEngine.Logging
{
    public class UnityLoggerProvider : ILoggerProvider
    {
        public void Dispose()
        {
            
        }

        public ILogger CreateLogger(string categoryName)
        {
            throw new System.NotImplementedException();
        }
    }
}