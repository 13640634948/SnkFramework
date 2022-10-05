using UnityEngine;

namespace Loxodon.Log
{
    public class BindingLog : BindingLogBase
    {
        protected override void InternalOutputString(string flag, string format, params object[] args)
        {
            Debug.Log("[" + flag + "]" + string.Format(format, args));
        }
    }
}