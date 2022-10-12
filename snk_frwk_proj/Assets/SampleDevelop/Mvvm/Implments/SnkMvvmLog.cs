using UnityEngine;

namespace SnkFramework.Mvvm.Log
{
    public class SnkMvvmLog : MvvmLogBase
    { 
        protected override void InternalOutputString(string flag, string format, params object[] args)
        {
            Debug.Log("[" + flag + "]" + string.Format(format, args));
        }
    }
}