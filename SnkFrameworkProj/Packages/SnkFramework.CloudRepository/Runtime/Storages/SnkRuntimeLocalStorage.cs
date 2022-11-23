using System;
using System.Collections.Generic;
using SnkFramework.CloudRepository.Runtime.Base;

namespace SnkFramework.CloudRepository.Runtime
{
    namespace Storage
    {
        public class SnkRuntimeLocalStorage : SnkLocalStorage
        {
            protected override (string,long)[] doLoadObjects(string prefixKey = null)
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