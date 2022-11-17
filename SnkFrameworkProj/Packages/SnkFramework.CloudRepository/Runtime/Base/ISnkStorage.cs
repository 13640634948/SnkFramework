using System.Collections.Generic;

namespace SnkFramework.CloudRepository.Runtime.Base
{
    public interface ISnkStorage : ISnkStorageTake
    {
        public string StorageName { get; }

        public List<SnkStorageObject> LoadObjectList(string path);
    }
}