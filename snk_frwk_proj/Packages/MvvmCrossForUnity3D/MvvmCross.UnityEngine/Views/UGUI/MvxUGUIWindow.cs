using MvvmCross.ViewModels;

namespace MvvmCross.UnityEngine.Views.UGUI
{
    public abstract class MvxUGUIWindow<TViewModel> : MvxUGUIView<TViewModel>, IMvxUGUIWindow
        where TViewModel : class, IMvxViewModel
    {
    }
}