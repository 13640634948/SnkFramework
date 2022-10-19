using MvvmCross.Unity.ViewModels;
using UnityEngine.EventSystems;

namespace MvvmCross.Unity.Views.UGUI
{
    public interface IMvxUGUIPopupWindow : IMvxUGUIWindow, IMvxUnityPopupWindow 
    {
    }
    
    public interface IMvxUGUIPopupWindow<TViewModel> : IMvxUGUIPopupWindow, IMvxUGUIWindow<TViewModel>, IMvxUnityPopupWindow<TViewModel, MvxUGUILayer,UIBehaviour>
        where TViewModel : class, IMvxUnityViewModel
    {
    }
}