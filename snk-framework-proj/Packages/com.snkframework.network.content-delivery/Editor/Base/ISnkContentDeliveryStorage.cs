using System.Collections.Generic;
using SnkFramework.Network.ContentDelivery.Runtime;

namespace SnkFramework.Network.ContentDelivery
{
    namespace Editor
    {
        public interface ISnkContentDeliveryStorage
        {            public string StorageName { get; }

            public STORAGE_STATE mStorageState { get; }

            public System.Exception mException { get; }

            public float mProgress { get; }

            public (string, long)[] LoadObjects(string prefixKey = null);
            public string[] TakeObjects(List<string> keyList, string localDirPath);
            public string[] PutObjects(List<string> keyList);
            public string[] DeleteObjects(List<string> keyList);
        }
    }
}