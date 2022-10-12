using System;
using System.Collections.Generic;

namespace SnkFramework.Mvvm.View
{
    public abstract class SnkUILayer : ISnkUILayer
    {
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
        protected int _priorityNum = 0;

        public ISnkTransitionExecutor mTransitionExecutor;
        
        public virtual ISnkTransition Show(ISnkWindow window)
        {
            SnkUIShowTransition transition = new SnkUIShowTransition(this, (ISnkWindowControllable)window);
            this.mTransitionExecutor.Execute(transition);
            return transition;
        }

        public virtual ISnkTransition Hide(ISnkWindow window)
        {
            SnkUIHideTransition transition = new SnkUIHideTransition(this, (ISnkWindowControllable)window, false);
            this.mTransitionExecutor.Execute(transition);
            return transition;
        }

        public virtual ISnkTransition Dismiss(ISnkWindow window)
        {
            SnkUIHideTransition transition = new SnkUIHideTransition(this, (ISnkWindowControllable)window, true);
            this.mTransitionExecutor.Execute(transition);
            return transition;
        }

        public virtual IEnumerator<ISnkWindow> Visibles()
            => new SnkInternalVisibleEnumerator(this.windowList);

        public virtual ISnkWindow Get(int index)
        {
            if (index < 0 || index > this.windowList.Count - 1)
                throw new IndexOutOfRangeException();

            return this.windowList[index];
        }

        public virtual void Add(ISnkWindow window)
        {
            if (window == null)
                throw new ArgumentNullException("window");

            if (this.windowList.Contains(window))
                return;

            window.mUILayer = this;
            window.mPriority = this._priorityNum++;
            this.windowList.Add(window);
        }

        public virtual bool Remove(ISnkWindow window)
        {
            if (window == null)
                throw new ArgumentNullException("window");

            return this.windowList.Remove(window);
        }

        public virtual ISnkWindow RemoveAt(int index)
        {
            if (index < 0 || index > this.windowList.Count - 1)
                throw new IndexOutOfRangeException();

            var window = this.windowList[index];
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
    }
}