using System;

namespace SnkFramework.Mvvm.Runtime.View
{
    public class WindowStateEventArgs : EventArgs
    {
        private readonly WIN_STATE oldState;
        private readonly WIN_STATE state;
        private readonly ISnkWindow window;

        public WindowStateEventArgs(ISnkWindow window, WIN_STATE oldState, WIN_STATE newState)
        {
            this.window = window;
            this.oldState = oldState;
            this.state = newState;
        }

        public WIN_STATE OldState
        {
            get { return this.oldState; }
        }

        public WIN_STATE State
        {
            get { return this.state; }
        }

        public ISnkWindow Window
        {
            get { return this.window; }
        }
    }
}