using System.Threading.Tasks;
using SnkFramework.Runtime.Basic.FiniteStateMachine;
using UnityEngine;

namespace BFFramework.Runtime.Managers
{
    public abstract class BFGameModule : SnkState<BFModuleManager>, IBFGameModule
    {
        public override async Task OnEnter(BFModuleManager owner, object userData, string prevStateName)
        {
            Debug.Log(this.GetType().Name + "-Enter, fromState:" + prevStateName);
        }

        public override async Task OnExit(BFModuleManager owner, string nextStateName)
        {
            Debug.Log(this.GetType().Name + "-Exit, nextState:" + nextStateName);
        }
    }
}