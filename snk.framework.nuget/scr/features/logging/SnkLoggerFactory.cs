using System.Collections.Generic;

namespace SnkFramework.NuGet.Features
{
    namespace Logging
    {
        public class SnkLoggerFactory : ISnkLoggerFactory
        {
            private ISnkLoggerProvider _loggerProvider;

            protected Dictionary<string, ISnkLogger> _dictionary = new Dictionary<string, ISnkLogger>();

            public void AddLoggerProvider(ISnkLoggerProvider loggerProvider)
            {
                this._loggerProvider = loggerProvider;
            }

            public virtual ISnkLogger CreateLogger(string categoryName)
            {
                if (_loggerProvider == null)
                    throw new System.ArgumentNullException("loggerProvider is null");

                string key = categoryName + _loggerProvider.GetType().FullName;
                if (this._dictionary.TryGetValue(key, out var logger))
                    return logger;

                logger = new SnkLogger(categoryName, _loggerProvider);
                this._dictionary.Add(key, logger);
                return logger;
            }

            public ISnkLogger CreateLogger<T>()
                => CreateLogger(typeof(T).FullName);
        }
    }
}

