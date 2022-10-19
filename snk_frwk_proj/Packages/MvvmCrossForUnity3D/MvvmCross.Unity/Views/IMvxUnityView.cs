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
        public UnityEngine.Component UnityOwner { get; }

        public IMvxUnityWindow ParentWindow { get; }
        public bool Visibility { get; set; }
        public bool Interactable { get; set; }

        public void Created(MvxUnityBundle Bundle);
        public void Appearing();
        public void Appeared();
        public IEnumerator Activate(bool animated);
        public IEnumerator Passivate(bool animated);
        public void Disappearing();
        public void Disappeared();
        public void Dismiss();
    }

    public interface IMvxUnityView<TViewModel, TUnityComponent> : IMvxUnityView, IMvxView<TViewModel>
        where TViewModel : class, IMvxUnityViewModel
        where TUnityComponent : UnityEngine.Component
    {
        public new TUnityComponent UnityOwner { get; }

        MvxFluentBindingDescriptionSet<IMvxUnityView<TViewModel, TUnityComponent>, TViewModel> CreateBindingSet();
    }
}