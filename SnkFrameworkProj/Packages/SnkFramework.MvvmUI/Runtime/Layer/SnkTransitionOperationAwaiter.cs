using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SnkFramework.Mvvm.Runtime.View
{
    public struct SnkTransitionOperationAwaiter : ISnkAwaiter, ICriticalNotifyCompletion
    {
        private SnkTransitionOperation _transitionOperation;
        private Action<SnkTransitionOperation> continuationAction;

        public SnkTransitionOperationAwaiter(SnkTransitionOperation asyncOperation)
        {
            this._transitionOperation = asyncOperation;
            this.continuationAction = null;
        }

        public bool IsCompleted => _transitionOperation.IsDone;

        public void GetResult()
        {
            Debug.Log("SnkTransitionOperation.GetResult:" + IsCompleted);
            //if (!IsCompleted)
            //    throw new Exception("The task is not finished yet");
        }

        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            this._transitionOperation.onCompleted = continuation;
        }
    }
}