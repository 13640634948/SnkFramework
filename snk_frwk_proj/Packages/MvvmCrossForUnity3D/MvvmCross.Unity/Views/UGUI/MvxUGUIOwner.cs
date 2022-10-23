using UnityEngine;
using UnityEngine.EventSystems;

namespace MvvmCross.Unity.Views.UGUI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MvxUGUIOwner : UIBehaviour, IMvxUGUIOwner
    {
        private static readonly bool UseBlocksRaycastsInsteadOfInteractable = false;

        private CanvasGroup _canvasGroup;
        public UIBehaviour Owner => this;
        public CanvasGroup CanvasGroup => _canvasGroup ??= this.GetComponent<CanvasGroup>();

        public bool IsVisibility
        {
            get
            {
                if (this.IsDestroyed() || this.gameObject == null)
                    return false;
                return this.gameObject.activeSelf;
            }
            set
            {
                if (this.IsDestroyed() || this.gameObject == null)
                    return;

                if (this.gameObject.activeSelf == value)
                    return;

                this.gameObject.SetActive(value);
            }
        }

        public bool IsInteractable
        {
            get
            {
                if (this.IsDestroyed() || this.gameObject == null)
                    return false;

                if (UseBlocksRaycastsInsteadOfInteractable)
                    return this.CanvasGroup.blocksRaycasts;
                return this.CanvasGroup.interactable;
            }
            set
            {
                if (this.IsDestroyed() || this.gameObject == null)
                    return;

                if (UseBlocksRaycastsInsteadOfInteractable)
                    this.CanvasGroup.blocksRaycasts = value;
                else
                    this.CanvasGroup.interactable = value;
            }
        }
    }
}