using SnkFramework.CloudRepository.Runtime.Base;

namespace SnkFramework.CloudRepository.Runtime
{
    public class SnkCloudRepository : ISnkCloudRepository
    {
        protected ISnkLocalStorage localStorage { get; }
        protected ISnkRemoteStorage remoteStorage { get; }
        
        internal SnkCloudRepository(ISnkLocalStorage localStorage, ISnkRemoteStorage remoteStorage)
        {
            this.localStorage = localStorage;
            this.remoteStorage = remoteStorage;
        }
    }
}
