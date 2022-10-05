using System;
using SnkFramework.Mvvm.Base;

namespace SnkFramework.Mvvm.View
{
    public interface IWindow : IWindowView
    {
        /// <summary>
        /// Triggered when the Activated's value to be changed.
        /// </summary>
        event EventHandler ActivatedChanged;

        /// <summary>
        /// Triggered when the window is dismissed.
        /// </summary>
        event EventHandler OnDismissed;

        /// <summary>
        /// Triggered when the WindowState's value to be changed.
        /// </summary>
        event EventHandler<WindowStateEventArgs> StateChanged;

        /// <summary>
        /// The name of the window.
        /// </summary>
        //string Name { get; set; }

        /// <summary>
        /// Returns  "true" if this window created.
        /// </summary>
        bool Created { get; }

        /// <summary>
        /// Returns  "true" if this window dismissed.
        /// </summary>
        bool Dismissed { get; }

        /// <summary>
        /// Returns  "true" if this window visibility.
        /// </summary>
        //bool Visibility { get; }

        /// <summary>
        /// Returns  "true" if this window activated.
        /// </summary>
        bool mActivated { get; }

        public int mSortingOrder { get; set; }

        /// <summary>
        /// The WindowManager of the window.
        /// </summary>
        //IWindowManager WindowManager { get; set; }
        IUILayer UILayer { get; set; }

        /// <summary>
        /// window type.
        /// </summary>
        WIN_TYPE WinType { get; set; }

        /// <summary>
        /// The priority of the window.When pop-up windows are queued to open, 
        /// windows with higher priority will be opened first.
        /// </summary>
        int WindowPriority { get; set; }

        /// <summary>
        /// Create window
        /// </summary>
        /// <param name="bundle"></param>
        IWindow Create(IBundle bundle = null);

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
//        void Show(bool ignoreAnimation = false);
        ITransition Show(bool ignoreAnimation = false);

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        ITransition Hide(bool ignoreAnimation = false);

        /// <summary>
        /// 
        /// </summary>
        ITransition Dismiss(bool ignoreAnimation = false);
    }
}