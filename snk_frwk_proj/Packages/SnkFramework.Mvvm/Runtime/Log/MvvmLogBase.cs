namespace SnkFramework.Mvvm.Log
{
    public abstract class MvvmLogBase : IMvvmLog
    {
        public bool IsInfoEnabled { get; set; } = true;
        public bool IsWarnEnabled { get; set; } = true;
        public bool IsErrorEnabled { get; set; } = true;
        public bool IsErrorEnabledx { get; set; } = true;
 
        protected abstract void InternalOutputString(string flag, string format, params object[] args);

        public void Info(object message)
        {
            InternalOutputString("Info", $"{message}");
        }

        public void Warn(object message)
        {
            InternalOutputString("Warn", $"{message}");
        }

        public void Error(object message)
        {
            InternalOutputString("Error", $"{message}");
        }

        public void InfoFormat(string format, params object[] args)
        {
            InternalOutputString("Info", format, args);
        }

        public void WarnFormat(string format, params object[] args)
        {
            InternalOutputString("Warn", format, args);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            InternalOutputString("Error", format, args);
        }
    }
}