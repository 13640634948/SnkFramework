using System;
using System.Collections;

namespace SampleDevelop.Test
{
    public class SnkUIShowTransition : SnkUITransition
    {
        private ISnkUILayer _uiLayer;
        public SnkUIShowTransition(ISnkUILayer uiLayer, ISnkControllable window) : base(window)
        {
            this._uiLayer = uiLayer;
        }

        protected virtual ActionType Overlay(ISnkWindow previous, ISnkWindow current)
        {
            return ActionType.None;
            /*
            if (previous == null || previous.WindowType == WindowType.FULL)
                return ActionType.None;

            if (previous.WindowType == WindowType.POPUP)
                return ActionType.Dismiss;

            return ActionType.None;
            */
        }
        
        protected override IEnumerator DoTransition()
            {
                ISnkControllable current = this.Window;
                //IManageable previous = (IManageable)this.manager.GetVisibleWindow(layer);
                ISnkControllable previous = (ISnkControllable)this._uiLayer.Current;
                if (previous != null)
                {
                    //Passivate the previous window
                    if (previous.mActivated)
                    {
                        IAsyncResult passivate = previous.Passivate(this.AnimationDisabled);
                        while (passivate.IsCompleted == false)
                            yield return null;

                        //yield return passivate.WaitForDone();
                    }

                    Func<ISnkWindow, ISnkWindow, ActionType> policy = this.OverlayPolicy;
                    if (policy == null)
                        policy = this.Overlay;
                    ActionType actionType = policy(previous, current);
                    switch (actionType)
                    {
                        case ActionType.Hide:
                            previous.DoHide(this.AnimationDisabled);
                            break;
                        case ActionType.Dismiss:
                            IAsyncResult result = previous.DoHide(this.AnimationDisabled);
                            while (result.IsCompleted == false)
                                yield return null;
                            previous.DoDismiss();
                            /*
                            previous.DoHide(this.AnimationDisabled).Callbackable().OnCallback((r) =>
                            {
                                previous.DoDismiss();
                            });
                            */
                            break;
                        default:
                            break;
                    }
                }

                if (!current.mVisibility)
                {
                    IAsyncResult result = current.DoShow(this.AnimationDisabled);
                    while (result.IsCompleted == false)
                        yield return null;
                    //yield return result.WaitForDone();
                }

                if (this._uiLayer.Activated && current.Equals(this._uiLayer.Current))
                {
                    IAsyncResult result = current.Activate(this.AnimationDisabled);
                    while (result.IsCompleted == false)
                        yield return null;
                    //yield return activate.WaitForDone();
                }
            }
    }
}