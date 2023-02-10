using System.Threading.Tasks;

namespace SnkFramework.Runtime.Basic
{
    namespace FiniteStateMachine
    {
        public abstract class SnkState<TFSMOwner> : ISnkState<TFSMOwner> where TFSMOwner : ISnkFiniteStateMachineOwner
        {
            public virtual void Initialize(object userData)
            {
            }

            public abstract Task OnEnter(TFSMOwner owner, object userData, string prevStateName);
            public abstract Task OnExit(TFSMOwner owner, string nextStateName);
        }
    }
}