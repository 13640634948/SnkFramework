using BFFramework.Runtime.UserInterface;
using GAME.Contents.UserInterfaces.ViewModels;
using Loxodon.Framework.Binding;
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
                get => this.enable;
                set => this.Set<bool> (ref this.enable, value);
            }

            public float Progress {
                get => this.progress;
                set => this.Set<float> (ref this.progress, value);
            }

            public string Tip {
                get => this.tip;
                set => this.Set<string> (ref this.tip, value);
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
            public Slider downloadSlider;
            public Text progressText;
            public Text tipText;

            protected override void onCreate(ISnkBundle bundle = null)
            {
                base.onCreate(bundle);
                var viewModel = new PatchViewModel();
                var bindingSet = this.CreateBindingSet(viewModel);
                
                bindingSet.Bind(this.tipText).For(v => v.text).To(vm => vm.Tip);
                bindingSet.Build();
            }
        }
    }
}