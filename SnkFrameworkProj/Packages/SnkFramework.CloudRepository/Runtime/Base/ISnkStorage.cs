using System.Collections.Generic;

namespace SnkFramework.CloudRepository.Runtime.Base
{
    public class SnkStorageObject
    {
    }

    public interface ISnkStorage
    {
        public string StorageName { get; }

        public List<SnkStorageObject> LoadObjectList(string path);
        
        public List<string> DeleteObjects(List<string> objectNameList);

        public bool PutObjects(string path, List<string> list);
        
        public bool TakeObjects(string path, List<string> list);
    }
}