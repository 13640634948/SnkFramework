using SnkFramework.Mvvm.View;
using SnkFramework.Mvvm.ViewModel;
using UnityEngine;

namespace SampleDevelop.Mvvm.Implments.UGUI
{
    public abstract class UGUIWindow<TViewModel> : Window
        where TViewModel : class, IViewModel, new()
    {
        public TViewModel mViewModel { get; set; }

        public override void SetOwner(GameObject owner)
        {
            this.mViewModel = new TViewModel();
            base.SetOwner(owner);
        }
    }
}