using System;

namespace SnkFramework.Mvvm.View
{
    public class WindowStateEventArgs : EventArgs
    {
        private readonly WIN_STATE oldState;
        private readonly WIN_STATE state;
        private readonly IWindow window;

        public WindowStateEventArgs(IWindow window, WIN_STATE oldState, WIN_STATE newState)
        {
            this.window = window;
            this.oldState = oldState;
            this.state = newState;
        }

        public WIN_STATE OldState => this.oldState;

        public WIN_STATE State => this.state;

        public IWindow Window => this.window;
    }
}