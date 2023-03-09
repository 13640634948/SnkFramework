using System.Threading;

namespace SnkFramework.Runtime.Core
{
    public class SnkMainThread : ISnkMainThread
    {
        public int ThreadId { get; protected set; }
        public bool IsMainThread => Context == SynchronizationContext.Current;
        public SynchronizationContext Context { get; }
        
        public SnkMainThread()
        {
            Context = SynchronizationContext.Current;
        }
    }
}