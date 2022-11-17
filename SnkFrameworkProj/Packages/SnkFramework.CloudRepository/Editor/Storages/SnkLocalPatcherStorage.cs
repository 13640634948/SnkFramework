using System.Collections.Generic;
using SnkFramework.CloudRepository.Runtime.Storage;

namespace SnkFramework.CloudRepository.Editor
{
    namespace Storage
    {
        public class SnkLocalPatcherStorage : SnkLocalPersistentStorage, ISnkStorageDelete, ISnkStoragePut
        {
            public List<string> DeleteObjects(List<string> objectNameList)
            {
                throw new System.NotImplementedException();
            }

            public bool PutObjects(string path, List<string> list)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}