using System;

namespace SampleDevelop.Test
{
    public abstract class SnkAnimation : ISnkAnimation
    {
        public abstract ISnkAnimation OnStart(Action onStart);
        public abstract ISnkAnimation OnEnd(Action onEnd);
        public abstract ISnkAnimation Play();
    }
}