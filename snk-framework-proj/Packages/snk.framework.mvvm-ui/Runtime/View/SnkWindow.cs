using System;
using System.Collections;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Layer;
using UnityEngine;

namespace SnkFramework.Mvvm.Runtime
{
    namespace View
    {
        [RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
        public abstract partial class SnkWindow : SnkView, ISnkWindow
        {
            public static readonly bool STATE_BROADCAST = false;

            private Canvas _canvas;
            public Canvas Canvas => _canvas ??= GetComponent<Canvas>();

            private CanvasGroup _canvasGroup;
            public CanvasGroup CanvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();

            public virtual bool Interactable
            {
                get
                {
                    if (this.IsDestroyed() || this.gameObject == null)
                        return false;

                    //if (GlobalSetting.useBlocksRaycastsInsteadOfInteractable)
                    return this.CanvasGroup.blocksRaycasts;
                    //return this.CanvasGroup.interactable;
                }
                set
                {
                    if (this.IsDestroyed() || this.gameObject == null)
                        return;

                    //if (GlobalSetting.useBlocksRaycastsInsteadOfInteractable)
                    this.CanvasGroup.blocksRaycasts = value;
                    //else
                    //    this.CanvasGroup.interactable = value;
                }
            }

            private EventHandler _windowStateChanged;

            private bool _created;

            private bool _dismissed;

            private SnkTransitionOperation _dismissTransitionOperation;

            private WIN_STATE state = WIN_STATE.none;

            public WIN_STATE WindowState
            {
                get => this.state;
                set
                {
                    if (this.state.Equals(value))
                        return;

                    WIN_STATE old = this.state;
                    this.state = value;
                    this.RaiseStateChanged(old, this.state);
                }
            }

            public event EventHandler WindowStateChanged
            {
                add
                {
                    lock (_lock)
                    {
                        this._windowStateChanged += value;
                    }
                }
                remove
                {
                    lock (_lock)
                    {
                        this._windowStateChanged -= value;
                    }
                }
            }

            public void Create(ISnkLayer layer, ISnkBundle bundle = null)
            {
                //if (this.dismissTransition != null || this.dismissed)
                if (this._dismissed)
                    throw new ObjectDisposedException(this.gameObject.name);

                if (this._created)
                    return;

                this.WindowState = WIN_STATE.create_begin;
                this.Layer = layer;
                this.Layer.AddChild(this);

                this.Visibility = false;
                this.Interactable = this.Activated;
                this.onCreate(bundle);
                this._created = true;
                this.WindowState = WIN_STATE.create_end;
            }

            protected virtual void onCreate(ISnkBundle bundle = null) { }
            protected virtual void onShown() { }
            protected virtual void onHiden() { }
            protected virtual void onDismiss() { }

            protected override void OnActivatedChanged()
            {
                base.OnActivatedChanged();
                this.Interactable = this.Activated;
            }

            protected void RaiseStateChanged(WIN_STATE oldState, WIN_STATE newState)
            {
                try
                {
                    WindowStateEventArgs eventArgs = new WindowStateEventArgs(this, oldState, newState);
                    _windowStateChanged?.Invoke(this, eventArgs);
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e);
                }
            }

            public virtual SnkTransitionOperation Show(bool animated)
            {
                onShown();
                this.Visibility = true;
                this.WindowState = WIN_STATE.visible;
                SnkTransitionOperation operation = new SnkTransitionOperation();
                var routine = onShowTransitioning(operation);
                if (routine != null && animated)
                {
                    this.WindowState = WIN_STATE.show_anim_begin;
                    operation.onCompleted += () => this.WindowState = WIN_STATE.show_anim_end;
                    StartCoroutine(routine);
                }
                return operation;
            }

            public virtual SnkTransitionOperation Hide(bool animated)
            {
                SnkTransitionOperation operation = new SnkTransitionOperation();
                var routine = onHideTransitioning(operation);
                if (routine != null && animated)
                {
                    this.WindowState = WIN_STATE.hide_anim_begin;
                    operation.onCompleted += () =>
                    {
                        this.WindowState = WIN_STATE.hide_anim_end;
                        this.Visibility = false;
                        this.WindowState = WIN_STATE.invisible;
                        this.onHiden();
                    };
                    StartCoroutine(routine);
                }
                else
                {
                    this.Visibility = false;
                    this.WindowState = WIN_STATE.invisible;
                    this.onHiden();
                }
                return operation;
            }

            public virtual SnkTransitionOperation Dismiss(bool animated)
            {
                if (_dismissTransitionOperation != null)
                    return _dismissTransitionOperation;
                
                if(this._dismissed == true)
                    throw new InvalidOperationException(string.Format("The window[{0}] has been destroyed.", this.gameObject.name));
                
                this._dismissed = true;
                _dismissTransitionOperation = new SnkTransitionOperation();
                var routine = onDismissTransitioning(_dismissTransitionOperation);
                if (routine != null && animated)
                {
                    this.WindowState = WIN_STATE.destroy_begin;
                    _dismissTransitionOperation.onCompleted += () =>
                    {
                        this.WindowState = WIN_STATE.destroy_end;
                        this.onDismiss();
                        this.Layer.RemoveChild(this);
                        this._dismissTransitionOperation = null;
                    };
                    StartCoroutine(routine);
                }
                else
                {
                    this.Layer.RemoveChild(this);
                    this._dismissTransitionOperation = null;
                    this.onDismiss();
                }
                return _dismissTransitionOperation;
                
            }

            /// <summary>
            /// 显示动画实现
            /// </summary>
            /// <param name="operation"></param>
            /// <returns></returns>
            protected virtual IEnumerator onShowTransitioning(SnkTransitionOperation operation)
            {
                operation.IsDone = true;
                yield break;
            }

            /// <summary>
            /// 隐藏动画实现
            /// </summary>
            /// <param name="operation"></param>
            /// <returns></returns>
            protected virtual IEnumerator onHideTransitioning(SnkTransitionOperation operation)
            {
                operation.IsDone = true;
                yield break;
            }
            
            /// <summary>
            /// 销毁动画实现
            /// </summary>
            /// <param name="operation"></param>
            /// <returns></returns>
            protected virtual IEnumerator onDismissTransitioning(SnkTransitionOperation operation) 
                => onHideTransitioning(operation);

            public override SnkTransitionOperation Activate(bool animated)
            {
                if (this.Visibility == false)
                    throw new InvalidOperationException("The window is not visible.");

                if (this.Activated == true)
                    return null;
                
                SnkTransitionOperation operation = new SnkTransitionOperation();
                var routine = onActivateTransitioning(operation);
                if (routine != null && animated)
                {
                    this.WindowState = WIN_STATE.activation_anim_begin;
                    operation.onCompleted += () =>
                    {
                        this.WindowState = WIN_STATE.activation_anim_end;
                        this.Activated = true;
                        this.WindowState = WIN_STATE.activation;
                    };
                    StartCoroutine(routine);
                }
                else
                {
                    this.Activated = true;
                    this.WindowState = WIN_STATE.activation;
                }
                return operation;
            }

            public override SnkTransitionOperation Passivate(bool animated)
            {
                if (this.Visibility == false)
                    throw new InvalidOperationException("The window is not visible.");

                if (this.Activated == false)
                    return null;

                this.Activated = false;
                this.WindowState = WIN_STATE.passivation;

                SnkTransitionOperation operation = new SnkTransitionOperation();
                var routine = onPassivateTransitioning(operation);
                if (routine != null && animated)
                {
                    this.WindowState = WIN_STATE.passivation_anim_begin;
                    operation.onCompleted += () =>
                    {
                        this.WindowState = WIN_STATE.passivation_anim_end;
                    };
                    StartCoroutine(routine);
                }
                return operation;
            }
            
        }
    }
}