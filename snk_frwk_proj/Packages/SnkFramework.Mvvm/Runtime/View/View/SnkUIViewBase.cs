using System.Collections;
using Loxodon.Framework.Binding.Contexts;
using SnkFramework.Mvvm.Base;

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

        protected virtual void OnOwnerLoadBegin()
        {
        }
        protected virtual void OnOwnerLoadEnd()
        {
        }
        protected virtual void OnOwnerUnloadBegin()
        {
        }
        protected virtual void OnOwnerUnloadEnd()
        {
        }

        public IBindingContext DataContext { get; set; }
        protected readonly string UI_PREFAB_PATH_FORMAT = "UI/Prefabs/{0}";

        public LoadState mLoadState { get; private set; } = LoadState.none;
        public string assetPath => string.Format(UI_PREFAB_PATH_FORMAT, this.GetType().Name);
        public void Load()
        {
            this.OnOwnerLoadBegin();
            this.mLoadState = LoadState.load_begin;
            this.mOwner = SnkMvvmSetup.mLoader.LoadViewOwner(assetPath);
            this.mLoadState = LoadState.load_end;
            this.OnOwnerLoadEnd();
        }

        public IEnumerator LoadAsync()
        {
            this.OnOwnerLoadBegin();
            this.mLoadState = LoadState.load_begin;
            yield return SnkMvvmSetup.mLoader.LoadViewOwnerAsync(assetPath, owner=>this.mOwner = owner);
            this.mLoadState = LoadState.load_end;
            this.OnOwnerLoadEnd();
        }

        public virtual void Unload()
        {
            this.OnOwnerUnloadBegin();
            this.mLoadState = LoadState.load_begin;
            if (this.mOwner != null)
            {
                this.mOwner.Dispose();
                this.mOwner = null;
            }
            this.mLoadState = LoadState.load_end;
            this.OnOwnerUnloadEnd();
        }
    }

}