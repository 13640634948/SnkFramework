using System;
using System.Threading;

namespace SnkFramework.Mvvm.View
{
    internal class UIAsyncResult : IAsyncResult
    {
        private bool _isCompleted = false;
        private Exception _exception = null;

        public object AsyncState { get; }
        public WaitHandle AsyncWaitHandle { get; }
        public bool CompletedSynchronously { get; }

        public bool IsCompleted => _isCompleted;
        private Exception mException => _exception;

        public void SetException(Exception exception)
        {
            this._exception = exception;
            this.SetResult();
        }

        public void SetResult()
        {
            _isCompleted = true;
        }
    }
}