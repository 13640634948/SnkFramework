using System.Collections;
using SnkFramework.Mvvm.View;

namespace SnkFramework.Mvvm.Base
{
    internal class ShowTransition : Transition
    {
        private IUILayer _layer;
        public ShowTransition(IUILayer layer, ISnkWindowControllable window) : base(window)
        {
            this._layer = layer;
        }

        protected override IEnumerator DoTransition()
        {
            ISnkWindowControllable current = this.Window;
            
            if (!current.mVisibility)
            {
                yield return current.DoShow(this.AnimationDisabled);
            }

            if (this._layer.mActivated && current.Equals(this._layer.mCurrent))
            {
                yield return current.Activate(this.AnimationDisabled);
            }
        }
    }
}