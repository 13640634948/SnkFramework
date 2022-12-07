using System;
using System.Collections.Generic;
using SnkFramework.Network.FileStorage.Runtime.Base;


namespace SnkFramework.Network.FileStorage
{
    namespace Runtime
    {
        public class SnkRuntimeRemoteStorage : SnkRemoteStorage
        {
            protected override (string, long)[] doLoadObjects(string prefixKey = null)
            {
                throw new NotImplementedException();
            }

            protected override string[] doTakeObjects(List<string> keyList, string localDirPath)
            {
                throw new NotImplementedException();
            }

            protected override string[] doPutObjects(List<string> keyList)
            {
                throw new NotImplementedException();
            }

            protected override string[] doDeleteObjects(List<string> keyList)
            {
                throw new NotImplementedException();
            }
        }
    }
}