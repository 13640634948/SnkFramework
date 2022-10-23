using System;
using System.Collections;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Unity.ViewModels;
using MvvmCross.Unity.Views.Base;
using MvvmCross.Views;

namespace MvvmCross.Unity.Views
{
    public interface IMvxUnityView : IMvxView, IMvxBindingContextOwner, IMvxUnityEventSourceView, IDisposable
    {
        public IMvxUnityWindow ParentWindow { get; }
        public bool Visibility { get; set; }
        public bool Interactable { get; set; }

        public void Created(MvxUnityBundle bundle);
        public void Appearing();
        public void Appeared();
        public IEnumerator Activate(bool animated);
        public IEnumerator Passivate(bool animated);
        public void Disappearing();
        public void Disappeared();
        public void Dismiss();
    }

    public interface IMvxUnityView<TViewModel> : IMvxUnityView, IMvxView<TViewModel>
        where TViewModel : class, IMvxUnityViewModel
    {
        MvxFluentBindingDescriptionSet<IMvxUnityView<TViewModel>, TViewModel> CreateBindingSet();
    }
}