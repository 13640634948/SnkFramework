using System.Collections;
using MvvmCross.Unity.Views.UGUI;
using UnityEngine;

namespace MvvmCross.Unity.Views.Transition
{
    public class MvxAlphaUITransition : MvxUITransition
    {
        public IMvxUGUINode view;
        public float from = 0;
        public float to = 1;
        public float duration = 2f;

        protected override IEnumerator onTransit()
        {
            
            var delta = (to - from) / duration;
            var alpha = from;
            this.view.Alpha = alpha;
            if (delta > 0f)
            {
                while (alpha < to)
                {
                    alpha += delta * Time.deltaTime;
                    if (alpha > to)
                    {
                        alpha = to;
                    }
                    this.view.Alpha = alpha;
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
                    this.view.Alpha = alpha;
                    yield return null;
                }
            }

        }
    }
}