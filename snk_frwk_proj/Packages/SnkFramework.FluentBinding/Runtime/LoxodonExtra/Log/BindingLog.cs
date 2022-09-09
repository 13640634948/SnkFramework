using UnityEngine;

namespace Loxodon.Log
{
    public class BindingLog : BindingLogBase
    {
        public BindingLog(string name) : base(name)
        {
        }

        protected override void InternalOutputString(string flag, string format, params object[] args)
        {
            Debug.Log("[" + flag + "]" + string.Format(format, args) + "\nTargetType:" + base.mName);
        }
    }
}