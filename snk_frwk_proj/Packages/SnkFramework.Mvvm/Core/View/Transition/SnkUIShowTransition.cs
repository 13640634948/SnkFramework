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

            protected virtual ActionType Overlay(ISnkWindow previous, ISnkWindow current) => ActionType.None;

            protected override IEnumerator DoTransition()
            {
                ISnkWindowControllable current = this.Window;
                ISnkWindowControllable previous = (ISnkWindowControllable) this._uiLayer.Current;
                if (previous != null)
                {
                    if (previous.mActivated)
                    {
                        yield return previous.Passivate(this.AnimationDisabled);
                    }

                    Func<ISnkWindow, ISnkWindow, ActionType> policy = this.OverlayPolicy ?? this.Overlay;
                    ActionType actionType = policy(previous, current);
                    switch (actionType)
                    {
                        case ActionType.Hide:
                            previous.DoHide(this.AnimationDisabled);
                            break;
                        case ActionType.Dismiss:
                            yield return previous.DoHide(this.AnimationDisabled);
                            previous.DoDismiss();
                            break;
                        default:
                            break;
                    }
                }

                if (!current.mVisibility)
                    yield return current.DoShow(this.AnimationDisabled);

                if (this._uiLayer.Activated && current.Equals(this._uiLayer.Current))
                    yield return current.Activate(this.AnimationDisabled);
            }
        }
    }
}