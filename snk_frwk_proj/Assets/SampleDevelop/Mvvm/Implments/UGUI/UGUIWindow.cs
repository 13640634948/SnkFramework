using System.Collections;
using System.Collections.Generic;
using Windows.LoginWindow;
using SampleDevelop.Test;
using SnkFramework.Mvvm.ViewModel;
using UnityEngine;

namespace SampleDevelop.Mvvm.Implments.UGUI
{
    public interface IUGUIWindow : ISnkWindow
    {
        public UGUIViewOwner mUGUIOwner { get; }
    }

    public abstract class UGUIWindow<TViewModel> : SnkWindowBase, IUGUIWindow
        where TViewModel : class, IViewModel, new()
    {
        public override string mName
        {
            get => this.mUGUIOwner.name;
            set => base.mName = value;
        }
        public TViewModel mViewModel { get; set; }

        public UGUIViewOwner mUGUIOwner => this.mOwner as UGUIViewOwner;
        public UGUILayer mUGUILayer => this.mUILayer as UGUILayer;
        
        private GameObject _gameObject;
        protected GameObject gameObject => _gameObject ??= this.mUGUIOwner.gameObject;

        private Transform _transform;
        protected Transform transform => _transform ??= this.mUGUIOwner.transform;

        public override bool mInteractable
        {
            get
            {
                if (this.gameObject == null)
                    return false;
                return this.mUGUIOwner.mCanvasGroup.interactable;
            }
            set
            {
                if (this.gameObject == null)
                    return;
                this.mUGUIOwner.mCanvasGroup.interactable = value;
            }
        }

        protected override void OnCreate()
        {
            Debug.Log("OnCreate");
            this.mViewModel = new TViewModel();
            this.onInitComponents();
            this.onBindingComponents();
        }

        protected override void OnOwnerLoaded()
        {
            base.OnOwnerLoaded();
            this.mUGUILayer.AddChild(this.transform);
        }

        protected virtual void onInitComponents()
        {
        }

        protected virtual void onBindingComponents()
        {
        }
        
    }


    /*
     public abstract class UGUIWindow<TViewModel> : Window
        where TViewModel : class, IViewModel, new()
    {
        public TViewModel mViewModel { get; set; }

        public override void SetOwner(GameObject owner)
        {
            this.mViewModel = new TViewModel();
            base.SetOwner(owner);
        }
    }
    */
}