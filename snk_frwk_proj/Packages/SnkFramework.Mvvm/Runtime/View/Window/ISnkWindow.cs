using System;

namespace SampleDevelop.Test
{
    public interface ISnkWindow : ISnkWindowView
    {
        public ISnkUIPage mMainPage { get; }
        public WindowState mWindowState { get; }

        public ISnkUILayer mUILayer { get; set; }

        public bool mCreated { get; }
        public bool mActivated { get; }
        public bool mDismissed { get; }

        public int mPriority { get; set; }

        public event EventHandler mVisibilityChanged;
        public event EventHandler mActivatedChanged;
        public event EventHandler mOnDismissed;
        public event EventHandler<WindowStateEventArgs> StateChanged;

        public ISnkTransition Show(bool ignoreAnimation = false);
        public ISnkTransition Hide(bool ignoreAnimation = false);
        public ISnkTransition Dismiss(bool ignoreAnimation = false);
    }
}