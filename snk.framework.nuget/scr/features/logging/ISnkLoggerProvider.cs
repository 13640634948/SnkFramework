namespace SnkFramework.NuGet.Features
{
    namespace Logging
    {
        public interface ISnkLoggerProvider
        {
            void Output(eSnkLogType logType, System.Exception e,  string formater, params object[] message);
        }
    }
}

