using BFFramework.Runtime.UserInterface;
using GAME.Contents.UserInterfaces.ViewModels;
using Loxodon.Framework.Binding;
using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.Binding.Contexts;
using SnkFramework.Mvvm.Runtime.Base;
using UnityEngine;
using UnityEngine.UI;

namespace GAME.Contents.UserInterfaces
{
    namespace ViewModels
    {
        public class PatchViewModel : BFViewModel
        {
            private float progress;
            private string tip;
            private bool enable;

            public bool Enable {
                get{ return this.enable; }
                set{ this.Set<bool> (ref this.enable, value); }
            }

            public float Progress {
                get{ return this.progress; }
                set{ this.Set<float> (ref this.progress, value); }
            }

            public string Tip {
                get{ return this.tip; }
                set{ this.Set<string> (ref this.tip, value); }
            }
            
            public override void Prepare(ISnkBundle parameterBundle)
            {
                Debug.Log("PatchViewModel-Prepare");
            }
        }
    }

    namespace Views
    {
        public class PatchWindow : BFWindow
        {
            public string xxoo = "";
            
            public Slider downloadSlider;
            public Text progressText;
            public Text tipText;

            private PatchViewModel _viewModel;
            protected override void onCreate(ISnkBundle bundle = null)
            {
                base.onCreate(bundle);
                _viewModel = new PatchViewModel();
                var bindingSet = this.CreateBindingSet(_viewModel);
                bindingSet.Bind(this.tipText).For(v => v.text).To(vm => vm.Tip);
                bindingSet.Build();
            }

            private void Update()
            {
                _viewModel.Tip = xxoo;
            }
        }
    }
}