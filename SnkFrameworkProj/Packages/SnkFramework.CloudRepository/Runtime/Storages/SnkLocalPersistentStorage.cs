using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnkFramework.CloudRepository.Runtime.Base;

namespace SnkFramework.CloudRepository.Runtime
{
    namespace Storage
    {
        public class SnkLocalPersistentStorage : SnkLocalStorage
        {
            public override List<SnkStorageObject> LoadObjectList(string path)
            {
                throw new NotImplementedException();
            }

            public override void TakeObject(string key, string localPath, SnkStorageTakeOperation takeOperation, int buffSize = 2097152)
            {
                throw new NotImplementedException();
            }
        }
    }
}