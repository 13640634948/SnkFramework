using SnkFramework.Mvvm.Core.View;
using UnityEngine;
using UnityEngine.UI;

namespace SnkFramework.Mvvm.Runtime
{
    namespace UGUI
    {
        public class UGUILayer : SnkUILayer, IUGUILayer
        {
            private Canvas _canvas;
            public Canvas mCanvas => this._canvas;

            private CanvasScaler _canvasScaler;
            public CanvasScaler mCanvasScaler => _canvasScaler;

            private Transform _transform;
            public Transform transform => _transform ??= this._canvas ? this._canvas.transform : null;

            private GameObject _gameObject;
            public GameObject gameObject => _gameObject ??= this._canvas ? this._canvas.gameObject : null;

            public void SetOwner(Canvas canvas)
            {
                this._canvas = canvas;
                this._canvasScaler = this._canvas.GetComponent<CanvasScaler>() ??
                                     this._canvas.gameObject.AddComponent<CanvasScaler>();
            }

            public void AddChild(Transform child, bool worldPositionStays = false)
            {
                if (child == null || this.transform.Equals(child.parent))
                    return;

                child.gameObject.layer = this.gameObject.layer;
                child.SetParent(this.transform, worldPositionStays);
                child.SetAsFirstSibling();
            }

            public virtual void RemoveChild(Transform child, bool worldPositionStays = false)
            {
                if (child == null || !this.transform.Equals(child.parent))
                    return;

                child.SetParent(null, worldPositionStays);
            }

            public override ISnkTransition Show(ISnkWindow window)
            {
                ISnkTransition transition = base.Show(window);
                return transition.OnStateChanged((w, state) =>
                {
                    /* Control the layer of the window */
                    if (state == WindowState.VISIBLE)
                        onTransitionCompleted(window, transition);
                    //this.MoveToIndex(w, window.mPriority);
                });
            }

            public override ISnkTransition Hide(ISnkWindow window)
            {
                ISnkTransition transition = base.Hide(window);
                return transition.OnStateChanged((w, state) =>
                {
                    /* Control the layer of the window */
                    if (state == WindowState.INVISIBLE)
                        onTransitionCompleted(window, transition);
                    //this.MoveToLast(w);
                });
            }

            public override ISnkTransition Dismiss(ISnkWindow window)
            {
                ISnkTransition transition = base.Dismiss(window);
                return transition.OnStateChanged((w, state) =>
                {
                    /* Control the layer of the window */
                    if (state == WindowState.INVISIBLE)
                        onTransitionCompleted(window, transition);
                    //this.MoveToLast(w);
                });
            }

            protected virtual void onTransitionCompleted(ISnkWindow window, ISnkTransition transition)
            {
                IUGUIViewOwner viewOwner = window.mOwner as IUGUIViewOwner;
                IUGUILayer layer = window.mUILayer as IUGUILayer;

                viewOwner.mCanvas.overrideSorting = true;
                viewOwner.mCanvas.sortingLayerID = layer.mCanvas.sortingLayerID;
                viewOwner.mCanvas.sortingOrder = window.mPriority;
            }
        }
    }
}