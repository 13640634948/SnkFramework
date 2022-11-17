namespace SnkFramework.CloudRepository.Runtime.Base
{
    public abstract class SnkStorage : ISnkStorage
    {
        /*
        public abstract string mNameEn { get; }
        public abstract string mNameCn { get; }
        protected abstract string _EndPoint { get; }
        protected abstract string _AccessKeyId { get; }
        protected abstract string _AccessKeySecret { get; }
        protected abstract string _BucketName { get; }
        
        public SnkRepository()
        {
        }

        protected FileInfo _CheckFilePath(string fullPath)
        {
            FileInfo fileInfo = new FileInfo(fullPath);
            if (fileInfo?.Exists == true)
                fileInfo.Delete();
            if (fileInfo?.Directory?.Exists == false)
                fileInfo.Directory.Create();
            return fileInfo;
        }

        public abstract List<TupleData> LoadObjectsList(string prefix = "", string marker = "", int maxKeys = 100);
        public abstract List<string> DeleteObjects(List<string> objectNameList, bool quiet = true);
        public abstract bool PutObjects(string localPath, List<string> objectList);
        public abstract void PutObject(string key, FileStream fs, Action<long, long> onProgress);
        public abstract bool GetObjects(string localPath, List<string> objectList, int buffLenght = 1024 * 8);
        */
    }
}