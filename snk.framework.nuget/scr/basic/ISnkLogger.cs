namespace SnkFramework.NuGet
{
    namespace Basic
    {
        public enum eLogType
        {
            info,
            warnning,
            error,
        }

        public interface ISnkLogger
        {
            bool Enable { get; set; }
            void Print(eLogType logType, string message);
            void Print(eLogType logType, string formater, string message);
        }
    }
}

