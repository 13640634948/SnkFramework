using System.Collections.Generic;
using SnkFramework.Network.FileStorage.Runtime;

namespace SnkFramework.Network.FileStorage
{
    namespace Editor
    {
        public abstract class SnkEditorStorage<T> : SnkRuntimeStorage, ISnkEditorStorage
            where T : SnkStorageSettings, new()
        {
            protected readonly T settings = SnkStorageSettings.Load<T>();
            protected virtual string mBucketName => settings.mBucketName;
            protected virtual string mEndPoint => settings.mEndPoint;
            protected virtual string mAccessKeyId => settings.mAccessKeyId;
            protected virtual string mAccessKeySecret => settings.mAccessKeySecret;
            protected virtual bool mIsQuietDelete => settings.mIsQuietDelete;
          
            protected virtual string[] doPutObjects(List<string> keyList)
                => throw new System.NotImplementedException();

            protected virtual string[] doDeleteObjects(List<string> keyList)
                => throw new System.NotImplementedException();

            public string[] PutObjects(List<string> keyList)
            {
                if (this.mStorageState != STORAGE_STATE.none)
                    return default;
                mStorageState = STORAGE_STATE.putting;
                this.mProgress = 0;

                var result = doPutObjects(keyList);

                this.mProgress = 1.0f;
                mStorageState = STORAGE_STATE.none;
                return result;
            }

            public string[] DeleteObjects(List<string> keyList)
            {
                if (this.mStorageState != STORAGE_STATE.none)
                    return default;
                mStorageState = STORAGE_STATE.deleting;
                this.mProgress = 0;

                var result = doDeleteObjects(keyList);

                this.mProgress = 1.0f;
                mStorageState = STORAGE_STATE.none;
                return result;
            }
        }
    }
}