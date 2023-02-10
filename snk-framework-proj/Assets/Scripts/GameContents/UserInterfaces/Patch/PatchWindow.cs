using System;
using BFFramework.Runtime.UserInterface;
using GAME.Contents.UserInterfaces.ViewModels;
using Loxodon.Framework.Binding.Contexts;
using SnkFramework.Mvvm.Runtime.Base;
using UnityEngine;
using UnityEngine.UI;

namespace GAME.Contents.UserInterfaces
{
    namespace ViewModels
    {
        public class PatchViewModel : BFViewModel, IBindingContextOwner
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

            public IBindingContext DataContext { get; set; }// = new BindingContext()
        }
    }

    namespace Views
    {
        public class PatchWindow : BFWindow
        {
            public string xxoo;
            
            public Slider downloadSlider;
            public Text progressText;
            public Text tipText;

            protected override void onCreate(ISnkBundle bundle = null)
            {
                /*
                base.onCreate(bundle);
                var vm = new PatchViewModel();
                var bindingSet = this.CreateBindingSet(vm);
                bindingSet.Bind(this.tipText).For(v => v.text).To(vm => vm.Tip);
                */
            }

            private void Update()
            {
                (this.ViewModel as PatchViewModel).Tip = xxoo;
            }
        }
    }
}