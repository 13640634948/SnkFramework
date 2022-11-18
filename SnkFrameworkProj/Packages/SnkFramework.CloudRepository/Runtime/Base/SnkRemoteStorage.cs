
using System.IO;

namespace SnkFramework.CloudRepository.Runtime.Base
{
    public abstract class SnkRemoteStorage : SnkStorage, ISnkRemoteStorage
    {
        protected void CleanPath(string fullPath)
        {
            FileInfo fileInfo = new FileInfo(fullPath);
            if (fileInfo.Exists)
                fileInfo.Delete();
            if (fileInfo.Directory!.Exists == false)
                fileInfo.Directory.Create();
        }
    }
}