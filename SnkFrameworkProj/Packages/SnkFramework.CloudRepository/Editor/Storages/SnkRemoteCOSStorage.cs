using System.Collections.Generic;
using SnkFramework.CloudRepository.Runtime.Base;

namespace SnkFramework.CloudRepository.Editor
{
    namespace Storage
    {
        /// <summary>
        /// 腾讯云COS(Cloud Object Storage)
        /// </summary>
        public class SnkRemoteCOSStorage : SnkRemoteStorage
        {
            public SnkRemoteCOSStorage(SnkRemoteStorageSettings settings) : base(settings)
            {
            }

            public override List<SnkStorageObject> LoadObjectList(string path)
            {
                throw new System.NotImplementedException();
            }

            public override List<string> DeleteObjects(List<string> objectNameList)
            {
                throw new System.NotImplementedException();
            }

            public override bool PutObjects(string path, List<string> list)
            {
                throw new System.NotImplementedException();
            }

            public override bool TakeObjects(string path, List<string> list)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}