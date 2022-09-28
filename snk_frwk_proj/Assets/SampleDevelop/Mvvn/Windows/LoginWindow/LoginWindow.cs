using Loxodon.Framework.Binding.Contexts;
using Loxodon.Framework.Interactivity;
using SnkFramework.Mvvm.Base;
using SnkFramework.Mvvm.View;
using UnityEngine;
using UnityEngine.UI;

namespace Windows.LoginWindow
{
    public class LoginWindow : Window<LoginViewModel>
    {
        private Text mTxt;
        public Button mButton;

        protected override void onInitComponents()
        {
            this.mTxt = this.mOwner.transform.Find("Text").GetComponent<Text>();
            this.mButton = this.mOwner.transform.Find("Button").GetComponent<Button>();
        }

        protected override void onBindingComponents()
        {
            var setter = this.CreateBindingSet(this.mViewModel);
            setter.Bind(this.mTxt).For(v => v.text).To(vm => vm.Tip);
            setter.Bind(this.mButton).For(v => v.onClick).To(vm => vm.mButtonCommand);
            setter.Bind().For(v => v.onInteractionFinished).To(vm => vm.mInteractionFinished);
            setter.Build();
        }

        protected override void onCreate(IBundle bundle)
        {
        }


        public void onInteractionFinished(object sender, InteractionEventArgs args)
        {
            Debug.Log("onInteractionFinished");
        }
    }
}