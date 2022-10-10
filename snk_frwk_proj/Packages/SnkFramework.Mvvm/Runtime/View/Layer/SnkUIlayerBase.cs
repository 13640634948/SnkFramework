using System;
using System.Collections.Generic;
using SnkFramework.Mvvm.Base;
using UnityEngine;

namespace SampleDevelop.Test
{
    public abstract class SnkUIlayerBase : ISnkUILayer
    {
        public IUILocator mLocator { get; }

        public SnkUIlayerBase(IUILocator locator)
        {
            mLocator = locator;
        }

        protected List<ISnkWindow> windowList = new();

        private bool _activated = true;
        
        public virtual ISnkWindow Current
        {
            get
            {
                if (this.windowList == null || this.windowList.Count <= 0)
                    return null;

                var window = this.windowList[0];
                return window != null && window.mVisibility ? window : null;
            }
        }

        public virtual bool Activated
        {
            get => _activated;
            set => _activated = value;
        }

        public virtual int Count => this.windowList.Count;
        
        public ISnkTransitionExecutor mTransitionExecutor;

        public virtual ISnkTransition Show(ISnkWindow window)
        {
            SnkUIShowTransition transition = new SnkUIShowTransition(this, (ISnkControllable)window);
            this.mTransitionExecutor.Execute(transition);
            return transition.OnStateChanged((w, state) =>
            {
                /* Control the layer of the window */
                if (state == WindowState.VISIBLE)
//                    this.MoveToIndex(w, transition.Layer);
                  this.MoveToIndex(w, window.mPriority);
            });
        }

        public virtual ISnkTransition Hide(ISnkWindow window)
        {
            SnkUIHideTransition transition = new SnkUIHideTransition(this, (ISnkControllable)window, false);
            this.mTransitionExecutor.Execute(transition);
            return transition.OnStateChanged((w, state) =>
            {
                /* Control the layer of the window */
                if (state == WindowState.INVISIBLE)
                    this.MoveToLast(w);
            });
        }

        public virtual ISnkTransition Dismiss(ISnkWindow window)
        {
            SnkUIHideTransition transition = new SnkUIHideTransition(this, (ISnkControllable)window, true);
            this.mTransitionExecutor.Execute(transition);
            return transition.OnStateChanged((w, state) =>
            {
                /* Control the layer of the window */
                if (state == WindowState.INVISIBLE)
                    this.MoveToLast(w);
            });
        }

        public virtual IEnumerator<ISnkWindow> Visibles()
            => new SnkInternalVisibleEnumerator(this.windowList);

        public virtual ISnkWindow Get(int index)
        {
            if (index < 0 || index > this.windowList.Count - 1)
                throw new IndexOutOfRangeException();

            return this.windowList[index];
        }

        //protected virtual void onAdd(ISnkWindow window){ }
        //protected virtual void onRemove(ISnkWindow window){ }
        //protected virtual void onRemoveAt(ISnkWindow window, int index){ }

        public virtual void Add(ISnkWindow window)
        {
            if (window == null)
                throw new ArgumentNullException("window");

            if (this.windowList.Contains(window))
                return;

            window.mUILayer = this;
            window.mPriority = this.GetPrioritySerialNum();
            this.windowList.Add(window);
            //this.onAdd(window);
        }

        public virtual bool Remove(ISnkWindow window)
        {
            if (window == null)
                throw new ArgumentNullException("window");

            //this.onRemove(window);
            return this.windowList.Remove(window);
        }

        public virtual ISnkWindow RemoveAt(int index)
        {
            if (index < 0 || index > this.windowList.Count - 1)
                throw new IndexOutOfRangeException();

            var window = this.windowList[index];
            //this.onRemoveAt(window, index);
            this.windowList.RemoveAt(index);
            return window;
        }

        public virtual bool Contains(ISnkWindow window)
            => this.windowList.Contains(window);
        
        public virtual int IndexOf(ISnkWindow window)
            =>this.windowList.IndexOf(window);
        
        public virtual List<ISnkWindow> Find(bool visible)
            =>this.windowList.FindAll(w => w.mVisibility == visible);

        public virtual T Find<T>() where T : class, ISnkWindow
            =>this.windowList.Find(w => w is T) as T;

        public virtual T Find<T>(string name) where T : class, ISnkWindow
            => this.windowList.Find(w => w is T && w.mName.Equals(name)) as T;

        public virtual List<T> FindAll<T>() where T : class, ISnkWindow
        {
            List<T> list = new List<T>();
            foreach (ISnkWindow window in this.windowList)
            {
                if (window is T)
                    list.Add(window as T);
            }
            return list;
        }

        public virtual void Clear()
        {
            for (int i = 0; i < this.windowList.Count; i++)
            {
                try
                {
                    this.windowList[i].Dismiss(true);
                }
                catch (Exception) { }
            }
            this.windowList.Clear();
        }

        protected int _serialNum = 0;
        public virtual int GetPrioritySerialNum() => _serialNum++;

        protected virtual void MoveToIndex(ISnkWindow window, int index)
        {
            if (window == null)
                throw new ArgumentNullException("window");

            int oldIndex = this.IndexOf(window);
            
            if (oldIndex < 0 || oldIndex == index)
                return;

            Debug.Log("MoveToIndex:" + window.mName + ", index:" + index);
            this.windowList.RemoveAt(oldIndex);
            this.windowList.Insert(index, window);
            PrintWindowPriority();
        }
        
        protected virtual void MoveToLast(ISnkWindow window)
        {
            if (window == null)
                throw new ArgumentNullException("window");
            int index = this.IndexOf(window);
            if (index < 0 || index == this.Count - 1)
                return;

            this.windowList.RemoveAt(index);
            this.windowList.Add(window);
            PrintWindowPriority();
        }

        protected abstract void PrintWindowPriority();
    }
}