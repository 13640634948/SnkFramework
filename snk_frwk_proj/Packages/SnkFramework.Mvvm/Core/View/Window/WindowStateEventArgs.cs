using System;

namespace SnkFramework.Mvvm.Core
{
    namespace View
    {
        public class WindowStateEventArgs : EventArgs
        {
            private readonly WindowState oldState;
            private readonly WindowState state;
            private readonly ISnkWindow window;

            public WindowStateEventArgs(ISnkWindow window, WindowState oldState, WindowState newState)
            {
                this.window = window;
                this.oldState = oldState;
                this.state = newState;
            }

            public WindowState OldState
            {
                get { return this.oldState; }
            }

            public WindowState State
            {
                get { return this.state; }
            }

            public ISnkWindow Window
            {
                get { return this.window; }
            }
        }
    }
}