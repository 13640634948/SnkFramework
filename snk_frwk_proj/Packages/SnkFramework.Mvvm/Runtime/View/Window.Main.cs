using System;
using SnkFramework.Mvvm.Base;
using SnkFramework.Mvvm.Log;
using UnityEngine;
using UnityEngine.UI;

namespace SnkFramework.Mvvm.View
{
    public abstract partial class Window : WindowView, ISnkWindowControllable
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

        private WIN_STATE _state = WIN_STATE.none;
        private WIN_TYPE _winType;

        private IUILayer _uiLayer;
        private ITransition _dismissTransition;

        public bool Created => this._created;
        public bool Dismissed => this._dismissed;
        public bool StateBroadcast => this._stateBroadcast;

        public event EventHandler ActivatedChanged
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

        public event EventHandler OnDismissed
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

        private int _sortingOrder;

        public int mSortingOrder
        {
            get => this._sortingOrder;
            set => this._sortingOrder = value;
        }

        public IUILayer UILayer
        {
            get => this._uiLayer;
            set => this._uiLayer = value;
        }

        public WIN_TYPE WinType
        {
            get => this._winType;
            set => this._winType = value;
        }

        public int WindowPriority
        {
            get => this._windowPriority;
            set => this._windowPriority = value;
        }

        protected WIN_STATE State
        {
            get { return this._state; }
            set
            {
                if (this._state.Equals(value))
                    return;

                WIN_STATE old = this._state;
                this._state = value;
                this.raiseStateChanged(old, this._state);
            }
        }


        protected void raiseStateChanged(WIN_STATE oldState, WIN_STATE newState)
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

        public bool mActivated
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


        public IWindow Create(IBundle bundle = null)
        {
            if (this._dismissTransition != null || this._dismissed)
                throw new ObjectDisposedException(this.mName);

            if (this._created)
                return this;

            if (this.mOwner.TryGetComponent<Canvas>(out var canvas) == false)
                canvas = mOwner.AddComponent<Canvas>();

            if (this.mOwner.TryGetComponent<GraphicRaycaster>(out var graphicRaycaster) == false)
                mOwner.AddComponent<GraphicRaycaster>();

            this.State = WIN_STATE.create_begin;

            this.UILayer.Add(this);
            canvas.overrideSorting = true;
            canvas.sortingOrder = this.mSortingOrder;
            canvas.sortingLayerID = this.UILayer.GetSortingLayerID();
            this.mName = "[" + this.mSortingOrder + "]" + this.mName;

            this.mVisibility = false;
            this.mInteractable = this.mActivated;

            this.onCreate(bundle);

            this._created = true;
            this.State = WIN_STATE.create_end;


            return this;
        }


        public ITransition Show(bool ignoreAnimation = false)
        {
            if (this._dismissTransition != null || this._dismissed)
                throw new InvalidOperationException("The window has been destroyed");

            if (this.mVisibility)
                throw new InvalidOperationException("The window is already visible.");

            return this.UILayer.Show(this).DisableAnimation(ignoreAnimation);
        }

        public ITransition Hide(bool ignoreAnimation = false)
        {
            if (!this._created)
                throw new InvalidOperationException("The window has not been created.");

            if (this._dismissed)
                throw new InvalidOperationException("The window has been destroyed.");

            if (!this.mVisibility)
                throw new InvalidOperationException("The window is not visible.");

            return this.UILayer.Hide(this).DisableAnimation(ignoreAnimation);
        }


        public ITransition Dismiss(bool ignoreAnimation = false)
        {
            if (this._dismissTransition != null)
                return this._dismissTransition;

            if (this._dismissed)
                throw new InvalidOperationException(string.Format("The window[{0}] has been destroyed.", this.mName));

            this._dismissTransition = this.UILayer.Dismiss(this).DisableAnimation(ignoreAnimation);
            return this._dismissTransition;
        }


        protected virtual void onActivatedChanged()
        {
            this.mInteractable = this.mActivated;
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