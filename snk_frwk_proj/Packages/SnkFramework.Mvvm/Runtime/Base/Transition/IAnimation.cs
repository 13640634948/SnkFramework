using System;

namespace SnkFramework.Mvvm.Base

{
    public interface IAnimation
    {
        IAnimation OnStart(Action onStart);

        IAnimation OnEnd(Action onEnd);

        IAnimation Play();
    }
}