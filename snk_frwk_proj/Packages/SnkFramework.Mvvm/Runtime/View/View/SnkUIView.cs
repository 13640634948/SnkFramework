using System.Collections;
using Loxodon.Framework.Binding.Contexts;
using SnkFramework.Mvvm.Base;
using SnkFramework.Mvvm.ViewModel;

namespace SampleDevelop.Test
{
    public abstract class SnkUIView : ISnkUIView
    {
        public virtual ISnkAnimation mEnterAnimation { get; set; }
        public virtual ISnkAnimation mExitAnimation { get; set; }
        public abstract bool mInteractable { get; set; }

        public ISnkViewOwner mOwner { get; private set; }


        private ISnkViewModel _viewModel;
        public virtual ISnkViewModel mViewModel
        {
            get => this._viewModel;
            protected set
            {
                if(this._viewModel == value)
                    return;
                this._viewModel = value;
                this.onViewModelChanged();
            }
        }

        protected virtual void onViewModelChanged()
        {
        }


        public virtual string mName { get; set; }
        public virtual ISnkView mParentView { get; set; }

        public abstract void Create();

        public bool mViewOwnerLoaded { get; protected set; }

        private bool _visibility;

        public virtual bool mVisibility
        {
            get => this._visibility;
            set
            {
                if (this._visibility == value)
                    return;
                this._visibility = value;
                this.OnVisibilityChanged();
            }
        }

        protected virtual void OnVisibilityChanged()
        {
        }


        public IBindingContext DataContext { get; set; }

        public abstract string mAssetPath { get; }

        public virtual void LoadViewOwner()
        {
            this.mOwner = SnkMvvmSetup.mLoader.LoadViewOwner(mAssetPath);
            this.mViewOwnerLoaded = true;
        }

        public virtual IEnumerator LoadViewOwnerAsync()
        {
            yield return SnkMvvmSetup.mLoader.LoadViewOwnerAsync(mAssetPath, owner => this.mOwner = owner);
            this.mViewOwnerLoaded = true;
        }

        public virtual void UnloadViewOwner()
        {
            if (this.mOwner != null)
            {
                this.mOwner.Dispose();
                this.mOwner = null;
            }

            this.mViewOwnerLoaded = false;
        }
    }
}