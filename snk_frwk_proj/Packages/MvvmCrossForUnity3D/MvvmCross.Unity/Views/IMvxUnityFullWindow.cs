using MvvmCross.Unity.ViewModels;

namespace MvvmCross.Unity.Views
{
    public interface IMvxUnityFullWindow : IMvxUnityPopupWindow
    {
        
    }

    public interface IMvxUnityFullWindow<TViewModel,TUnityLayer, TUnityComponent> : IMvxUnityFullWindow, IMvxUnityPopupWindow<TViewModel, TUnityLayer, TUnityComponent>
        where TViewModel : class, IMvxUnityViewModel
        where TUnityLayer : class, IMvxUnityLayer
        where TUnityComponent : UnityEngine.Component
    {
    }
}