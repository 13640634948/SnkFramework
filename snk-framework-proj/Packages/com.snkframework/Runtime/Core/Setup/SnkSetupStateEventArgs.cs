using System;

namespace SnkFramework
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