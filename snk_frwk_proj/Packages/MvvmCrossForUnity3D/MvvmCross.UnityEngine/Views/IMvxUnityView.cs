using MvvmCross.Binding.BindingContext;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.UnityEngine.Views
{
    public interface IMvxUnityView : IMvxView, IMvxBindingContextOwner
    {
    }

    public interface IMvxUnityView<TViewModel> : IMvxUnityView, IMvxView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        MvxFluentBindingDescriptionSet<IMvxUnityView<TViewModel>, TViewModel> CreateBindingSet();
    }
}