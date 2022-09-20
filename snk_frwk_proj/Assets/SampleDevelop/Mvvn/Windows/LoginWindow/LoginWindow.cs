using Loxodon.Framework.Binding.Builder;
using SnkFramework.Mvvm.Base;
using SnkFramework.Mvvm.View;
using UnityEngine;
using UnityEngine.UI;

namespace Windows.LoginWindow
{
    public class LoginWindow : Window<LoginViewModel>
    {
        private Text mTxt;

        protected override void onInitComponents()
        {
            this.mTxt = this.mOwner.transform.Find("Text").GetComponent<Text>();
        }

        protected override void onBindingComponents(BindingSet<View<LoginViewModel>, LoginViewModel> setter)
        {
            setter.Bind(this.mTxt).For(v => v.text).To(vm => vm.Tip);
        }

        protected override void onCreate(IBundle bundle)
        { 
        }

        protected override void onShow()
        {
            base.onShow();
            this.mName = "[LoginWindow]" + Time.frameCount.ToString();
        }
    }
}