using System.Collections;
using MvvmCross.Base;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Unity.ViewModels;
using MvvmCross.Unity.Views.Base;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MvvmCross.Unity.Views.UGUI
{
    public abstract class MvxUGUIView<TViewModel> : MvxUnityEventSourceView, IMvxUGUIView<TViewModel>
        where TViewModel : class, IMvxUnityViewModel
    {
        private static readonly bool UseBlocksRaycastsInsteadOfInteractable = false;
        Component IMvxUnityView.UnityOwner => _unityOwner;
        public UIBehaviour UnityOwner => _unityOwner;
        
        internal UIBehaviour _unityOwner;
        
        public virtual IMvxUnityWindow ParentWindow { get; }

        private CanvasGroup _canvasGroup;
        public virtual CanvasGroup CanvasGroup
            => _canvasGroup ??= this.UnityOwner.GetComponent<CanvasGroup>();

        public virtual IMvxBindingContext BindingContext { get; set; }

        public virtual object? DataContext
        {
            get => BindingContext.DataContext;
            set => BindingContext.DataContext = value;
        }
        
        IMvxViewModel? IMvxView.ViewModel
        {
            get => ViewModel;
            set => ViewModel = value as TViewModel;
        }

        public TViewModel? ViewModel { get; set; }

        public virtual bool Visibility
        {
            get
            {
                return !this.UnityOwner.IsDestroyed() && this.UnityOwner.gameObject != null
                    ? this.UnityOwner.gameObject.activeSelf
                    : false;
            }
            set
            {
                if (this.UnityOwner.IsDestroyed() || this.UnityOwner.gameObject == null)
                    return;

                if (this.UnityOwner.gameObject.activeSelf == value)
                    return;

                this.UnityOwner.gameObject.SetActive(value);
            }
        }


        public virtual bool Interactable {  get
            {
                if (this.UnityOwner.IsDestroyed() || this.UnityOwner.gameObject == null)
                    return false;

                if (UseBlocksRaycastsInsteadOfInteractable)
                    return this.CanvasGroup.blocksRaycasts;
                return this.CanvasGroup.interactable;
            }
            set
            {
                if (this.UnityOwner.IsDestroyed() || this.UnityOwner.gameObject == null)
                    return;

                if (UseBlocksRaycastsInsteadOfInteractable)
                    this.CanvasGroup.blocksRaycasts = value;
                else
                    this.CanvasGroup.interactable = value;
            } }


        public MvxFluentBindingDescriptionSet<IMvxUnityView<TViewModel, UIBehaviour>, TViewModel> CreateBindingSet()
            => this.CreateBindingSet<IMvxUnityView<TViewModel, UIBehaviour>, TViewModel>();


        public virtual void Created(MvxUnityBundle Bundle)
        {
            createCalled?.Raise(this, Bundle);
            this.ViewModel?.ViewCreated();
        }

        public virtual void Appearing()
        {
            appearingCalled.Raise(this);
            this.ViewModel?.ViewAppearing();
        }

        public virtual void Appeared()
        {
            appearedCalled.Raise(this);
            this.ViewModel?.ViewAppeared();
        }

        public virtual IEnumerator Activate(bool animated)
        {
            activateCalled.Raise(this, animated);
            yield break;
        }

        public virtual IEnumerator Passivate(bool animated)
        {
            passivateCalled.Raise(this, animated);
            yield break;
        }

        public virtual void Disappearing()
        {
            disappearingCalled.Raise(this);
            this.ViewModel?.ViewDisappearing();
        }

        public virtual void Disappeared()
        {
            disappearedCalled?.Raise(this);
            this.ViewModel?.ViewDisappeared();
        }

        public virtual void Dismiss()
        {
            dismissCalled.Raise(this);
            this.ViewModel?.ViewDestroy();
        }

        public virtual void Dispose()
        {
            disposeCalled?.Raise(this);
            this.ViewModel?.ViewDisappeared();
        }

    }
}