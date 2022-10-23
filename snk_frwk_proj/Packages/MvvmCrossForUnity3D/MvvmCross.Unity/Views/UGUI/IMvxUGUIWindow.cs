using MvvmCross.Unity.ViewModels;

namespace MvvmCross.Unity.Views.UGUI
{
    public interface IMvxUGUIWindow : IMvxUnityWindow, IMvxUGUIView
    {
    }

    public interface IMvxUGUIWindow<TViewModel> : IMvxUGUIWindow, IMvxUnityWindow<TViewModel, MvxUGUILayer>, IMvxUGUIView<TViewModel>
        where TViewModel : class, IMvxUnityViewModel
    {
    }
}