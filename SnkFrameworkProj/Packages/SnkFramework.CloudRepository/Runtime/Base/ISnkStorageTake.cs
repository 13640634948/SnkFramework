using System.Collections.Generic;

namespace SnkFramework.CloudRepository.Runtime.Base
{
    public interface ISnkStorageTake
    {
        public bool TakeObjects(string path, List<string> list);
    }
}