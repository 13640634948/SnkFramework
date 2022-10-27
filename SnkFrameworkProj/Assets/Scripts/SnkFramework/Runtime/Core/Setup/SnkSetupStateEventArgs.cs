using System;

namespace SnkFramework.Runtime.Core.Setup
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