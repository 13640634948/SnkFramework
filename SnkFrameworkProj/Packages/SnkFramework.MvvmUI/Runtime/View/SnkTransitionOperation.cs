using System;
using SnkFramework.Mvvm.Runtime.Base;
using SnkFramework.Mvvm.Runtime.Layer;

namespace SnkFramework.Mvvm.Runtime
{
    namespace View
    {
        public class SnkTransitionOperation
        {
            public bool IsDone = false;

            public Action onCompleted;

            public ISnkAwaiter GetAwaiter()
                => new SnkTransitionOperationAwaiter(this);
        }

    }
}