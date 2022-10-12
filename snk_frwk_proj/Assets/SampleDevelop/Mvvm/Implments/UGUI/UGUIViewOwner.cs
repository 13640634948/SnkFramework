using UnityEngine;
using UnityEngine.EventSystems;

namespace SampleDevelop.Mvvm.Implments.UGUI
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public class UGUIViewOwner : UIBehaviour, IUGUIViewOwner
    { 
        private Canvas _canvas;
        public Canvas mCanvas => this._canvas ??= this.GetComponent<Canvas>();
        
        private CanvasGroup _canvasGroup;
        public CanvasGroup mCanvasGroup => this._canvasGroup ??= this.GetComponent<CanvasGroup>();
        public virtual void Dispose()
        {
            this.onDisposeBegin();
            if (!this.IsDestroyed() && this.gameObject != null)
                GameObject.Destroy(this.gameObject);
            this.onDisposeEnd();
        }

        protected virtual void onDisposeBegin()
        {
            
        }
        protected virtual void onDisposeEnd()
        {
            
        }
        public   bool mInteractable
        {
            get
            {
                if (this.gameObject == null)
                    return false;
                return this.mCanvasGroup.blocksRaycasts;
            }
            set
            {
                if (this.gameObject == null)
                    return;
                this.mCanvasGroup.blocksRaycasts = value;
            }
        }
    }
}