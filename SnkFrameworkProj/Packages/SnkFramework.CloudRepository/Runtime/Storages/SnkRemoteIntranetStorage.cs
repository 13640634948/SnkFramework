using System;
using System.Collections.Generic;
using SnkFramework.CloudRepository.Runtime.Base;

namespace SnkFramework.CloudRepository.Runtime
{
    namespace Storage
    {
        public class SnkRemoteIntranetStorage : SnkRemoteStorage
        {
            public override List<SnkStorageObject> LoadObjectList(string path)
            {
                throw new NotImplementedException();
            }

            public override bool TakeObjects(string path, List<string> list)
            {
                throw new NotImplementedException();
            }
        }
    }
}