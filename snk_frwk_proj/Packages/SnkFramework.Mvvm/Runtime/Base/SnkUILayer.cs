using System;
using System.Collections.Generic;
using SnkFramework.Mvvm.View;
using UnityEngine;

namespace SnkFramework.Mvvm.Base
{
    public interface ISnkUIMainLayer : IUILayer
    {
    }

    [RequireComponent(typeof(RectTransform), typeof(Canvas))]
    public class SnkUIMainLayer : SnkUILayer, ISnkUIMainLayer
    {
    }

    public class SnkUILayer : MonoBehaviour, IUILayer
    {
        private List<IWindow> windowList = new List<IWindow>();

        public int VisibleCount
        {
            get { return this.windowList.FindAll(w => w.mVisibility).Count; }
        }

        /*
        public ITransition Show(IWindow window)
        {
            ShowTransition transition = new ShowTransition(this, (IManageable) window);
            GetTransitionExecutor().Execute(transition);
            return transition.OnStateChanged((w, state) =>
            {
                //Control the layer of the window
                if (state == WindowState.VISIBLE)
                    this.MoveToIndex(w, transition.Layer);

                //if (state == WindowState.INVISIBLE)
                //    this.MoveToLast(w);
            });
        }
        */

        public ITransition Show(IWindow window)
        {
            if (window.Activated == false)
                window.Activate(true);
            return default;
        }

        public ITransition Hide(IWindow window)
        {
            if (window.Activated)
                window.Passivate(false);
            if (window.mVisibility)
                window.DoHide(true);
            return default;
        }

        public ITransition Dismiss(IWindow window)
        {
            Hide(window);
            window.DoDismiss();
            return default;
        }

        public virtual int IndexOf(IWindow window)
        {
            return this.windowList.IndexOf(window);
        }

        protected virtual int GetChildIndex(Transform child)
        {
            Transform transform = this.transform;
            int count = transform.childCount;
            for (int i = count - 1; i >= 0; i--)
            {
                if (transform.GetChild(i).Equals(child))
                    return i;
            }

            return -1;
        }


        protected virtual void MoveToIndex(IWindow window, int index)
        {
            if (window == null)
                throw new ArgumentNullException("window");

            int oldIndex = this.IndexOf(window);
            try
            {
                if (oldIndex < 0 || oldIndex == index)
                    return;

                this.windowList.RemoveAt(oldIndex);
                this.windowList.Insert(index, window);
            }
            finally
            {
                Transform transform = GetTransform(window);
                if (transform != null)
                {
                    if (index == 0)
                    {
                        transform.SetAsLastSibling();
                    }
                    else
                    {
                        IWindow preWindow = this.windowList[index - 1];
                        int preWindowPosition = GetChildIndex(GetTransform(preWindow));
                        int currWindowPosition = oldIndex >= index ? preWindowPosition - 1 : preWindowPosition;
                        transform.SetSiblingIndex(currWindowPosition);
                    }
                }
            }
        }

        public virtual void Add(IWindow window)
        {
            if (window == null)
                throw new ArgumentNullException("window");

            if (this.windowList.Contains(window))
                return;

            this.windowList.Add(window);
            this.AddChild(GetTransform(window));
        }

        protected virtual void AddChild(Transform child, bool worldPositionStays = false)
        {
            if (child == null || this.transform.Equals(child.parent))
                return;

            child.gameObject.layer = this.gameObject.layer;
            child.SetParent(this.transform, worldPositionStays);
            child.SetAsFirstSibling();
        }

        public virtual bool Remove(IWindow window)
        {
            if (window == null)
                throw new ArgumentNullException("window");

            this.RemoveChild(GetTransform(window));
            return this.windowList.Remove(window);
        }

        protected virtual void RemoveChild(Transform child, bool worldPositionStays = false)
        {
            if (child == null || !this.transform.Equals(child.parent))
                return;

            child.SetParent(null, worldPositionStays);
        }

        protected virtual Transform GetTransform(IWindow window)
        {
            if (window == null || window.mOwner == null)
                return null;
            return window.mOwner.transform;
            /*
            try
            {
                if (window == null)
                    return null;

                if (window is UIView)
                    return (window as UIView).Transform;

                var propertyInfo = window.GetType().GetProperty("Transform");
                if (propertyInfo != null)
                    return (Transform)propertyInfo.GetGetMethod().Invoke(window, null);

                if (window is Component)
                    return (window as Component).transform;
                return null;
            }
            catch (Exception)
            {
                return null;
            }
            */
        }
    }
}