using System.Collections;
using SampleDevelop.Test;
using SnkFramework.Mvvm.View;
using UnityEngine;

namespace SnkFramework.Mvvm.Base
{
    /*
    public class AlphaAnimation : UIAnimation
    {
        [Range(0f, 1f)] public float from = 1f;
        [Range(0f, 1f)] public float to = 1f;

        public float duration = 2f;

        public override void Initialize(ISnkView view)
        {
            base.Initialize(view);

            if (this.AnimType == ANIM_TYPE.activation_anim ||
                this.AnimType == ANIM_TYPE.enter_anim)
            {
                base.mView.mCanvasGroup.alpha = from;
            }
        }

        protected override IEnumerator DoPlay()
        {
            this.OnStart();

            var delta = (to - from) / duration;
            var alpha = from;
            this.mView.Alpha = alpha;
            if (delta > 0f)
            {
                while (alpha < to)
                {
                    alpha += delta * Time.deltaTime;
                    if (alpha > to)
                    {
                        alpha = to;
                    }

                    base.mView.Alpha = alpha;
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

                    base.mView.Alpha = alpha;
                    yield return null;
                }
            }

            this.OnEnd();
        }
    }
    */
}