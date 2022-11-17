using System.Collections.Generic;
using SnkFramework.CloudRepository.Runtime.Base;
using SnkFramework.CloudRepository.Runtime.Storage;

namespace SnkFramework.CloudRepository.Editor
{
    namespace Storage
    {
        /// <summary>
        /// 阿里云OSS(Object Storage Service)
        /// </summary>
        public class SnkRemoteOSSStorage  : SnkRemoteStorage, ISnkStorageDelete, ISnkStoragePut
        {
            public SnkRemoteOSSStorage(SnkRemoteStorageSettings settings)
            {
            }

            public override List<SnkStorageObject> LoadObjectList(string path)
            {
                throw new System.NotImplementedException();
            }

            public override bool TakeObjects(string path, List<string> list)
            {
                throw new System.NotImplementedException();
            }

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