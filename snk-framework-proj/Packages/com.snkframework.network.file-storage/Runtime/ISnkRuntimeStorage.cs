using System.Collections.Generic;

namespace SnkFramework.Network.FileStorage
{
    namespace Runtime
    {
        public interface ISnkRuntimeStorage
        {
            public string StorageName { get; }

            public STORAGE_STATE mStorageState { get; }

            public System.Exception mException { get; }

            public float mProgress { get; }

            public (string, long)[] LoadObjects(string prefixKey = null);
            public string[] TakeObjects(List<string> keyList, string localDirPath);
        }
    }
}