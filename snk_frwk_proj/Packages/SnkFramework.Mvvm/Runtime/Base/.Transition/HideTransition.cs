using System.Collections;
using SnkFramework.Mvvm.View;

namespace SnkFramework.Mvvm.Base
{
    internal class HideTransition : Transition
    {
        private IUILayer _layer;
        private bool _dismiss;

        public HideTransition(IUILayer layer, ISnkWindowControllable window, bool dismiss) : base(window)
        {
            this._layer = layer;
            this._dismiss = dismiss;
        }

        protected override IEnumerator DoTransition()
        {
            ISnkWindowControllable current = this.Window;
            if (this._layer.IndexOf(current) == 0)
            {
                if (current.mActivated)
                {
                    yield return current.Passivate(this.AnimationDisabled);
                }

                if (current.mVisibility)
                {
                    yield return current.DoHide(this.AnimationDisabled);
                }
            }
            else
            {
                if (current.mVisibility)
                {
                    yield return current.DoHide(this.AnimationDisabled);
                }
            }

            if (_dismiss)
            {
                current.DoDismiss();
            }
        }
    }
}