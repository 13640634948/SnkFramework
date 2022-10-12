using System;

namespace SnkFramework.Mvvm.View
{
    public abstract class SnkAnimation : ISnkAnimation
    {
        public abstract void Initialize(ISnkView view);
        public abstract ISnkAnimation OnStart(Action onStart);
        public abstract ISnkAnimation OnEnd(Action onEnd);
        public abstract ISnkAnimation Play();
    }
}