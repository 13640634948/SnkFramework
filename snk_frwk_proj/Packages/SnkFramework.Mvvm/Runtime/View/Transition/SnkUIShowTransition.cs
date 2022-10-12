using System;
using System.Collections;

namespace SnkFramework.Mvvm.View
{
    public class SnkUIShowTransition : SnkUITransition
    {
        private ISnkUILayer _uiLayer;
        public SnkUIShowTransition(ISnkUILayer uiLayer, ISnkWindowControllable window) : base(window)
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
                ISnkWindowControllable current = this.Window;
                //IManageable previous = (IManageable)this.manager.GetVisibleWindow(layer);
                ISnkWindowControllable previous = (ISnkWindowControllable)this._uiLayer.Current;
                if (previous != null)
                {
                    //Passivate the previous window
                    if (previous.mActivated)
                    {
                        yield return previous.Passivate(this.AnimationDisabled);;
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
                            yield return previous.DoHide(this.AnimationDisabled);
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
                    yield return current.DoShow(this.AnimationDisabled);
                }

                if (this._uiLayer.Activated && current.Equals(this._uiLayer.Current))
                {
                    yield return current.Activate(this.AnimationDisabled);
                }
            }
    }
}