using MvvmCross.Unity.ViewModels;

namespace MvvmCross.Unity.Views
{
    public interface IMvxUnityPopupWindow : IMvxUnityWindow
    {
    }
    
    public interface IMvxUnityPopupWindow<TViewModel, TUnityLayer, TUnityOwner> : IMvxUnityPopupWindow, IMvxUnityWindow<TViewModel, TUnityLayer, TUnityOwner>
        where TViewModel : class, IMvxUnityViewModel
        where TUnityLayer: class, IMvxUnityLayer
        where TUnityOwner : class, IMvxUnityOwner
    {
    }
}