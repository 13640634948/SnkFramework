using Loxodon.Framework.Binding.Contexts;
using Loxodon.Framework.Interactivity;
using SampleDevelop.Mvvm.Implments.UGUI;
using SampleDevelop.Test;
using UnityEngine;
using UnityEngine.UI;

namespace Windows.LoginWindow
{
    public interface IUGUIViewOwner : ISnkViewOwner
    {
        public CanvasGroup mCanvasGroup { get; }
    }

    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public class UGUIViewOwner : MonoBehaviour, IUGUIViewOwner
    {
        private CanvasGroup _canvasGroup;
        public CanvasGroup mCanvasGroup => this._canvasGroup ??= this.GetComponent<CanvasGroup>();
    }

    public class LoginWindow : UGUIWindow<LoginViewModel>
    {
        private Text mTxt;
        public Button mButton;

        protected override void onInitComponents()
        {
            this.mTxt = this.transform.Find("Text").GetComponent<Text>();
            this.mButton = this.transform.Find("Button").GetComponent<Button>();
        }

        protected override void onBindingComponents()
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