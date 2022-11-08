using System.Runtime.CompilerServices;

namespace SnkFramework.Mvvm.Runtime.View
{
    public interface ISnkAwaiter : INotifyCompletion
    {
        public bool IsCompleted { get; }
        public void GetResult();
    }
    
    
    public interface ISnkAwaiter<T> : INotifyCompletion
    {
        public bool IsCompleted { get; }
        public T GetResult();
    }
}