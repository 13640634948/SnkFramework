using System.Collections;
using Loxodon.Framework.Binding.Contexts;
using SnkFramework.Mvvm.Base;
using UnityEngine;

namespace SampleDevelop.Test
{
    public abstract class SnkUIViewBase : ISnkUIView
    {
        public virtual ISnkAnimation mEnterAnimation { get; set; }
        public virtual ISnkAnimation mExitAnimation { get; set; }
        public abstract bool mInteractable { get; set; }

        public virtual ISnkViewOwner mOwner { get; private set; }
        public virtual string mName { get; set; }
        public virtual ISnkView mParentView { get; set; }
        public virtual void Create()
        {
            
        }

        private bool _visibility;

        public virtual bool mVisibility
        {
            get => this._visibility;
            set
            {
                if(this._visibility == value)
                    return;
                this._visibility = value;
                this.OnVisibilityChanged();
            }
        } 

        protected virtual void OnVisibilityChanged()
        {
        }

        protected virtual void OnOwnerLoaded()
        {
        }

        public IBindingContext DataContext { get; set; }
        protected readonly string UI_PREFAB_PATH_FORMAT = "UI/Prefabs/{0}";


        public LoadState mLoadState { get; private set; } = LoadState.none;
        public string assetPath => string.Format(UI_PREFAB_PATH_FORMAT, this.GetType().Name);
        public void Load()
        {
            this.mLoadState = LoadState.loading;
            this.mOwner = SnkMvvmSetup.mLoader.LoadViewOwner(assetPath);
            this.mLoadState = LoadState.loaded;
            this.OnOwnerLoaded();
        }

        public IEnumerator LoadAsync()
        {
            this.mLoadState = LoadState.loading;
            yield return SnkMvvmSetup.mLoader.LoadViewOwnerAsync(assetPath, owner=>this.mOwner = owner);
            this.mLoadState = LoadState.loaded;
        }
    }

}