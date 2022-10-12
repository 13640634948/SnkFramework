using System;
using SnkFramework.Mvvm.ViewModel;

namespace SampleDevelop.Test
{
    public enum WindowState
    {
        NONE,
        CREATE_BEGIN,
        CREATE_END,
        INIT_BEGIN,
        INIT_END,
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

    public abstract class SnkWindow<TViewOwner, TLayer, TViewModel> : SnkWindow,
        ISnkControllable<TViewOwner, TLayer, TViewModel>
        where TViewOwner : class, ISnkViewOwner
        where TLayer : class, ISnkUILayer
        where TViewModel : class, ISnkViewModel, new()
    {
        public new TViewOwner mOwner => base.mOwner as TViewOwner;
        public new TLayer mUILayer => base.mUILayer as TLayer;
        public new TViewModel mViewModel
        {
            get => base.mViewModel as TViewModel;
            protected set => base.mViewModel = value;
        }

        protected override Func<ISnkViewModel> ViewModelCreater => () => new TViewModel();
    }

    public abstract partial class SnkWindow : SnkWindowView, ISnkControllable
    {
        private SnkUIPage _mainPage;

        public virtual ISnkUIPage mMainPage
        {
            get
            {
                if (_mainPage == null)
                {
                    if (this.mPageList != null && this.mPageList.Count > 0)
                        _mainPage = this.mPageList[0];
                }

                return _mainPage;
            }
        }

        private bool _created = false;
        private bool _dismissed = false;
        private bool _activated = false;
        private int _priority = 0;

        private ISnkUILayer _uiLayer;

        public ISnkUILayer mUILayer
        {
            get => this._uiLayer;
            set => this._uiLayer = value;
        }

        public bool mCreated => this._created;
        public bool mDismissed => this._dismissed;

        public int mPriority
        {
            get => this._priority;
            set => this._priority = value;
        }

        public bool mActivated
        {
            get => this._activated;
            protected set
            {
                if (this._activated == value)
                    return;
                this._activated = value;
                this.OnActivatedChanged();
                this.raiseActivatedChanged();
            }
        }

        protected virtual void OnActivatedChanged()
        {
            this.mInteractable = this.mActivated;
        }

        //public bool mStateBroadcast = true;

        private WindowState _windowState = WindowState.NONE;

        public WindowState mWindowState
        {
            get => this._windowState;
            protected set
            {
                if (this._windowState.Equals(value))
                    return;
                WindowState oldState = this._windowState;
                this._windowState = value;
                this.raiseStateChanged(oldState, this._windowState);
            }
        }

        private ISnkTransition _dismissTransition;

        private readonly object _lock = new();
        private EventHandler _visibilityChanged;
        private EventHandler _activatedChanged;
        private EventHandler _onDismissed;
        private EventHandler<WindowStateEventArgs> _stateChanged;

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

        public event EventHandler mOnDismissed
        {
            add
            {
                lock (_lock) this._onDismissed += value;
            }
            remove
            {
                lock (_lock) this._onDismissed -= value;
            }
        }

        public event EventHandler<WindowStateEventArgs> StateChanged
        {
            add
            {
                lock (_lock) this._stateChanged += value;
            }
            remove
            {
                lock (_lock) this._stateChanged -= value;
            }
        }

        protected void raiseActivatedChanged() => this._activatedChanged?.Invoke(this, EventArgs.Empty);

        protected void raiseVisibilityChanged() => this._visibilityChanged?.Invoke(this, EventArgs.Empty);

        protected void raiseOnDismissed() => this._onDismissed?.Invoke(this, EventArgs.Empty);

        protected void raiseStateChanged(WindowState oldState, WindowState newState)
            => this._stateChanged?.Invoke(this, new WindowStateEventArgs(this, oldState, newState));

        protected override void OnVisibilityChanged()
        {
            base.OnVisibilityChanged();
            this.raiseVisibilityChanged();
        }

        public override void Create()
        {
            if (this._dismissTransition != null || this._dismissed)
                throw new ObjectDisposedException(this.mName);

            if (this._created)
                return;

            this.mWindowState = WindowState.CREATE_BEGIN;
            this.mVisibility = false;
            this.mInteractable = this.mActivated;
            this.onCreate();
            this._created = true;
            this.mWindowState = WindowState.CREATE_END;
            
            this.mWindowState = WindowState.INIT_BEGIN;
            this.onInitialize();
            this.mWindowState = WindowState.INIT_END;
            
            this.mViewModel = ViewModelCreater?.Invoke();
        }

        protected abstract Func<ISnkViewModel> ViewModelCreater { get; }

        protected virtual void onCreate()
        {
        }

        protected virtual void onInitialize()
        {
        }
    }
}