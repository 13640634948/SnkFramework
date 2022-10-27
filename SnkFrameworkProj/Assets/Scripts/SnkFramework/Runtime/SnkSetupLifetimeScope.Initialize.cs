using System;
using VContainer;

namespace SnkFramework.Runtime
{
    public partial class SnkSetupLifetimeScope
    {
        public event EventHandler<SnkSetupStateEventArgs>? StateChanged;

        private SnkSetupState _state;

        public SnkSetupState State
        {
            get => _state;
            private set
            {
                if(_state == value)
                    return;
                _state = value;
                FireStateChange(value);
            }
        }
        
        private void FireStateChange(SnkSetupState state)
        {
            StateChanged?.Invoke(this, new SnkSetupStateEventArgs(state));
        }
        
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

        public virtual void InitializeSecondary(IContainerBuilder builder)
        {
            if (State != SnkSetupState.InitializedPrimary)
                return;
            State = SnkSetupState.InitializingSecondary;

            InitializeLastChance(builder);
            State = SnkSetupState.Initialized;
        }

        protected virtual void InitializeLoggingServices(IContainerBuilder builder)
        {
        }

        protected virtual void RegisterDefaultSetupDependencies()
        {
        }

        protected virtual void InitializeFirstChance(IContainerBuilder builder)
        {
        }

        protected virtual void InitializeLastChance(IContainerBuilder builder)
        {
        }
    }
}