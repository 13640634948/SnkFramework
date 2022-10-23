using UnityEngine;
using UnityEngine.EventSystems;

namespace MvvmCross.Unity.Views.UGUI
{
    public class MvxUGUINode : UIBehaviour, IMvxUGUINode
    {
        private static readonly bool UseBlocksRaycastsInsteadOfInteractable = false;

        public Canvas _canvas;
        public Canvas Canvas => this._canvas ??= GetComponent<Canvas>();

        public CanvasGroup _canvasGroup;
        public CanvasGroup CanvasGroup => this._canvasGroup ??= GetComponent<CanvasGroup>();

        public float Alpha
        {
            get => this.CanvasGroup.alpha;
            set => this.CanvasGroup.alpha = value;
        }

        public bool Visibility
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

        public bool Interactable
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