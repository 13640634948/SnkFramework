namespace SnkFramework.NuGet.Features
{
    namespace Logging
    {
        public interface ISnkLogger
        {
            string CategoryName { get; }
            bool Enable { get; set; }

            void Debug(string message);
            void Debug(string formater, params object[] args);

            void Warning(string message);
            void Warning(string formater, params object[] args);

            void Info(string message);
            void Info(string formater, params object[] args);

            void Error(string message);
            void Error(string formater, params object[] args);

            void Exception(System.Exception e, string format, params object[] args);
        }
    }
}

