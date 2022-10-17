using System;
using Microsoft.Extensions.Logging;
using UUnityEngine = UnityEngine;

namespace MvvmCross.UnityEngine.Logging
{
    public class UnityLogger : ILogger
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            UUnityEngine.Debug.Log(formatter.Invoke(state, exception));
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state) => default;
    }
}