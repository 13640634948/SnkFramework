using System.Threading.Tasks;

namespace SnkFramework.Runtime.Basic
{
    namespace FiniteStateMachine
    {
        public interface ISnkState<TFSMOwner> where TFSMOwner : ISnkFiniteStateMachineOwner
        {
            void Initialize(object userData);
            Task OnEnter(TFSMOwner owner, object userData, string prevStateName);
            Task OnExit(TFSMOwner owner, string nextStateName);
        }
    }
}