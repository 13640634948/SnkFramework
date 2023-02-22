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

            protected OnProgressCallback onProgressCallbackHandle;

            public virtual string StorageName => this.GetType().Name;
            public STORAGE_STATE mStorageState { get; protected set; }
            public Exception mException { get; protected set; }
            //public float mProgress { get; protected set; }

            protected void setException(Exception exception)
            {
                this.mException = exception;
            }

            public void SetProgressCallbackHandle(OnProgressCallback progressCallback)
            {
                this.onProgressCallbackHandle = progressCallback;
            }
/*
            protected void updateProgress(float progress)
            {
                this.mProgress = progress;
            }
            */

            protected void EnsurePathExists(string fullPath)
            {
                FileInfo fileInfo = new FileInfo(fullPath);
                if (fileInfo.Exists)
                    fileInfo.Delete();
                if (fileInfo.Directory!.Exists == false)
                    fileInfo.Directory.Create();
            }

            protected abstract IEnumerable<(string, long)> doLoadObjects(string prefixKey = null);

            protected abstract IEnumerable<byte> doTakeObject(string key);
            protected abstract string[] doTakeObjects(List<string> keyList, string localDirPath);

            protected abstract string[] doPutObjects(List<string> keyList);

            protected abstract string[] doDeleteObjects(List<string> keyList);

            public IEnumerable<(string, long)> LoadObjects(string prefixKey = null)
            {
                if (this.mStorageState != STORAGE_STATE.none)
                    return default;
                mStorageState = STORAGE_STATE.loading;
                var result = doLoadObjects(prefixKey);
                mStorageState = STORAGE_STATE.none;
                return result;
            }

            public IEnumerable<byte> TaskObject(string key)
            {
                if (this.mStorageState != STORAGE_STATE.none)
                    return default;
                mStorageState = STORAGE_STATE.takeing;
                var result = doTakeObject(key);
                mStorageState = STORAGE_STATE.none;
                return result;
            }

            public string[] TakeObjects(List<string> keyList, string localDirPath)
            {
                if (this.mStorageState != STORAGE_STATE.none)
                    return default;
                mStorageState = STORAGE_STATE.takeing;
                var result = doTakeObjects(keyList, localDirPath);
                mStorageState = STORAGE_STATE.none;
                return result;
            }

            public string[] PutObjects(List<string> keyList)
            {
                if (this.mStorageState != STORAGE_STATE.none)
                    return default;
                mStorageState = STORAGE_STATE.putting;
                var result = doPutObjects(keyList);
                mStorageState = STORAGE_STATE.none;
                return result;
            }

            public string[] DeleteObjects(List<string> keyList)
            {
                if (this.mStorageState != STORAGE_STATE.none)
                    return default;
                mStorageState = STORAGE_STATE.deleting;
                var result = doDeleteObjects(keyList);
                mStorageState = STORAGE_STATE.none;
                return result;
            }
        }
    }
}