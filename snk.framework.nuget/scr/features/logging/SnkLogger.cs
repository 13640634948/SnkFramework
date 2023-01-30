namespace SnkFramework.NuGet.Features
{
    namespace Logging
    {
        public class SnkLogger : ISnkLogger
        {
            private ISnkLoggerProvider _logProvider;
            private bool _enable;

            public string CategoryName { get; }

            public virtual bool Enable
            {
                get => this._enable;
                set => this._enable = value;
            }

            public SnkLogger(string categoryName, ISnkLoggerProvider logProvider)
            {
                this.CategoryName = categoryName;
                this._logProvider = logProvider;
            }

            public virtual void Debug(string message)
            {
                this._logProvider.Output(eSnkLogType.debug, null, message);
            }

            public virtual void Debug(string formater, params object[] args)
            {
                this._logProvider.Output(eSnkLogType.debug, null, formater, args);
            }

            public virtual void Info(string message)
            {
                this._logProvider.Output(eSnkLogType.info, null, message);
            }

            public virtual void Info(string formater, params object[] args)
            {
                this._logProvider.Output(eSnkLogType.info, null, formater, args);
            }

            public virtual void Warning(string message)
            {
                this._logProvider.Output(eSnkLogType.warning, null, message);
            }

            public virtual void Warning(string formater, params object[] args)
            {
                this._logProvider.Output(eSnkLogType.warning, null, formater, args);
            }

            public virtual void Error(string message)
            {
                this._logProvider.Output(eSnkLogType.error, null, message);
            }

            public virtual void Error(string formater, params object[] args)
            {
                this._logProvider.Output(eSnkLogType.error, null, formater, args);
            }

            public virtual void Exception(System.Exception e, string formater, params object[] args)
            {
                this._logProvider.Output(eSnkLogType.error, e, formater, args);
            }
        }
    }
}

