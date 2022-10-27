using System;

namespace SnkFramework.Runtime
{
    public class SnkSetupStateEventArgs : EventArgs
    {
        public SnkSetupStateEventArgs(SnkSetupState setupState)
        {
            SetupState = setupState;
        }

        public SnkSetupState SetupState { get; }
    }
}