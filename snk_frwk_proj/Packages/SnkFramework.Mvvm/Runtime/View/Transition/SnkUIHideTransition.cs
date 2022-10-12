using System;
using System.Collections;

namespace SnkFramework.Mvvm.View
{
    public class SnkUIHideTransition : SnkUITransition
    {
        private bool _dismiss;
        private ISnkUILayer _uiLayer;

        public SnkUIHideTransition(ISnkUILayer uiLayer, ISnkWindowControllable window, bool dismiss) : base(window)
        {
            this._uiLayer = uiLayer;
            this._dismiss = dismiss;
        }

        protected override IEnumerator DoTransition()
        {
            ISnkWindowControllable current = this.Window;
            if (this._uiLayer.IndexOf(current) == 0 && current.mActivated)
            {
                yield return current.Passivate(this.AnimationDisabled);
            }

            if (current.mVisibility)
            {
                yield return current.DoHide(this.AnimationDisabled);
            }

            if (_dismiss)
                current.DoDismiss();
        }
    }
}