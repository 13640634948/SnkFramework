using MvvmCross.Unity.ViewModels;

namespace MvvmCross.Unity.Views.UGUI
{
    public abstract class MvxUGUIPopupWindow<TViewModel> : MvxUGUIWindow<TViewModel>, IMvxUGUIPopupWindow<TViewModel>
        where TViewModel : class, IMvxUnityViewModel
    {
        
    }
}