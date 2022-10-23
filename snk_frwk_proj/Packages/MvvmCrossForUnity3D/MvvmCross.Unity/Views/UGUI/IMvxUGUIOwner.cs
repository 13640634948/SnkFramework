using UnityEngine;
using UnityEngine.EventSystems;

namespace MvvmCross.Unity.Views.UGUI
{
    public interface IMvxUGUIOwner : IMvxUnityOwner
    {
        public UIBehaviour Owner { get; }
        public CanvasGroup CanvasGroup { get; }
        public bool IsVisibility { get; set; }
        public bool IsInteractable { get; set; }
    }
}