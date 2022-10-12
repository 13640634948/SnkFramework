using Loxodon.Framework.Binding.Contexts;
using Loxodon.Framework.Interactivity;
using SampleDevelop.Mvvm.Implments.UGUI;
using SampleDevelop.Test;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Windows.LoginWindow
{
    public interface IUGUIViewOwner : ISnkViewOwner
    {
        public CanvasGroup mCanvasGroup { get; }
        public Canvas mCanvas { get; }
    }

    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public class UGUIViewOwner : UIBehaviour, IUGUIViewOwner
    { 
        private Canvas _canvas;
        public Canvas mCanvas => this._canvas ??= this.GetComponent<Canvas>();
        
        private CanvasGroup _canvasGroup;
        public CanvasGroup mCanvasGroup => this._canvasGroup ??= this.GetComponent<CanvasGroup>();
        public virtual void Dispose()
        {
            this.onDisposeBegin();
            if (!this.IsDestroyed() && this.gameObject != null)
                GameObject.Destroy(this.gameObject);
            this.onDisposeEnd();
        }

        protected virtual void onDisposeBegin()
        {
            
        }
        protected virtual void onDisposeEnd()
        {
            
        }
    }

    public class LoginWindow : UGUIWindow<LoginViewModel>
    {
        private Text mTxt;
        public Button mButton;

        protected override void onInitialize()
        {
            this.mTxt = this.transform.Find("Text").GetComponent<Text>();
            this.mButton = this.transform.Find("Button").GetComponent<Button>();
        }

        protected override void onViewModelChanged()
        {
            var setter = this.CreateBindingSet(this.mViewModel);
            setter.Bind(this.mTxt).For(v => v.text).To(vm => vm.Tip);
            setter.Bind(this.mButton).For(v => v.onClick).To(vm => vm.mButtonCommand);
            setter.Bind().For(v => v.onInteractionFinished).To(vm => vm.mInteractionFinished);
            setter.Build();
        }

        public void onInteractionFinished(object sender, InteractionEventArgs args)
        {
            Debug.Log("onInteractionFinished");
        }

    }
}