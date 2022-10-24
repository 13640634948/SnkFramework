using System;

namespace MvvmCross.Unity.Base
{
    public interface IMvxTransition
    {
        
        IMvxTransition OnStart(Action onStart);

        IMvxTransition OnEnd(Action onEnd);

        IMvxTransition Play();
    }
}