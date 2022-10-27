using VContainer;

namespace SnkFramework.Runtime.Core.Setup
{
    public partial class SnkSetupLifetimeScope
    {
        public virtual void InitializeSecondary(IContainerBuilder builder)
        {
            if (State != SnkSetupState.InitializedPrimary)
                return;
            State = SnkSetupState.InitializingSecondary;

            InitializeLastChance(builder);
            State = SnkSetupState.Initialized;
        }

        protected virtual void InitializeLastChance(IContainerBuilder builder)
        {
        }
    }
}