using System.Collections.Generic;

namespace SnkFramework.CloudRepository.Runtime.Base
{
    public abstract class SnkStorage : ISnkStorage
    {
        public virtual string StorageName => this.GetType().Name;
        public abstract List<SnkStorageObject> LoadObjectList(string path);
        public abstract List<string> DeleteObjects(List<string> objectNameList);
        public abstract bool PutObjects(string path, List<string> list);
        public abstract bool TakeObjects(string path, List<string> list);
    }
}