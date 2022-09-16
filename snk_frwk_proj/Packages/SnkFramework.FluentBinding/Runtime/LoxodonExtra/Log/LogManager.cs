using System;
using System.Collections.Generic;

namespace Loxodon.Log
{
    public class LogManager
    {
        private static Dictionary<System.Type, ILog> _logDict = new Dictionary<Type, ILog>();
        
        public static ILog GetLogger(System.Type logType)
        {
            if (_logDict.TryGetValue(logType, out ILog log) == false)
            {
                log = new BindingLog(logType.FullName);
                _logDict.Add(logType, log);
            }
            return log;
        }
    }
}