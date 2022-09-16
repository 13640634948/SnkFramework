using System;
using SnkFramework.Mvvm.Base;
using SnkFramework.Mvvm.Log;
using SnkFramework.Mvvm.ViewModel;

public interface IMvvmPromise
{
}

namespace SnkFramework.Mvvm.View
{
    public abstract partial class Window<TViewModel> : WindowView<TViewModel>, IWindow<TViewModel>
        where TViewModel : class, IViewModel, new()
    {
        static private IMvvmLog log = SnkMvvmSetup.mMvvmLog;

        private readonly object _lock = new();
        private EventHandler _activatedChanged;
        private EventHandler _onDismissed;
        private EventHandler<WindowStateEventArgs> _stateChanged;

        private bool _created = false;
        private bool _dismissed = false;
        private bool _activated = false;
        private bool _stateBroadcast = true;

        private int _windowPriority;

        private WindowState _state = WindowState.NONE;
        private WindowType _windowType;

        private IUILayer _uiLayer;
        private ITransition _dismissTransition;

        public bool Created => this._created;
        public bool Dismissed => this._dismissed;
        public bool StateBroadcast => this._stateBroadcast;
        
        public event EventHandler ActivatedChanged
        {
            add { lock (_lock) this._activatedChanged += value; }
            remove { lock (_lock) this._activatedChanged -= value; }
        }

        public event EventHandler OnDismissed
        {
            add { lock (_lock) this._onDismissed += value; }
            remove { lock (_lock) this._onDismissed -= value; }
        }

        public event EventHandler<WindowStateEventArgs> StateChanged
        {
            add { lock (_lock) this._stateChanged += value; }
            remove { lock (_lock) this._stateChanged -= value; }
        }

        public IUILayer UILayer
        {
            get => this._uiLayer;
            set => this._uiLayer = value;
        }

        public WindowType WindowType
        {
            get => this._windowType;
            set => this._windowType = value;
        }

        public int WindowPriority
        {
            get => this._windowPriority;
            set => this._windowPriority = value;
        }

        protected WindowState State
        {
            get { return this._state; }
            set
            {
                if (this._state.Equals(value))
                    return;

                WindowState old = this._state;
                this._state = value;
                this.raiseStateChanged(old, this._state);
            }
        }


        protected void raiseStateChanged(WindowState oldState, WindowState newState)
        {
            try
            {
                WindowStateEventArgs eventArgs = new WindowStateEventArgs(this, oldState, newState);
                if (SnkMvvmSetup.mSettings.mEnableWindowStateBroadcast && StateBroadcast)
                {
                    //抛出事件
                    //Messenger.Publish(eventArgs);   
                }

                this._stateChanged?.Invoke(this, eventArgs);
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("{0}", e);
            }
        }

        public bool Activated
        {
            get => this._activated;
            protected set
            {
                if (this._activated == value)
                    return;

                this._activated = value;
                this.onActivatedChanged();
                this.raiseActivatedChanged();
            }
        }


        protected void raiseActivatedChanged()
        {
            try
            {
                this._activatedChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("{0}", e);
            }
        }

        protected void raiseOnDismissed()
        {
            try
            {
                this._onDismissed?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("{0}", e);
            }
        }


        public void Create(IBundle bundle = null)
        {
            if (this._dismissTransition != null || this._dismissed)
                throw new ObjectDisposedException(this.mName);

            if (this._created)
                return;

            this.State = WindowState.CREATE_BEGIN;
            this.mVisibility = false;
            this.mInteractable = this.Activated;
            this.onCreate(bundle);
            //this.WindowManager.Add(this);
            this.UILayer.Add(this);
            this._created = true;
            this.State = WindowState.CREATE_END;
        }

        public ITransition Show(bool ignoreAnimation = false)
        {
            if (this._dismissTransition != null || this._dismissed)
                throw new InvalidOperationException("The window has been destroyed");

            if (this.mVisibility)
                throw new InvalidOperationException("The window is already visible.");

            this.UILayer.Show(this);
            return default;
            //return this.UILayer.Show(this).DisableAnimation(ignoreAnimation);
            //return this.WindowManager.Show(this).DisableAnimation(ignoreAnimation);
        }

        public ITransition Hide(bool ignoreAnimation = false)
        {
            if (!this._created)
                throw new InvalidOperationException("The window has not been created.");

            if (this._dismissed)
                throw new InvalidOperationException("The window has been destroyed.");

            if (!this.mVisibility)
                throw new InvalidOperationException("The window is not visible.");

            this._uiLayer.Hide(this);
            return default;
            //return this.WindowManager.Hide(this).DisableAnimation(ignoreAnimation);
        }

        public ITransition Dismiss(bool ignoreAnimation = false)
        {
            if (this._dismissTransition != null)
                return this._dismissTransition;

            if (this._dismissed)
                throw new InvalidOperationException(string.Format("The window[{0}] has been destroyed.", this.mName));

            this.UILayer.Dismiss(this);
            return default;
            //this.dismissTransition = this.UILayer.Dismiss(this).DisableAnimation(ignoreAnimation);
            //return this.dismissTransition;
        }


        protected virtual void onActivatedChanged()
        {
            this.mInteractable = this.Activated;
        }

        protected abstract void onCreate(IBundle bundle);

        protected virtual void onShow()
        {
        }

        protected virtual void onHide()
        {
        }

        protected virtual void onDismiss()
        {
        }
    }
}