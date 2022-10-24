namespace SnkFramework.Mvvm.Core
{
    namespace Log
    {
        public interface ISnkMvvmLogger
        {
            void Info(object message);
            void Warn(object message);
            void Error(object message);

            void InfoFormat(string format, params object[] args);
            void WarnFormat(string format, params object[] args);
            void ErrorFormat(string format, params object[] args);

            bool IsInfoEnabled { get; }
            bool IsWarnEnabled { get; }
            bool IsErrorEnabled { get; }
        }
    }
}