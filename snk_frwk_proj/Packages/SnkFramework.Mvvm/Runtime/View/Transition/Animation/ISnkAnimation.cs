using System;

namespace SnkFramework.Mvvm.View
{
    public interface ISnkAnimation
    {
        ISnkAnimation OnStart(Action onStart);

        ISnkAnimation OnEnd(Action onEnd);

        ISnkAnimation Play();
    }
}