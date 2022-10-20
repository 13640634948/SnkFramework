using MvvmCross.Unity.ViewModels;

namespace MvvmCross.Unity.Views
{
    public interface IMvxUnityFullWindow : IMvxUnityPopupWindow
    {
        
    }

    public interface IMvxUnityFullWindow<TViewModel,TUnityLayer, TUnityOwner> : IMvxUnityFullWindow, IMvxUnityPopupWindow<TViewModel, TUnityLayer, TUnityOwner>
        where TViewModel : class, IMvxUnityViewModel
        where TUnityLayer : class, IMvxUnityLayer
        where TUnityOwner : class, IMvxUnityOwner
    {
    }
}