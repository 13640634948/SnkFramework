using MvvmCross.Unity.ViewModels;

namespace MvvmCross.Unity.Views.UGUI
{
    public interface IMvxUGUIWindow : IMvxUnityWindow, IMvxUGUIView
    {
    }

    public interface IMvxUGUIWindow<TViewModel> : IMvxUGUIWindow, IMvxUnityWindow<TViewModel, IMvxUGUILayer>, IMvxUGUIView<TViewModel>
        where TViewModel : class, IMvxUnityViewModel
    {
    }
}