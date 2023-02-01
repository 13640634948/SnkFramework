using System;

namespace SnkFramework.Runtime
{
    namespace Core
    {
        public class SnkSetupStateEventArgs : EventArgs
        {
            public SnkSetupStateEventArgs(eSnkSetupState setupState)
            {
                SetupState = setupState;
            }

            public eSnkSetupState SetupState { get; }
        }
    }
}