using System;

namespace SnkFramework.Mvvm.Core
{
    public class SnkMvvmSetupStateEventArgs : EventArgs
    {
        public SnkMvvmSetupStateEventArgs(SnkMvvmSetupState setupState)
        {
            SetupState = setupState;
        }

        public SnkMvvmSetupState SetupState { get; }
    }
}