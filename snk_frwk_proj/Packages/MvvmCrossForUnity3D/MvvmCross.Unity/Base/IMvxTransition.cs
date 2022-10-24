using System;
using System.Collections;
using System.Threading.Tasks;
using MvvmCross.Unity.Views;
using MvvmCross.Unity.Views.UGUI;
using UnityEngine;

namespace MvvmCross.Unity.Base
{
    public interface IMvxTransition
    {
        public IEnumerator Transit();
    }

    public interface IMvxUITransition : IMvxTransition
    {
    }

    public abstract class MvxUITransition : IMvxUITransition
    {
        protected void onStart()
        {
        }

        protected void onEnd()
        {
        }

        public IEnumerator Transit()
        {
            onStart();
            yield return onTransit();
            onEnd();
        }

        protected abstract IEnumerator onTransit();
    }

    public class AlphaUITransition : MvxUITransition
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