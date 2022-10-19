using MvvmCross.Unity.ViewModels;

namespace MvvmCross.Unity.Views.UGUI
{
    public abstract class MvxUGUIFullWindow<TViewModel> : MvxUGUIPopupWindow<TViewModel>, IMvxUGUIFullWindow<TViewModel>
        where TViewModel : class, IMvxUnityViewModel
    {
        
    }
}