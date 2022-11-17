using System.Collections.Generic;

namespace SnkFramework.CloudRepository.Runtime.Storage
{
    public interface ISnkStorageDelete
    {
        public List<string> DeleteObjects(List<string> objectNameList);
    }
}