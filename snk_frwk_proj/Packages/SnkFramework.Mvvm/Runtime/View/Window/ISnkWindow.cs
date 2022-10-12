using System;
using SnkFramework.Mvvm.ViewModel;

namespace SnkFramework.Mvvm.View
{
    public interface ISnkWindow<TViewOwner, TLayer, TViewModel> : ISnkWindow<TLayer>, ISnkWindowView<TViewOwner,TViewModel>
        where TViewOwner : class, ISnkViewOwner
        where TLayer : class, ISnkUILayer
        where TViewModel : class, ISnkViewModel
    {
        
    }   
    
    public interface ISnkWindow<TLayer> : ISnkWindow
        where TLayer : class, ISnkUILayer
    {
        public new ISnkUILayer mUILayer { get; set; }
    }

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