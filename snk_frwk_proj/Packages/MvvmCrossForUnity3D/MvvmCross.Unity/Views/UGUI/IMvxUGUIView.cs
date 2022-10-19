using MvvmCross.Unity.ViewModels;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MvvmCross.Unity.Views.UGUI
{
    public interface IMvxUGUIView : IMvxUnityView
    {
        public CanvasGroup CanvasGroup { get; }
    }

    public interface IMvxUGUIView<TViewModel> : IMvxUGUIView, IMvxUnityView<TViewModel, UIBehaviour>
        where TViewModel : class, IMvxUnityViewModel
    {
    }
}