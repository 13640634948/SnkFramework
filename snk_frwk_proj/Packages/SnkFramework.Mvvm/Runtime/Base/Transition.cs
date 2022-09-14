using System;
using SnkFramework.Mvvm.View;

namespace SnkFramework.Mvvm.Base
{
    public abstract class Transition : ITransition
    {
        public abstract bool IsDone { get; }
        public abstract object WaitForDone();
        public abstract ITransition DisableAnimation(bool disabled);
        public abstract ITransition AtLayer(int layer);
        public abstract ITransition Overlay(Func<IWindow, IWindow, ActionType> policy);
        public abstract ITransition OnStart(Action callback);
        public abstract ITransition OnStateChanged(Action<IWindow, WindowState> callback);
        public abstract ITransition OnFinish(Action callback);
    }
}