using System;

namespace SnkFramework.CloudRepository.Runtime.Base
{
    public class SnkStorageTakeOperation
    {
        public float progress;
        public Exception exception;
        public bool isCompleted;

        public void SetResult()
        {
            this.isCompleted = true;
        }

        public void SetException(Exception exception)
        {
            this.exception = exception;
            this.isCompleted = false;
        }
    }

    public interface ISnkStorageTake
    {
        public void TakeObjects(string key, string localPath, SnkStorageTakeOperation takeOperation, int buffSize = 1024 * 1024 * 2);
    }
}