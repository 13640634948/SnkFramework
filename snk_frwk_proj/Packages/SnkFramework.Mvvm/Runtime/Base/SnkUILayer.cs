using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SnkFramework.Mvvm.View;
using UnityEngine;

namespace SnkFramework.Mvvm.Base
{

    public class SnkUILayer : MonoBehaviour, IUILayer
    {
        private List<IWindow> windowList = new List<IWindow>();

        public ITransition Show(IWindow window)
        {
            return default;
            /*
            ShowTransition transition = new ShowTransition(this, (IManageable)window);
            GetTransitionExecutor().Execute(transition);
            return transition.OnStateChanged((w, state) =>
            {
                //Control the layer of the window
                if (state == WindowState.VISIBLE)
                    this.MoveToIndex(w, transition.Layer);

                //if (state == WindowState.INVISIBLE)
                //    this.MoveToLast(w);
            });
            */
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