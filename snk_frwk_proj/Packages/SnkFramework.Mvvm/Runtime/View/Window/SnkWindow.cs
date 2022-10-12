using System;
using SnkFramework.Mvvm.ViewModel;

namespace SnkFramework.Mvvm.View
{
    public abstract class SnkWindow<TViewOwner, TLayer, TViewModel> : SnkWindow,
        ISnkWindowControllable<TViewOwner, TLayer, TViewModel>
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

    public abstract class SnkWindow : SnkWindowView, ISnkWindowControllable
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

        private bool _activated = false;

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

            base.Create();

            this.mWindowState = WindowState.CREATE_BEGIN;
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

        protected virtual void OnShow()
        {
        }

        protected virtual void OnHide()
        {
        }

        protected virtual void OnDismiss()
        {
        }

        public ISnkTransition Show(bool ignoreAnimation = false)
        {
            if (this._dismissTransition != null || this._dismissed)
                throw new InvalidOperationException("The window has been destroyed");

            if (this.mVisibility)
                throw new InvalidOperationException("The window is already visible.");

            return this.mUILayer.Show(this).DisableAnimation(ignoreAnimation);
        }

        public ISnkTransition Hide(bool ignoreAnimation = false)
        {
            if (!this._created)
                throw new InvalidOperationException("The window has not been created.");

            if (this._dismissed)
                throw new InvalidOperationException("The window has been destroyed.");

            if (!this.mVisibility)
                throw new InvalidOperationException("The window is not visible.");

            return this.mUILayer.Hide(this).DisableAnimation(ignoreAnimation);
        }

        public ISnkTransition Dismiss(bool ignoreAnimation = false)
        {
            if (this._dismissTransition != null)
                return this._dismissTransition;

            if (this._dismissed)
                throw new InvalidOperationException(string.Format("The window[{0}] has been destroyed.", this.mName));

            this._dismissTransition = this.mUILayer.Dismiss(this).DisableAnimation(ignoreAnimation);
            return this._dismissTransition;
        }


        public IAsyncResult Activate(bool ignoreAnimation)
        {
            UIAsyncResult result = new UIAsyncResult();
            try
            {
                if (this.mVisibility == false)
                {
                    result.SetException(new InvalidOperationException("The window is not visible."));
                    return result;
                }

                if (this.mActivated)
                {
                    result.SetResult();
                    return result;
                }

                if (!ignoreAnimation && this.mActivationAnimation != null)
                {
                    this.mActivationAnimation.OnStart(() =>
                    {
                        this.mWindowState = WindowState.ACTIVATION_ANIMATION_BEGIN;
                    }).OnEnd(() =>
                    {
                        this.mWindowState = WindowState.ACTIVATION_ANIMATION_END;
                        this.mActivated = true;
                        this.mWindowState = WindowState.ACTIVATED;
                        result.SetResult();
                    }).Play();
                }
                else
                {
                    this.mActivated = true;
                    this.mWindowState = WindowState.ACTIVATED;
                    result.SetResult();
                }
            }
            catch (Exception e)
            {
                result.SetException(e);
            }

            return result;
        }

        public IAsyncResult Passivate(bool ignoreAnimation)
        {
            UIAsyncResult result = new UIAsyncResult();
            try
            {
                if (this.mVisibility == false)
                {
                    result.SetException(new InvalidOperationException("The window is not visible."));
                    return result;
                }

                if (this.mActivated == false)
                {
                    result.SetResult();
                    return result;
                }

                this.mActivated = false;
                this.mWindowState = WindowState.PASSIVATED;

                if (!ignoreAnimation && this.mPassivationAnimation != null)
                {
                    this.mPassivationAnimation.OnStart(() =>
                    {
                        this.mWindowState = WindowState.PASSIVATION_ANIMATION_BEGIN;
                    }).OnEnd(() =>
                    {
                        this.mWindowState = WindowState.PASSIVATION_ANIMATION_END;
                        result.SetResult();
                    }).Play();
                }
                else
                {
                    result.SetResult();
                }
            }
            catch (Exception e)
            {
                result.SetException(e);
            }

            return result;
        }


        public IAsyncResult DoShow(bool ignoreAnimation = false)
        {
            UIAsyncResult result = new UIAsyncResult();
            try
            {
                if (this._created == false)
                {
                    new System.Exception("未成功创建");
                }

                this.OnShow();
                this.mVisibility = true;
                this.mWindowState = WindowState.VISIBLE;
                if (!ignoreAnimation && this.mEnterAnimation != null)
                {
                    this.mEnterAnimation.OnStart(() => { this.mWindowState = WindowState.ENTER_ANIMATION_BEGIN; })
                        .OnEnd(() =>
                        {
                            this.mWindowState = WindowState.ENTER_ANIMATION_END;
                            result.SetResult();
                        }).Play();
                }
                else
                {
                    result.SetResult();
                }
            }
            catch (Exception e)
            {
                result.SetException(e);
                UnityEngine.Debug.LogException(e);
                //if (log.IsWarnEnabled)
                //    log.WarnFormat("The window named \"{0}\" failed to open!Error:{1}", this.Name, e);
            }

            return result;
        }

        public IAsyncResult DoHide(bool ignoreAnimation = false)
        {
            UIAsyncResult result = new UIAsyncResult();
            try
            {
                if (!ignoreAnimation && this.mExitAnimation != null)
                {
                    this.mExitAnimation.OnStart(() => { this.mWindowState = WindowState.EXIT_ANIMATION_BEGIN; }).OnEnd(
                        () =>
                        {
                            this.mWindowState = WindowState.EXIT_ANIMATION_END;
                            this.mVisibility = false;
                            this.mWindowState = WindowState.INVISIBLE;
                            this.OnHide();
                            result.SetResult();
                        }).Play();
                }
                else
                {
                    this.mVisibility = false;
                    this.mWindowState = WindowState.INVISIBLE;
                    this.OnHide();
                    result.SetResult();
                }
            }
            catch (Exception e)
            {
                result.SetException(e);
                //if (log.IsWarnEnabled)
                //    log.WarnFormat("The window named \"{0}\" failed to hide!Error:{1}", this.Name, e);
            }

            return result;
        }

        public virtual void DoDismiss()
        {
            if (this._dismissed == false)
            {
                this.mWindowState = WindowState.DISMISS_BEGIN;
                this._dismissed = true;
                this.OnDismiss();
                this.raiseOnDismissed();

                this.mUILayer.Remove(this);

                this.UnloadViewOwner();
                //if (!this.IsDestroyed() && this.gameObject != null)
                //    GameObject.Destroy(this.gameObject);

                this.mWindowState = WindowState.DISMISS_END;
                this._dismissTransition = null;
            }
        }
    }
}