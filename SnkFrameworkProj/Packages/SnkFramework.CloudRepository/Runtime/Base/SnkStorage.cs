using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnkFramework.CloudRepository.Runtime.Base
{
    public abstract class SnkStorage : ISnkStorage
    {
        public virtual string StorageName => this.GetType().Name;

        public abstract List<SnkStorageObject> LoadObjectList(string path);

        public abstract void TakeObjects(string key, string localPath, SnkStorageTakeOperation takeOperation, int buffSize = 2097152);
    }
}