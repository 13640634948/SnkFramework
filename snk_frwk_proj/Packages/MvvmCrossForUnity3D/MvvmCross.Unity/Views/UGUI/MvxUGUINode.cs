using UnityEngine;
using UnityEngine.EventSystems;

namespace MvvmCross.Unity.Views.UGUI
{
    public partial class MvxUGUINode : UIBehaviour, IMvxUGUINode
    {
        private static readonly bool UseBlocksRaycastsInsteadOfInteractable = false;

        public UIBehaviour Owner => this;

        private Canvas _canvas;
        public Canvas Canvas => this._canvas ??= GetComponent<Canvas>();

        private CanvasGroup _canvasGroup;
        public CanvasGroup CanvasGroup => this._canvasGroup ??= GetComponent<CanvasGroup>();
        
        private bool _activated = false;
     
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
                
                this.OnActivatedChanged();
                this.raiseActivatedChanged();
            }
        }
        
        public bool Activated
        {
            get => this._activated;
            protected set
            {
                if (this._activated == value)
                    return;
                this._activated = value;
                this.OnActivatedChanged();
                this.raiseActivatedChanged();
            }
        }
        
        protected virtual void OnActivatedChanged()
        {
            this.Interactable = this.Activated;
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