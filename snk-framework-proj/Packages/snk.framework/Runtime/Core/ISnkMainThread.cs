using System.Threading;

namespace SnkFramework.Runtime.Core
{
    public interface ISnkMainThread
    {
        public int ThreadId { get; }
        public bool IsMainThread { get; }
        public SynchronizationContext Context { get; }
    }
}