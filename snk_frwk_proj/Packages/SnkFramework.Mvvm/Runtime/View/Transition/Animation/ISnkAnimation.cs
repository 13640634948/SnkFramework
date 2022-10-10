using System;

namespace SampleDevelop.Test
{
    public interface ISnkAnimation
    {
        ISnkAnimation OnStart(Action onStart);

        ISnkAnimation OnEnd(Action onEnd);

        ISnkAnimation Play();
    }
}