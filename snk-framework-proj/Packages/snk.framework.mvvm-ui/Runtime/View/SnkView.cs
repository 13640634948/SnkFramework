using System;
using System.Collections;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.ViewModel;
using UnityEngine;

namespace SnkFramework.Mvvm.Runtime.View
{
    public abstract class SnkView : SnkUIBehaviour, ISnkView
    {
        public virtual ISnkViewModel ViewModel { get; set; }
        protected readonly object _lock = new ();
        private bool _activated;

        private EventHandler _activatedChanged;
        private EventHandler _visibilityChanged;

        public virtual bool Visibility
        {
            get => !this.IsDestroyed() && this.gameObject != null && this.gameObject.activeSelf;
            set
            {
                if (this.IsDestroyed() || this.gameObject == null)
                    return;

                if (this.gameObject.activeSelf == value)
                    return;

                this.gameObject.SetActive(value);
            }
        }

        public event EventHandler ActivatedChanged
        {
            add { lock (_lock) this._activatedChanged += value; }
            remove { lock (_lock) this._activatedChanged -= value; }
        }

        public event EventHandler VisibilityChanged
        { 
            add { lock (_lock) this._visibilityChanged += value; }
            remove { lock (_lock) this._visibilityChanged -= value; }
        }

        public virtual bool Activated
        {
            get => this._activated;
            protected set
            {
                if (this._activated == value)
                    return;

                this._activated = value;
                this.OnActivatedChanged();
                this.RaiseActivatedChanged();
            }
        }

        protected virtual void OnActivatedChanged()
        {
        }

        protected virtual void OnVisibilityChanged()
        {
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.RaiseVisibilityChanged();
        }

        protected override void OnDisable()
        {
            this.RaiseVisibilityChanged();
            base.OnDisable();
        }

        protected void RaiseVisibilityChanged()
        {
            try
            {
                if (this._visibilityChanged != null)
                    this._visibilityChanged(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        protected void RaiseActivatedChanged()
        {
            try
            {
                this._activatedChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        public virtual SnkTransitionOperation Activate(bool animated)
        {
            SnkTransitionOperation operation = new SnkTransitionOperation();
            var routine = onActivateTransitioning(operation);
            if (routine != null && animated)
            {
                StartCoroutine(routine);
            }
            return operation;
        }

        public virtual SnkTransitionOperation Passivate(bool animated)
        {
            SnkTransitionOperation operation = new SnkTransitionOperation();
            var routine = onPassivateTransitioning(operation);
            if (routine != null && animated)
                StartCoroutine(routine);
            return operation;
        }

        /// <summary>
        /// 激活动画实现
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        protected virtual IEnumerator onActivateTransitioning(SnkTransitionOperation operation)
        {
            operation.IsDone = true;
            yield break;
        }

        /// <summary>
        /// 钝化动画实现
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        protected virtual IEnumerator onPassivateTransitioning(SnkTransitionOperation operation)
        {
            operation.IsDone = true;
            yield break;
        }
    }
}