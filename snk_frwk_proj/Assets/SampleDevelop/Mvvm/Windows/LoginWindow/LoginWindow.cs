using Loxodon.Framework.Binding.Contexts;
using Loxodon.Framework.Interactivity;
using SnkFramework.Mvvm.LayoutEngine.UGUI;
using UnityEngine;
using UnityEngine.UI;

namespace SampleDevelop.Mvvm
{
    public class LoginWindow : UGUIWindow<LoginViewModel>
    {
        private Text mTxt;
        public Button mButton;

        protected override void onInitialize()
        {
            this.mTxt = this.mTransform.Find("Text").GetComponent<Text>();
            this.mButton = this.mTransform.Find("Button").GetComponent<Button>();
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