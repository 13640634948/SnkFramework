using System;
using System.Collections;
using System.Collections.Generic;
using SnkFramework.Mvvm.Log;
using SnkFramework.Mvvm.View;

namespace SnkFramework.Mvvm.Base
{
    public abstract class SnkUILayerBase : IUILayer
    {
        public static readonly short SORTING_ORDER_STEP = 10;

        private static readonly IMvvmLog log = SnkMvvmSetup.mMvvmLog;

        private LinkedList<IWindow> _linkedList = new LinkedList<IWindow>();

        protected TransitionQueue _transitionQueue = new TransitionQueue();

        public bool mActivated { get; protected set; } = true;
        public IWindow mCurrent => _linkedList.Count == 0 ? null : _linkedList.Last.Value;

        public int Count => this._linkedList.Count;

        
        public short _currSortingOrder = 0;

        
        public virtual short AddSortingOrder()
        {
            _currSortingOrder -= SORTING_ORDER_STEP;
            return _currSortingOrder;
        }

        public abstract int GetSortingLayerID();
        
        public int IndexOf(IWindow window)
        {
            int index = -1;
            if (window == null)
                return index;
            LinkedList<IWindow>.Enumerator enumerator = this._linkedList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ++index;
                if (enumerator.Current == null || enumerator.Current.Dismissed)
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

            this._linkedList.AddLast(window);
        }

        public virtual void CoroutineExecuter(IEnumerator routine)
            => SnkMvvmSetup.mCoroutineExecutor.RunOnCoroutineNoReturn(routine);

        public virtual ITransition Show(ISnkWindowControllable window)
        {
            ShowTransition transition = new ShowTransition(this, window);
            CoroutineExecuter(this._transitionQueue.Play(transition));
            return transition.OnStateChanged((w, state) =>
            {
                /* Control the layer of the window */
                if (state == WindowState.VISIBLE)
                    log.InfoFormat("Show-MoveToIndex({0}, {1});", window, transition.Layer);
                //this.MoveToIndex(w, transition.Layer);

                //if (state == WindowState.INVISIBLE)
                //    this.MoveToLast(w);
            });
        }

        public virtual ITransition Hide(ISnkWindowControllable window)
        {
            HideTransition transition = new HideTransition(this, window, false);
            CoroutineExecuter(this._transitionQueue.Play(transition));
            return transition.OnStateChanged((w, state) =>
            {
                /* Control the layer of the window */
                if (state == WindowState.INVISIBLE)
                    log.InfoFormat("Hide-MoveToIndex({0}, {1});", window, transition.Layer);
                // this.MoveToLast(w);
            });
        }

        public virtual ITransition Dismiss(ISnkWindowControllable window)
        {
            HideTransition transition = new HideTransition(this, window, true);
            CoroutineExecuter(this._transitionQueue.Play(transition));
            return transition.OnStateChanged((w, state) =>
            {
                /* Control the layer of the window */
                if (state == WindowState.INVISIBLE)
                    log.InfoFormat("Dismiss-MoveToIndex({0}, {1});", window, transition.Layer);
//                    this.MoveToLast(w);
            });
        }
    }
}