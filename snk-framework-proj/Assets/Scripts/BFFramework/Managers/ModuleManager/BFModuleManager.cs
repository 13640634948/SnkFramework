using System.Threading.Tasks;
using SnkFramework.NuGet.Basic;
using SnkFramework.Runtime.Basic.FiniteStateMachine;

namespace BFFramework.Runtime.Managers
{
    public class BFModuleManager : BFManager<BFModuleManager>, IBFModuleManager, ISnkFiniteStateMachineOwner
    {
        private readonly SnkFiniteStateMachine<BFModuleManager> _fsmModule;

        public BFModuleManager()
        {
            _fsmModule = new SnkFiniteStateMachine<BFModuleManager>(this);
        }

        public void RegisterModule<TModule>(object userData = null) where TModule : BFGameModule, new()
        {
            _fsmModule.RegisterState<TModule>(userData);
        }

        public async Task SwitchModuleAsync<T>(bool force, object userData = null) where T : BFGameModule
        {
            await this._fsmModule.Switch(typeof(T).Name, userData, !force);
        }
    }
}