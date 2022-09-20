using System;
using System.Collections.Generic;
using SnkFramework.Mvvm.View;
using UnityEngine;

namespace SnkFramework.Mvvm.Base
{
    public abstract class SnkUILayerBase : MonoBehaviour, IUILayer
    {
        private LinkedList<IWindow> _linkedList = new LinkedList<IWindow>();

        protected TransitionQueue _transitionQueue = new TransitionQueue();

        public bool mActivated { get; protected set; }
        public IWindow mCurrent { get; protected set; }

        public int IndexOf(IWindow window)
        {
            int index = -1;
            if (window == null)
                return index;
            LinkedList<IWindow>.Enumerator enumerator = this._linkedList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ++index;
                if(enumerator.Current == null || enumerator.Current.Dismissed)
                    continue;
                if (enumerator.Current.mName == window.mName)
                    return index;
            }
            return index;
        }

        public virtual bool Remove(IWindow window)
        {
            if (window == null)
                throw new ArgumentNullException("window");

            window.mOwner.transform.SetParent(null);
            return this._linkedList.Remove(window);
        }

        public virtual void Add(IWindow window)
        {
            if (window == null)
                throw new ArgumentNullException("window");

            if (this._linkedList.Contains(window))
                return;

            LinkedListNode<IWindow> windowNode = this._linkedList.AddLast(window);
            RectTransform child = windowNode.Value.mOwner.transform as RectTransform;

            if (child == null || this.transform.Equals(child.parent))
                return;

            child.gameObject.layer = this.gameObject.layer;
            child.SetParent(this.transform);
            
            child.anchorMin = Vector2.zero;
            child.anchorMax = Vector2.one;
            child.sizeDelta = Vector2.zero;
            child.anchoredPosition3D = Vector3.zero;
            child.localScale = Vector3.one;
            
            
            child.SetAsFirstSibling();
        }

        public virtual ITransition Show(ISnkWindowControllable window)
        {
            ShowTransition transition = new ShowTransition(this, window);
            StartCoroutine(this._transitionQueue.Play(transition));
            return transition.OnStateChanged((w, state) =>
            {
                /* Control the layer of the window */
                if (state == WindowState.VISIBLE)
                    Debug.LogFormat("Show-MoveToIndex({0}, {1});", window, transition.Layer);
                //this.MoveToIndex(w, transition.Layer);

                //if (state == WindowState.INVISIBLE)
                //    this.MoveToLast(w);
            });
        }

        public virtual ITransition Hide(ISnkWindowControllable window)
        {    
            HideTransition transition = new HideTransition(this, window, false);
            StartCoroutine(this._transitionQueue.Play(transition));
            return transition.OnStateChanged((w, state) =>
            {
                /* Control the layer of the window */
                if (state == WindowState.INVISIBLE)
                    Debug.LogFormat("Hide-MoveToIndex({0}, {1});", window, transition.Layer);
                   // this.MoveToLast(w);
            });
        }

        public virtual ITransition Dismiss(ISnkWindowControllable window)
        {
            HideTransition transition = new HideTransition(this, window, true);
            StartCoroutine(this._transitionQueue.Play(transition));
            return transition.OnStateChanged((w, state) =>
            {
                /* Control the layer of the window */
                if (state == WindowState.INVISIBLE)
                    Debug.LogFormat("Dismiss-MoveToIndex({0}, {1});", window, transition.Layer);
//                    this.MoveToLast(w);
            });
        }
    }
}