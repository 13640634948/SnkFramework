using Loxodon.Framework.Binding.Contexts;
using SnkFramework.Mvvm.Base;
using UnityEngine;

namespace SnkFramework.Mvvm.View
{
    public abstract class View : IView, IBindingContextOwner
    {
        public virtual GameObject mOwner { get; private set; }

        public virtual string mName { get; }
        public virtual bool mActivated { get; }
        public virtual bool mInteractable { get; set; }
        public virtual UIAnimation mEnterAnimation { get; set; }
        public virtual UIAnimation mExitAnimation { get; set; }
        public virtual UIAttribute[] mUIAttributes { get; set; }
        
        public virtual void SetOwner(GameObject owner)
        {
            this.mOwner = owner;
        }

        public virtual void InitComponents()
        {
        }

        public IBindingContext DataContext { get; set; }
    }
}