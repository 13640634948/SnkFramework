using System.Collections;

namespace MvvmCross.Unity.Views.Transition
{
    public abstract class MvxUITransition : IMvxTransition
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
}