using Windows.LoginWindow;
using SampleDevelop.Test;
using SnkFramework.Mvvm.ViewModel;
using UnityEngine;

namespace SampleDevelop.Mvvm.Implments.UGUI
{

    public interface IUGUIView : ISnkUIView
    {
        public UGUIViewOwner mUGUIOwner { get; }
    }

    public interface IUGUIPageBase : ISnkUIPage, IUGUIView
    {
        
    }

    public interface IUGUIWindowView : ISnkWindowView, IUGUIPageBase
    {
    }

    public interface IUGUIWindow : ISnkWindow, IUGUIWindowView
    {
        
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

        private RectTransform _rectTransform;
        protected RectTransform rectTransform => _rectTransform ??= transform as RectTransform;

        public override bool mInteractable
        {
            get
            {
                if (this.gameObject == null)
                    return false;
                //return this.mUGUIOwner.mCanvasGroup.interactable;
                return this.mUGUIOwner.mCanvasGroup.blocksRaycasts;
            }
            set
            {
                if (this.gameObject == null)
                    return;
                //this.mUGUIOwner.mCanvasGroup.interactable = value;
                this.mUGUIOwner.mCanvasGroup.blocksRaycasts = value;
            }
        }

        protected override void OnCreate()
        {
            Debug.Log("OnCreate");
            this.mViewModel = new TViewModel();
            this.onInitComponents();
            this.onBindingComponents();
        }

        protected override void OnOwnerLoadEnd()
        {
            base.OnOwnerLoadEnd();
            this.mUGUILayer.AddChild(rectTransform);
        }

        protected override void OnOwnerUnloadEnd()
        {
            base.OnOwnerUnloadEnd();
            this.mUGUILayer.RemoveChild(rectTransform);
        }

        protected virtual void onInitComponents()
        {
        }

        protected virtual void onBindingComponents()
        {
        }
    }
}