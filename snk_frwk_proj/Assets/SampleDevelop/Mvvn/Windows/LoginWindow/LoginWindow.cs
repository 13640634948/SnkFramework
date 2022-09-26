using Loxodon.Framework.Binding.Builder;
using SnkFramework.Mvvm.Base;
using SnkFramework.Mvvm.View;
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

        protected override void onBindingComponents(BindingSet<View<LoginViewModel>, LoginViewModel> setter)
        {
            setter.Bind(this.mTxt).For(v => v.text).To(vm => vm.Tip);
            setter.Bind(this.mButton).For(v => v.onClick).To(vm => vm.mButtonCommand);
        }

        protected override void onCreate(IBundle bundle)
        { 
        }
    }
}