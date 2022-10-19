using MvvmCross.Unity.ViewModels;
using UnityEngine.EventSystems;

namespace MvvmCross.Unity.Views.UGUI
{
    public interface IMvxUGUIFullWindow : IMvxUGUIPopupWindow, IMvxUnityFullWindow
    {
    }
    
    public interface IMvxUGUIFullWindow<TViewModel> : IMvxUGUIFullWindow, IMvxUGUIPopupWindow<TViewModel>, IMvxUnityFullWindow<TViewModel, UIBehaviour>
        where TViewModel : class, IMvxUnityViewModel
    {
    }
}