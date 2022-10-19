using MvvmCross.Unity.ViewModels;

namespace MvvmCross.Unity.Views
{
    public interface IMvxUnityFullWindow : IMvxUnityPopupWindow
    {
        
    }

    public interface IMvxUnityFullWindow<TViewModel, TUnityComponent> : IMvxUnityFullWindow, IMvxUnityPopupWindow<TViewModel, TUnityComponent>
        where TViewModel : class, IMvxUnityViewModel
        where TUnityComponent : UnityEngine.Component
    {
    }
}