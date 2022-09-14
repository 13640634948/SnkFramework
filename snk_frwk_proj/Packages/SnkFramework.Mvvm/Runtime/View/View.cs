using System;
using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.Binding.Contexts;
using SnkFramework.Mvvm.Base;
using SnkFramework.Mvvm.ViewModel;
using UnityEngine;

namespace SnkFramework.Mvvm.View
{
    public abstract class View<TViewModel> : IView<TViewModel>, IBindingContextOwner
        where TViewModel : class, IViewModel, new()
    {
        private TViewModel _viewModel;

        private UIAnimation _enterAnimation;
        private UIAnimation _exitAnimation;
        private CanvasGroup _canvasGroup;
        [NonSerialized] private UIAttribute[] _attributes;
        private EventHandler _visibilityChanged;

        IViewModel IView.mViewModel => _viewModel;
        public TViewModel mViewModel => _viewModel;


        public GameObject mOwner { get; private set; }

        public IBindingContext DataContext { get; set; }

        public UIAttribute[] mUIAttributes => _attributes;
        private readonly object _lock = new();

        public virtual string mName
        {
            get => this.ownerValidityCheck() ? this.mOwner.gameObject.name : null;
            set
            {
                if (this.ownerValidityCheck() == false)
                    return;
                if (string.Equals(this.mOwner.gameObject.name, value))
                    return;
                this.mOwner.gameObject.name = value;
            }
        }

        public bool mVisibility
        {
            get => this.ownerValidityCheck() ? this.mOwner.gameObject.activeSelf : false;
            set
            {
                if (this.ownerValidityCheck() == false)
                    return;
                if (this.mOwner.gameObject.activeSelf == value)
                    return;
                this.mOwner.gameObject.SetActive(value);

                if (value)
                {
                    this.onVisibilityChanged(true);
                    this.raiseVisibilityChanged();
                }
                else
                {
                    this.raiseVisibilityChanged();
                    this.onVisibilityChanged(false);
                }
            }
        }

        public event EventHandler VisibilityChanged
        {
            add
            {
                lock (_lock)
                {
                    this._visibilityChanged += value;
                }
            }
            remove
            {
                lock (_lock)
                {
                    this._visibilityChanged -= value;
                }
            }
        }

        protected void raiseVisibilityChanged()
        {
            try
            {
                if (this._visibilityChanged != null)
                    this._visibilityChanged(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                throw;
                //if (log.IsWarnEnabled)
                //    log.WarnFormat("{0}", e);
            }
        }

        public bool mInteractable
        {
            get
            {
                if (this.ownerValidityCheck() == false)
                    return false;

                if (SnkMvvmSetting.useBlocksRaycastsInsteadOfInteractable)
                    return this.mCanvasGroup.blocksRaycasts;
                return this.mCanvasGroup.interactable;
            }
            set
            {
                if (this.ownerValidityCheck() == false)
                    return;

                if (SnkMvvmSetting.useBlocksRaycastsInsteadOfInteractable)
                    this.mCanvasGroup.blocksRaycasts = value;
                else
                    this.mCanvasGroup.interactable = value;
            }
        }

        public UIAnimation mEnterAnimation
        {
            get => this._enterAnimation;
            set => this._enterAnimation = value;
        }

        public UIAnimation mExitAnimation
        {
            get => this._exitAnimation;
            set => this._exitAnimation = value;
        }

        public CanvasGroup mCanvasGroup
        {
            get
            {
                if (this.ownerValidityCheck() == false)
                    return null;
                return this._canvasGroup ??= this.mOwner.GetComponent<CanvasGroup>();
            }
        }

        public float Alpha
        {
            get => this.ownerValidityCheck() ? this.mCanvasGroup.alpha : 0.0f;
            set
            {
                if (this.ownerValidityCheck() == false)
                    return;
                if (Math.Abs(this.mCanvasGroup.alpha - value) <= 0)
                    return;
                this.mCanvasGroup.alpha = value;
            }
        }

        protected bool ownerValidityCheck() => this.viewValidityCheck(this);

        protected bool viewValidityCheck(IView view)
            => this.mOwner != null && view.mOwner.gameObject != null;

        public void SetOwner(GameObject owner)
        {
            this.mOwner = owner;
            this.onInitComponents();
            this._viewModel = new TViewModel();
            var setter = this.CreateBindingSet(this._viewModel);
            this.onBindingComponents(setter);
            setter.Build();
        }

        protected abstract void onInitComponents();

        protected abstract void onBindingComponents(BindingSet<View<TViewModel>, TViewModel> setter);

        protected virtual void onVisibilityChanged(bool visibility)
        {
        }
    }
}