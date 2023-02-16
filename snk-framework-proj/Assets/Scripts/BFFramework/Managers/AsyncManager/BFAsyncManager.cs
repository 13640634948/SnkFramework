using SnkFramework.NuGet.Asynchronous;

namespace BFFramework.Runtime
{
    namespace Managers
    {
        public class BFAsyncManager : BFManager<BFAsyncManager>, IBFAsyncManager
        {
            private ISnkAsyncExecutor _executor;

            public BFAsyncManager()
            {
                SnkAsyncExecutor.RegiestAsyncExecutor<BFAsyncExecutor>();
            }
        }
    }
}