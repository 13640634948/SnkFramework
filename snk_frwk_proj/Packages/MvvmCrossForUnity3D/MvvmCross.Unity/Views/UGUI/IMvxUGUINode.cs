using UnityEngine;
using UnityEngine.EventSystems;

namespace MvvmCross.Unity.Views.UGUI
{
    public interface IMvxUGUINode
    {
        public UIBehaviour Owner { get; }
        public Canvas Canvas { get; }
        public CanvasGroup CanvasGroup { get; }
        
        public float Alpha { get; set; }
        public bool Visibility { get; set; }
        public bool Interactable { get; set; }
    }
}