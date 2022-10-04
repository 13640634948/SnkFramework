using System.Collections;
using SnkFramework.Mvvm.View;
using UnityEngine;

namespace SnkFramework.Mvvm.Base
{
    public class AlphaAnimation : UIAnimation
    {
        [Range(0f, 1f)] public float from = 1f;
        [Range(0f, 1f)] public float to = 1f;

        public float duration = 2f;

        private IView _view;

        public void Init(IView view)
        {
            this._view = view;
            switch (this.AnimType)
            {
                case ANIM_TYPE.enter_anim:
                    this._view.mEnterAnimation = this;
                    break;
                case ANIM_TYPE.exit_anim:
                    this._view.mExitAnimation = this;
                    break;
                case ANIM_TYPE.activation_anim:
                    if (this._view is IWindowView)
                        (this._view as IWindowView).mActivationAnimation = this;
                    break;
                case ANIM_TYPE.passivation_anim:
                    if (this._view is IWindowView)
                        (this._view as IWindowView).mPassivationAnimation = this;
                    break;
            }

            if (this.AnimType == ANIM_TYPE.activation_anim ||
                this.AnimType == ANIM_TYPE.enter_anim)
            {
                this._view.mCanvasGroup.alpha = from;
            }
        }

        public override IAnimation Play()
        {
            ////use the DoTween
            //this.view.CanvasGroup.DOFade (this.to, this.duration).OnStart (this.OnStart).OnComplete (this.OnEnd).Play ();		

            this.StartCoroutine(DoPlay());
            return this;
        }

        IEnumerator DoPlay()
        {
            this.OnStart();

            var delta = (to - from) / duration;
            var alpha = from;
            this._view.Alpha = alpha;
            if (delta > 0f)
            {
                while (alpha < to)
                {
                    alpha += delta * Time.deltaTime;
                    if (alpha > to)
                    {
                        alpha = to;
                    }

                    this._view.Alpha = alpha;
                    yield return null;
                }
            }
            else
            {
                while (alpha > to)
                {
                    alpha += delta * Time.deltaTime;
                    if (alpha < to)
                    {
                        alpha = to;
                    }

                    this._view.Alpha = alpha;
                    yield return null;
                }
            }

            this.OnEnd();
        }
    }
}