using MvvmCross.Unity.ViewModels;

namespace MvvmCross.Unity.Views.UGUI
{
    public interface IMvxUGUIFullWindow : IMvxUGUIPopupWindow, IMvxUnityFullWindow
    {
    }
    
    public interface IMvxUGUIFullWindow<TViewModel> : IMvxUGUIFullWindow, IMvxUGUIPopupWindow<TViewModel>, IMvxUnityFullWindow<TViewModel, MvxUGUILayer,IMvxUGUIOwner>
        where TViewModel : class, IMvxUnityViewModel
    {
    }
}