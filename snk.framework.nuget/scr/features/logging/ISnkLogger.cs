namespace SnkFramework.NuGet.Features
{
    namespace Logging
    {
        public interface ISnkLogger
        {
            string CategoryName { get; }
            bool Enable { get; set; }

            bool IsWarnEnabled { get; }

            void Debug(string message);
            void DebugFormat(string formater, params object[] args);

            void Warning(string message);
            void WarnFormat(string formater, params object[] args);

            void Info(string message);
            void InfoFormat(string formater, params object[] args);

            void Error(string message);
            void ErrorFormat(string formater, params object[] args);

            void Exception(System.Exception e, string format, params object[] args);
        }
    }
}

