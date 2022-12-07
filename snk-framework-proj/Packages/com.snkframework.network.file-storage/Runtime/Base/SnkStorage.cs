using System;
using System.Collections.Generic;
using System.IO;

namespace SnkFramework.Network.FileStorage
{
    namespace Runtime.Base
    {
        public abstract class SnkStorage : ISnkStorage
        {
            public virtual string StorageName => this.GetType().Name;
            public STORAGE_STATE mStorageState { get; private set; }
            public Exception mException { get; private set; }
            public float mProgress { get; private set; }

            protected void setException(Exception exception)
            {
                this.mException = exception;
            }

            protected void updateProgress(float progress)
            {
                this.mProgress = progress;
            }

            protected abstract (string, long)[] doLoadObjects(string prefixKey = null);
            protected abstract string[] doTakeObjects(List<string> keyList, string localDirPath);
            protected abstract string[] doPutObjects(List<string> keyList);
            protected abstract string[] doDeleteObjects(List<string> keyList);

            protected void EnsurePathExists(string fullPath)
            {
                FileInfo fileInfo = new FileInfo(fullPath);
                if (fileInfo.Exists)
                    fileInfo.Delete();
                if (fileInfo.Directory!.Exists == false)
                    fileInfo.Directory.Create();
            }

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