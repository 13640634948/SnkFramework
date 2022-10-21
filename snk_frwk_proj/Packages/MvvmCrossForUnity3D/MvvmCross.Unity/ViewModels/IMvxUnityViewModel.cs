using MvvmCross.ViewModels;

namespace MvvmCross.Unity.ViewModels
{
    public interface IMvxUnityViewModel : IMvxViewModel
    {
        void ViewShowing(bool animated);

        void ViewShowed();

        void ViewHideing(bool animated);

        void ViewHidden();
    }
    
    public interface IMvxUnityViewModel<in TParameter, TResult> : IMvxUnityViewModel, IMvxViewModel<TParameter, TResult>
    {
        
    }
}