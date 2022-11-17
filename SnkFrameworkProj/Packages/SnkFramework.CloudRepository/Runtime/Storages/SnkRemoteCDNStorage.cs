using System;
using System.Collections.Generic;
using SnkFramework.CloudRepository.Runtime.Base;

namespace SnkFramework.CloudRepository.Runtime
{
    namespace Storage
    {
        public class SnkRemoteCDNStorage : SnkRemoteStorage
        {
            public SnkRemoteCDNStorage(SnkRemoteStorageSettings settings) : base(settings)
            {
            }
            
            public override List<SnkStorageObject> LoadObjectList(string path)
            {
                throw new NotImplementedException();
            }

            public override List<string> DeleteObjects(List<string> objectNameList)
                => throw new System.PlatformNotSupportedException("DeleteObjects is not supported on this platform. ");
            

            public override bool PutObjects(string path, List<string> list)
                => throw new System.PlatformNotSupportedException("PutObjects is not supported on this platform. ");

            public override bool TakeObjects(string path, List<string> list)
            {
                throw new NotImplementedException();
            }
        }
    }
}