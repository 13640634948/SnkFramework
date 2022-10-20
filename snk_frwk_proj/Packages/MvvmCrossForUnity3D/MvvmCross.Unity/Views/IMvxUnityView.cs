using System;
using System.Collections;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Unity.ViewModels;
using MvvmCross.Unity.Views.Base;
using MvvmCross.Views;

namespace MvvmCross.Unity.Views
{
    public interface IMvxUnityView : IMvxView, IMvxBindingContextOwner, IDisposable
    {
        public IMvxUnityOwner UnityOwner { get; }

        public IMvxUnityWindow ParentWindow { get; }
        public bool Visibility { get; set; }
        public bool Interactable { get; set; }

        public void Created(MvxUnityBundle Bundle);
        public void Appearing();
        public void Appeared(IMvxUnityOwner unityOwner);
        public IEnumerator Activate(bool animated);
        public IEnumerator Passivate(bool animated);
        public void Disappearing();
        public void Disappeared();
        public void Dismiss();
    }

    public interface IMvxUnityView<TViewModel, TUnityOwner> : IMvxUnityView, IMvxView<TViewModel>
        where TViewModel : class, IMvxUnityViewModel
        where TUnityOwner : class, IMvxUnityOwner
    {
        public new TUnityOwner UnityOwner { get; }

        MvxFluentBindingDescriptionSet<IMvxUnityView<TViewModel, TUnityOwner>, TViewModel> CreateBindingSet();
    }
}