using System;

namespace SnkFramework.Mvvm.Runtime.View
{
    public class SnkTransitionOperation
    {
        public bool IsDone = false;

        public Action onCompleted;

        public ISnkAwaiter GetAwaiter()
            => new SnkTransitionOperationAwaiter(this);
    }
}