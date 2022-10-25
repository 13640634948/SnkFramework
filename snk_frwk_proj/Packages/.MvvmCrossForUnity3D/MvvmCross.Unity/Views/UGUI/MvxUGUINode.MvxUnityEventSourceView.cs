using System;

namespace MvvmCross.Unity.Views.UGUI
{
    public partial class MvxUGUINode
    {
        private EventHandler _activatedChanged;
        private EventHandler _visibilityChanged;
        private readonly object _lock = new();

        public event EventHandler mVisibilityChanged
        {
            add
            {
                lock (_lock) this._visibilityChanged += value;
            }
            remove
            {
                lock (_lock) this._visibilityChanged -= value;
            }
        }

        public event EventHandler mActivatedChanged
        {
            add
            {
                lock (_lock) this._activatedChanged += value;
            }
            remove
            {
                lock (_lock) this._activatedChanged -= value;
            }
        }
        

        protected void raiseActivatedChanged() => this._activatedChanged?.Invoke(this, EventArgs.Empty);
        protected void raiseVisibilityChanged() => this._visibilityChanged?.Invoke(this, EventArgs.Empty);

    }
}