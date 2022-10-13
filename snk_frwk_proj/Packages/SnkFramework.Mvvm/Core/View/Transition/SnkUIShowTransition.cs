using System;
using System.Collections;

namespace SnkFramework.Mvvm.Core
{
    namespace View
    {
        public class SnkUIShowTransition : SnkUITransition
        {
            private ISnkUILayer _uiLayer;

            public SnkUIShowTransition(ISnkUILayer uiLayer, ISnkWindowControllable window) : base(window)
            {
                this._uiLayer = uiLayer;
            }

            protected virtual ACT_TYPE Overlay(ISnkWindow previous, ISnkWindow current)
            {
                return ACT_TYPE.None;
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
                ISnkWindowControllable previous = (ISnkWindowControllable) this._uiLayer.Current;
                if (previous != null)
                {
                    //Passivate the previous window
                    if (previous.mActivated)
                    {
                        yield return previous.Passivate(this.AnimationDisabled);
                        ;
                    }

                    Func<ISnkWindow, ISnkWindow, ACT_TYPE> policy = this.OverlayPolicy;
                    if (policy == null)
                        policy = this.Overlay;
                    ACT_TYPE actionType = policy(previous, current);
                    switch (actionType)
                    {
                        case ACT_TYPE.Hide:
                            previous.DoHide(this.AnimationDisabled);
                            break;
                        case ACT_TYPE.Dismiss:
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
}