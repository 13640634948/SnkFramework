using System.Collections;
using MvvmCross.Base;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Unity.ViewModels;
using MvvmCross.Unity.Views.Base;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Unity.Views.UGUI
{
    public abstract partial class MvxUGUIView<TViewModel> : MvxUGUINode, IMvxUGUIView<TViewModel>
        where TViewModel : class, IMvxUnityViewModel
    {
        public TViewModel? ViewModel { get; set; }
        public virtual IMvxBindingContext BindingContext { get; set; }
        public virtual IMvxUnityWindow ParentWindow { get; }
 
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

        public MvxFluentBindingDescriptionSet<IMvxUnityView<TViewModel>, TViewModel> CreateBindingSet()
            => this.CreateBindingSet<IMvxUnityView<TViewModel>, TViewModel>();

        public virtual void Created(MvxUnityBundle bundle)
        {
            createCalled?.Raise(this, bundle);
            this.ViewModel?.ViewCreated();
        }

        public virtual void Appearing()
        {
            appearingCalled?.Raise(this);
            this.ViewModel?.ViewAppearing();
        }

        public virtual void Appeared()
        {
            appearedCalled?.Raise(this);
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