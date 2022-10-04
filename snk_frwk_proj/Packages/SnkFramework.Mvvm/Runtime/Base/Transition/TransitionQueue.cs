using System;
using System.Collections;
using System.Collections.Generic;
using SnkFramework.Mvvm.View;

namespace SnkFramework.Mvvm.Base
{
    public class TransitionQueue
    {
        private bool running = false;
        private List<Transition> transitions = new List<Transition>();


        public IEnumerator Play(Transition transition)
        {
            try
            {
                this.transitions.Add(transition);
            }
            catch
            {
                throw;
            }

            if (this.running == false)
                yield return this.DoTask();
        }

        private bool Check(Transition transition)
        {
            if (!(transition is ShowTransition))
                return true;

            ISnkWindowControllable window = transition.Window;
            var layer = window.UILayer;
            var current = layer.mCurrent;
            if (current == null)
                return true;

            if (current.WinType == WIN_TYPE.dialog || current.WinType == WIN_TYPE.progress)
                return false;

            if (current.WinType == WIN_TYPE.queue_popup &&
                !(window.WinType == WIN_TYPE.dialog || window.WinType == WIN_TYPE.progress))
                return false;
            return true;
        }

        protected virtual IEnumerator DoTask()
        {
            try
            {
                this.running = true;
                yield return null; //wait one frame
                while (this.transitions.Count > 0)
                {
                    Transition transition = this.transitions.Find(e => Check(e));
                    if (transition != null)
                    {
                        this.transitions.Remove(transition);
                        yield return transition.TransitionTask();

                        var layer = transition.Window.UILayer;
                        var current = layer.mCurrent;
                        if (layer.mActivated && current != null && !current.mActivated &&
                            !this.transitions.Exists((e) => e.Window.UILayer.Equals(layer)))
                        {
                            if (current == null)
                                throw new NullReferenceException("current is null");

                            yield return (current as ISnkWindowControllable).Activate(transition.AnimationDisabled);
                        }
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