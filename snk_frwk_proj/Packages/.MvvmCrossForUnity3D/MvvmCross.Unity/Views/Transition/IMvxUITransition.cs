using System.Collections;

namespace MvvmCross.Unity.Views.Transition
{
    public interface IMvxTransition
    {
        public IEnumerator Transit();

    }
}