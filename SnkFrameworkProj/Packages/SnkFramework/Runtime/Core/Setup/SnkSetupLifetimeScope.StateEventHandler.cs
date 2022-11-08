using System;

namespace SnkFramework.Runtime.Core.Setup
{
    public partial class SnkSetupLifetimeScope
    {
        public event EventHandler<SnkSetupStateEventArgs> StateChanged;

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
    }
}