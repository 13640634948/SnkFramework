using VContainer;

namespace SnkFramework.Runtime.Core.Setup
{
    public partial class SnkSetupLifetimeScope
    {
        public virtual void InitializePrimary(IContainerBuilder builder)
        {
            if (State != SnkSetupState.Uninitialized)
                return;
            State = SnkSetupState.InitializingPrimary;
            
            InitializeLoggingServices(builder);
            
            RegisterDefaultSetupDependencies();
            InitializeFirstChance(builder);
            State = SnkSetupState.InitializedPrimary;
        }
        
        protected virtual void InitializeFirstChance(IContainerBuilder builder)
        {
        }

     
        protected virtual void InitializeLoggingServices(IContainerBuilder builder)
        {
        }

        protected virtual void RegisterDefaultSetupDependencies()
        {
        }

    }
}