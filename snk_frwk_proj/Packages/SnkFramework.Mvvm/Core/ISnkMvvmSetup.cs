using System;

namespace SnkFramework.Mvvm.Core
{
    public interface ISnkMvvmSetup
    {
        void InitializePrimary();
        void InitializeSecondary();

        event EventHandler<SnkMvvmSetupStateEventArgs>? StateChanged;
        SnkMvvmSetupState State { get; }
    }
}