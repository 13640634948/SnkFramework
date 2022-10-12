using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnkFramework.Mvvm.View
{
    public interface ICoroutineExecutor
    {
        public IAsyncResult RunOnCoroutine(IEnumerator routine);
    }

    public interface ISnkTransitionExecutor
    {
        public void Execute(SnkTransition transition);
    }

    public class SnkTransitionPopupExecutor : ISnkTransitionExecutor
    {
        private ICoroutineExecutor _coroutineExecutor;
        private bool running = false;

        private IAsyncResult _asyncResult;

        public SnkTransitionPopupExecutor(ICoroutineExecutor coroutineExecutor)
        {
            this._coroutineExecutor = coroutineExecutor;
        }

        private List<SnkTransition> transitions = new List<SnkTransition>();

        public void Execute(SnkTransition transition)
        {
            try
            {
                this.transitions.Add(transition);
            }
            finally
            {
                if (this.running == false)
                    this._asyncResult = this._coroutineExecutor.RunOnCoroutine(DoTask());
            }
        }

        protected virtual bool Check(SnkTransition transition)=> true;

        protected virtual IEnumerator DoTask()
        {
            try
            {
                this.running = true;
                yield return null; //wait one frame
                while (this.transitions.Count > 0)
                {
                    SnkTransition transition = this.transitions.Find(e => Check(e));
                    if (transition != null)
                    {
                        this.transitions.Remove(transition);
                        yield return transition.TransitionTask();

                        var layer = transition.Window.mUILayer;//.UILayer;
                        var current = layer.Current;
                        if (layer.Activated && current != null && !current.mActivated)
                        {
                            if (current == null)
                                throw new NullReferenceException("current is null");

                            yield return (current as ISnkWindowControllable).Activate(transition.AnimationDisabled);
                        }
                        /*
                        else
                        {
                            Debug.LogError("Error");
                            Debug.LogError("layer.Activated:" + layer.Activated);
                            Debug.LogError("current:" + current);
                            if(current != null)
                                Debug.LogError("current.mActivated:" + current.mActivated);
                        }
                        */
                    }
                    else
                    {
                        yield return null;
                    }
                }
            }
            finally
            {
                this.running = false;
            }
        }
    }
}