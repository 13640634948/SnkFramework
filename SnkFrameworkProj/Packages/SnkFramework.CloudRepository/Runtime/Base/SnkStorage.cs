using System.Collections.Generic;

namespace SnkFramework.CloudRepository.Runtime.Base
{
    public abstract class SnkStorage : ISnkStorage
    {
        public virtual string StorageName => this.GetType().Name;

        public abstract List<SnkStorageObject> LoadObjectList(string path);

        public abstract bool TakeObjects(string path, List<string> list);
        
    }
}