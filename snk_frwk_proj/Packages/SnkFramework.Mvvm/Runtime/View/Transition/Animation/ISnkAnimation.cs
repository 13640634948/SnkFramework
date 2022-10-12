using System;

namespace SnkFramework.Mvvm.View
{
    public interface ISnkAnimation
    {
        public void Initialize(ISnkView view);

        ISnkAnimation OnStart(Action onStart);

        ISnkAnimation OnEnd(Action onEnd);

        ISnkAnimation Play();
    }
}