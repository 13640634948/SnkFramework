using MvvmCross.Unity.ViewModels;

namespace MvvmCross.Unity.Views.UGUI
{
    public interface IMvxUGUIPopupWindow : IMvxUGUIWindow, IMvxUnityPopupWindow 
    {
    }
    
    public interface IMvxUGUIPopupWindow<TViewModel> : IMvxUGUIPopupWindow, IMvxUGUIWindow<TViewModel>, IMvxUnityPopupWindow<TViewModel, MvxUGUILayer,IMvxUGUIOwner>
        where TViewModel : class, IMvxUnityViewModel
    {
    }
}