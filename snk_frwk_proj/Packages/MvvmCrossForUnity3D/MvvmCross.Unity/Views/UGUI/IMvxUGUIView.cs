using MvvmCross.Unity.ViewModels;
using UnityEngine;

namespace MvvmCross.Unity.Views.UGUI
{
    public interface IMvxUGUIView : IMvxUnityView, IMvxUGUINode
    {
    }

    public interface IMvxUGUIView<TViewModel> : IMvxUGUIView, IMvxUnityView<TViewModel>
        where TViewModel : class, IMvxUnityViewModel
    {
    }
}