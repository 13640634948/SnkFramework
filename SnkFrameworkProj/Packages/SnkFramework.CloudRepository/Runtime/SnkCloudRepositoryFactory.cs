using SnkFramework.CloudRepository.Runtime.Base;
using SnkFramework.CloudRepository.Runtime.Storage;

namespace SnkFramework.CloudRepository.Runtime
{
    public class SnkCloudRepositoryFactory
    {
        public static ISnkCloudRepository Create( CLOUD_REPO_MODE mode )
        {
            switch (mode)
            {
                case CLOUD_REPO_MODE.alpha:
                    return Create<SnkRemoteIntranetStorage>();
                case CLOUD_REPO_MODE.debug:
                    return Create<SnkRemoteBucketStorage>();
                case CLOUD_REPO_MODE.release:
                    return Create<SnkRemoteCDNStorage>();
                default:
                    throw new System.PlatformNotSupportedException(mode + " is not supported on this platform. ");
            }
        } 
        
        public static ISnkCloudRepository Create<TRemoteStorage>( )
            where TRemoteStorage : class, ISnkRemoteStorage,new()
        {
            return Create<SnkLocalPersistentStorage, TRemoteStorage>();
        }
        
        public static ISnkCloudRepository Create<TLocalStorage, TRemoteStorage>( )
            where TLocalStorage : class, ISnkLocalStorage,new()
            where TRemoteStorage : class, ISnkRemoteStorage,new()
        {
            ISnkLocalStorage localStorage = new TLocalStorage();
            ISnkRemoteStorage remoteStorage = new TRemoteStorage();
            return new SnkCloudRepository(localStorage, remoteStorage);
        }
    }
}