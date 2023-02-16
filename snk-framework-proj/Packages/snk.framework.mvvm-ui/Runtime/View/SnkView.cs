using System;
using System.Threading.Tasks;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.ViewModel;
using SnkFramework.NuGet.Asynchronous;
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

        public virtual Task Activate(bool animated)
        {
            if(animated)
                onActivateTransitioning();
            return Task.CompletedTask;
        }

        public virtual Task Passivate(bool animated)
        {
            if(animated)
                onPassivateTransitioning();
            return Task.CompletedTask;
        }

        /// <summary>
        /// 激活动画实现
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        protected virtual Task onActivateTransitioning()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 钝化动画实现
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        protected virtual Task onPassivateTransitioning()
        {
            return Task.CompletedTask;
        }
    }
}