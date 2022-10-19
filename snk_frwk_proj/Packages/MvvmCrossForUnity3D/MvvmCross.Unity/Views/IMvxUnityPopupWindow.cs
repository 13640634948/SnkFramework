using MvvmCross.Unity.ViewModels;

namespace MvvmCross.Unity.Views
{
    public interface IMvxUnityPopupWindow : IMvxUnityWindow
    {
    }
    
    public interface IMvxUnityPopupWindow<TViewModel, TUnityComponent> : IMvxUnityPopupWindow, IMvxUnityWindow<TViewModel, TUnityComponent>
        where TViewModel : class, IMvxUnityViewModel
        where TUnityComponent : UnityEngine.Component
    {
    }
}