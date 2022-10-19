using MvvmCross.Unity.ViewModels;
using UnityEngine.EventSystems;

namespace MvvmCross.Unity.Views.UGUI
{
    public interface IMvxUGUIWindow : IMvxUnityWindow, IMvxUGUIView
    {
    }

    public interface IMvxUGUIWindow<TViewModel> : IMvxUGUIWindow, IMvxUnityWindow<TViewModel, MvxUGUILayer, UIBehaviour>, IMvxUGUIView<TViewModel>
        where TViewModel : class, IMvxUnityViewModel
    {
    }
}