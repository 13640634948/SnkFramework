using MvvmCross.ViewModels;
using UnityEngine.EventSystems;

namespace MvvmCross.Unity.Views.UGUI
{
    public abstract class MvxUGUIView<TViewModel> : MvxUnityView<TViewModel, UIBehaviour>, IMvxUGUIView
        where TViewModel : class, IMvxViewModel

    {
        
    }
}