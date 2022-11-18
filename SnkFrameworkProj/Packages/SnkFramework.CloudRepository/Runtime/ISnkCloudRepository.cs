using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnkFramework.CloudRepository.Runtime
{
    public interface ISnkCloudRepository
    {
        public Task<string> TakeObject(string key);
        
        public Task<T> TakeObject<T>(string key) where T : ISnkStorageValueOf<T>;
        
        public Task<string> TakeObjectToFile(string key);

        public bool PreviewRemoteSyncToLocal(string key, ref List<string> localAddList, ref List<string> localDelList);

        public bool ExecuteSync(string key, ref float progress);

    }
}