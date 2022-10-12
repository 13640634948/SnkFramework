using SnkFramework.Mvvm.Log;
using UnityEngine;

namespace SnkFramework.Mvvm.LayoutEngine
{
    public class SnkMvvmLog : MvvmLogBase
    { 
        protected override void InternalOutputString(string flag, string format, params object[] args)
        {
            Debug.Log("[" + flag + "]" + string.Format(format, args));
        }
    }
}