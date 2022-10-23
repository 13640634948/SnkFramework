using MvvmCross.Unity.ViewModels;
using UnityEngine;

namespace MvvmCross.Unity.Views.UGUI
{
    public interface IMvxUGUIView : IMvxUnityView
    {
        public CanvasGroup CanvasGroup { get; }
    }

    public interface IMvxUGUIView<TViewModel> : IMvxUGUIView, IMvxUnityView<TViewModel>
        where TViewModel : class, IMvxUnityViewModel
    {
    }
}