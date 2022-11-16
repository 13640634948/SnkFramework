using SnkFramework.CloudRepository.Runtime.LocalRepository;
using SnkFramework.CloudRepository.Runtime.RemoteRepository;

namespace SnkFramework.CloudRepository.Runtime
{
    public abstract class SnkCloudRepository<TLocalRepo, TRemoteRepo> : ISnkCloudRepository
        where TLocalRepo : class, ISnkLocalRepository
        where TRemoteRepo : class, ISnkRemoteRepository
    {
        public TLocalRepo mLocalRepo { get; }
        public TRemoteRepo mMainRemoteRepo { get; }
        public ISnkRemoteRepository[] mRepositories { get; }

        public void AddSpareRepository(ISnkRemoteRepository remoteRepo)
        {
            
        }
        
        public void LocalSyncToMainRemote()
        {
            
        }

        public void MainRemoteSyncToLocal()
        {
            
        }

        public void MainRemoteSyncToSpares()
        {
            
        }
        
        public void SpareSyncToMainRemote(int spareIndex)
        {
            
        }

        public void GenLocalChangedListWithMainRemote()
        {
            
        }

    }
}