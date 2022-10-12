using System.Collections;
using Windows.LoginWindow;
using SampleDevelop.Test;
using SnkFramework.Mvvm.ViewModel;
using UnityEngine;

namespace SampleDevelop.Mvvm.Implments.UGUI
{
    public interface IUGUIView : ISnkUIView
    {
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
        where TViewModel : class, ISnkViewModel, new()
    {
        protected readonly string UI_PREFAB_PATH_FORMAT = "UI/Prefabs/{0}";
        public override string mAssetPath => string.Format(UI_PREFAB_PATH_FORMAT, this.GetType().Name);
        
        public override string mName
        {
            get => this.mOwner.name;
            set => base.mName = value;
        }
        public TViewModel mViewModel { get; set; }

        public new UGUIViewOwner mOwner => base.mOwner as UGUIViewOwner;
        public UGUILayer mUGUILayer => this.mUILayer as UGUILayer;
        
        private GameObject _gameObject;
        protected GameObject gameObject => _gameObject ??= this.mOwner.gameObject;

        private Transform _transform;
        protected Transform transform => _transform ??= this.mOwner.transform;

        private RectTransform _rectTransform;
        protected RectTransform rectTransform => _rectTransform ??= transform as RectTransform;

        public override bool mInteractable
        {
            get
            {
                if (this.gameObject == null)
                    return false;
                //return this.mUGUIOwner.mCanvasGroup.interactable;
                return this.mOwner.mCanvasGroup.blocksRaycasts;
            }
            set
            {
                if (this.gameObject == null)
                    return;
                //this.mUGUIOwner.mCanvasGroup.interactable = value;
                this.mOwner.mCanvasGroup.blocksRaycasts = value;
            }
        }

        protected override void OnCreate()
        {
            Debug.Log("OnCreate");
            this.mViewModel = new TViewModel();
            this.onInitComponents();
            this.onBindingComponents();
        }

        public override void LoadViewOwner()
        {
            base.LoadViewOwner();
            this.mUGUILayer.AddChild(rectTransform);
        }

        public override IEnumerator LoadViewOwnerAsync()
        {
            yield return base.LoadViewOwnerAsync();
            this.mUGUILayer.AddChild(rectTransform);
        }

        public override void UnloadViewOwner()
        {
            base.UnloadViewOwner();
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