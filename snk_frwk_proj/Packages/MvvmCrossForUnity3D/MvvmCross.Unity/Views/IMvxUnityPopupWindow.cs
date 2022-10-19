using MvvmCross.Unity.ViewModels;

namespace MvvmCross.Unity.Views
{
    public interface IMvxUnityPopupWindow : IMvxUnityWindow
    {
    }
    
    public interface IMvxUnityPopupWindow<TViewModel, TUnityLayer, TUnityComponent> : IMvxUnityPopupWindow, IMvxUnityWindow<TViewModel, TUnityLayer, TUnityComponent>
        where TViewModel : class, IMvxUnityViewModel
        where TUnityLayer: class, IMvxUnityLayer
        where TUnityComponent : UnityEngine.Component
    {
    }
}