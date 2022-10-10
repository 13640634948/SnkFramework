using System;
using SnkFramework.Mvvm.View;

namespace SnkFramework.Mvvm.Base

{
    public interface IAnimation
    {
        public void Initialize(IView view);
        
        IAnimation OnStart(Action onStart);

        IAnimation OnEnd(Action onEnd);

        void Play();
    }
}