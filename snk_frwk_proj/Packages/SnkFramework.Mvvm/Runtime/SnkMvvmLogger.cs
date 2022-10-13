using SnkFramework.Mvvm.Core.Log;
using UnityEngine;

namespace SnkFramework.Mvvm.Runtime
{
    public class SnkMvvmLogger : SnkMvvmLoggerBase
    { 
        protected override void InternalOutputString(string flag, string format, params object[] args)
        {
            Debug.Log("[" + flag + "]" + string.Format(format, args));
        }
    }
}