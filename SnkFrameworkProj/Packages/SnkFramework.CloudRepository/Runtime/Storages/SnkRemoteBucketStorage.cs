using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnkFramework.CloudRepository.Runtime.Base;

namespace SnkFramework.CloudRepository.Runtime
{
    namespace Storage
    {
        public class SnkRemoteBucketStorage : SnkRemoteStorage
        {
            public override List<SnkStorageObject> LoadObjectList(string path)
            {
                throw new NotImplementedException();
            }

            public override void TakeObjects(string key, string localPath, SnkStorageTakeOperation takeOperation, int buffSize = 2097152)
            {
                throw new NotImplementedException();
            }

        }
    }
}