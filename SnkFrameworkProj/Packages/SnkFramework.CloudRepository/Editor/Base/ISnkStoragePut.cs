using System.Collections.Generic;

namespace SnkFramework.CloudRepository.Runtime.Storage
{
    public interface ISnkStoragePut
    {
        public bool PutObjects(string path, List<string> list);
    }
}