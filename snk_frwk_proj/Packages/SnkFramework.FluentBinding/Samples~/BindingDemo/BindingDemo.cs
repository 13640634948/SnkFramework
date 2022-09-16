using Loxodon.Framework.Binding;
using SnkFramework.FluentBinding.Base;
using UnityEngine;
using UnityEngine.UI;

namespace SnkFramework.FluentBinding.Sample
{
    public class BindingTest : MonoBehaviour
    {
        private TestViewModel _viewModel;

        public Text mTxt;
        public string mInputString;

        void Awake()
        {
            SnkBindingSetup.Initialize();
        }

        void Start()
        {
            _viewModel = new TestViewModel();
            var setter = this.CreateBindingSet(_viewModel);
            setter.Bind(this.mTxt).For(v => v.text).To(vm => vm.Tip);
            setter.Build();
        }

        private void Update()
        {
            _viewModel.Tip = mInputString;
        }
    }
}