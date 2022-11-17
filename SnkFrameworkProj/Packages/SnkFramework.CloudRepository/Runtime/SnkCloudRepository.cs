using SnkFramework.CloudRepository.Runtime.Base;
using SnkFramework.CloudRepository.Runtime.Storage;

namespace SnkFramework.CloudRepository.Runtime
{
    public class SnkCloudRepository : ISnkCloudRepository
    {
        private ISnkLocalStorage localStorage { get; }
        private ISnkRemoteStorage remoteStorage { get; }

        public static ISnkCloudRepository Create( CLOUD_REPO_MODE mode )
        {
            ISnkLocalStorage localStorage = new SnkLocalPersistentStorage();
            ISnkRemoteStorage remoteStorage = mode switch
            {
                CLOUD_REPO_MODE.alpha => new SnkRemoteIntranetStorage(),
                CLOUD_REPO_MODE.debug => new SnkRemoteBucketStorage(),
                CLOUD_REPO_MODE.release => new SnkRemoteCDNStorage(),
                _ =>
                    throw new System.PlatformNotSupportedException(mode + " is not supported on this platform. ")
            };
            return new SnkCloudRepository(localStorage, remoteStorage);
        }
        
        private SnkCloudRepository(ISnkLocalStorage localStorage, ISnkRemoteStorage remoteStorage)
        {
            this.localStorage = localStorage;
            this.remoteStorage = remoteStorage;
        }
    }
}
