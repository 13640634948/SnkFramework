using System;
using System.Threading;
using MvvmCross.Base;

namespace MvvmCross.Unity.Views
{
    public class MvxUnityThreadAsyncDispatcher : MvxMainThreadAsyncDispatcher
    {
        private SynchronizationContext _synchronizationContext;

        public override bool IsOnMainThread => _synchronizationContext == SynchronizationContext.Current;

        public MvxUnityThreadAsyncDispatcher(SynchronizationContext synchronizationContext)
        {
            _synchronizationContext = synchronizationContext;
        }
        
        public override bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
                ExceptionMaskedAction(action, maskExceptions);
            else
            {
                _synchronizationContext.Post(ignored =>
                {
                    ExceptionMaskedAction(action, maskExceptions);
                }, null);
            }
            return true;
        }

    }
}