using System;
using System.Collections.Generic;
using System.IO;
using SnkFramework.Network.ContentDelivery.Runtime;

namespace SnkFramework.Network.ContentDelivery
{
    namespace Editor
    {
        public abstract class SnkContentDeliveryStorage<T> : ISnkContentDeliveryStorage
            where T : SnkStorageSettings, new()
        {
            protected readonly T settings = SnkStorageSettings.Load<T>();
            protected virtual string mBucketName => settings.mBucketName;
            protected virtual string mEndPoint => settings.mEndPoint;
            protected virtual string mAccessKeyId => settings.mAccessKeyId;
            protected virtual string mAccessKeySecret => settings.mAccessKeySecret;
            protected virtual bool mIsQuietDelete => settings.mIsQuietDelete;

            
            public virtual string StorageName => this.GetType().Name;
            public STORAGE_STATE mStorageState { get; protected set; }
            public Exception mException { get; protected set; }
            public float mProgress { get; protected set; }

            protected void setException(Exception exception)
            {
                this.mException = exception;
            }

            protected void updateProgress(float progress)
            {
                this.mProgress = progress;
            }

            protected void EnsurePathExists(string fullPath)
            {
                FileInfo fileInfo = new FileInfo(fullPath);
                if (fileInfo.Exists)
                    fileInfo.Delete();
                if (fileInfo.Directory!.Exists == false)
                    fileInfo.Directory.Create();
            }

            protected virtual (string, long)[] doLoadObjects(string prefixKey = null)
                => throw new System.NotImplementedException();

            protected virtual string[] doTakeObjects(List<string> keyList, string localDirPath)
                => throw new System.NotImplementedException();

            protected virtual string[] doPutObjects(List<string> keyList)
                => throw new System.NotImplementedException();

            protected virtual string[] doDeleteObjects(List<string> keyList)
                => throw new System.NotImplementedException();


            public (string, long)[] LoadObjects(string prefixKey = null)
            {
                if (this.mStorageState != STORAGE_STATE.none)
                    return default;
                mStorageState = STORAGE_STATE.loading;
                this.mProgress = 0;

                var result = doLoadObjects(prefixKey);

                this.mProgress = 1.0f;
                mStorageState = STORAGE_STATE.none;
                return result;
            }

            public string[] TakeObjects(List<string> keyList, string localDirPath)
            {
                if (this.mStorageState != STORAGE_STATE.none)
                    return default;
                mStorageState = STORAGE_STATE.takeing;
                this.mProgress = 0;

                var result = doTakeObjects(keyList, localDirPath);

                this.mProgress = 1.0f;
                mStorageState = STORAGE_STATE.none;
                return result;
            }
            
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