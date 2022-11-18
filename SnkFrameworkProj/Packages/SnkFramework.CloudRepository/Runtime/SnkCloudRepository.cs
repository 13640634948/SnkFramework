using System.Collections.Generic;
using System.Threading.Tasks;
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

        public Task<string> TakeObject(string key)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> TakeObject<T>(string key) where T : ISnkStorageValueOf<T>
        {
            throw new System.NotImplementedException();
        }

        public Task<string> TakeObjectToFile(string key)
        {
            throw new System.NotImplementedException();
        }

        public bool PreviewRemoteSyncToLocal(string key, ref List<string> localAddList, ref List<string> localDelList)
        {
            throw new System.NotImplementedException();
        }

        public bool ExecuteSync(string key, ref float progress)
        {
            throw new System.NotImplementedException();
        }
    }
}
