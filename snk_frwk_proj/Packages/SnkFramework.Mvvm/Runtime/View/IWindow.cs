using System;
using SnkFramework.Mvvm.Base;
using SnkFramework.Mvvm.ViewModel;

namespace SnkFramework.Mvvm.View
{
    public enum WindowType
    {
        /// <summary>
        /// The full screen window.
        /// </summary>
        FULL,

        /// <summary>
        /// The pop-up window
        /// </summary>
        POPUP,

        /// <summary>
        /// The dialog window
        /// </summary>
        DIALOG,

        /// <summary>
        /// The progress bar dialog window
        /// </summary>
        PROGRESS,

        /// <summary>
        /// The Queued pop-up window.
        /// </summary>
        QUEUED_POPUP
    }

    public enum WindowState
    {
        NONE,
        CREATE_BEGIN,
        CREATE_END,
        ENTER_ANIMATION_BEGIN,
        VISIBLE,
        ENTER_ANIMATION_END,
        ACTIVATION_ANIMATION_BEGIN,
        ACTIVATED,
        ACTIVATION_ANIMATION_END,
        PASSIVATION_ANIMATION_BEGIN,
        PASSIVATED,
        PASSIVATION_ANIMATION_END,
        EXIT_ANIMATION_BEGIN,
        INVISIBLE,
        EXIT_ANIMATION_END,
        DISMISS_BEGIN,
        DISMISS_END
    }

    public class WindowStateEventArgs : EventArgs
    {
        private readonly WindowState oldState;
        private readonly WindowState state;
        private readonly IWindow window;

        public WindowStateEventArgs(IWindow window, WindowState oldState, WindowState newState)
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

        public IWindow Window
        {
            get { return this.window; }
        }
    }

    public interface IWindow : IWindowView, ISnkWindowControllable
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
        bool Activated { get; }

        /// <summary>
        /// The WindowManager of the window.
        /// </summary>
        //IWindowManager WindowManager { get; set; }
        IUILayer UILayer { get; set; }

        /// <summary>
        /// window type.
        /// </summary>
        WindowType WindowType { get; set; }

        /// <summary>
        /// The priority of the window.When pop-up windows are queued to open, 
        /// windows with higher priority will be opened first.
        /// </summary>
        int WindowPriority { get; set; }

        /// <summary>
        /// Create window
        /// </summary>
        /// <param name="bundle"></param>
        void Create(IBundle bundle = null);

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

    public interface IWindow<TViewModel> : IWindow, IWindowView<TViewModel>
        where TViewModel : class, IViewModel
    {
    }
}