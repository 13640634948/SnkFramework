using System.Collections.Generic;

namespace SnkFramework.PatchRepository.Runtime.Base
{
    [System.Serializable]
    public class DiffManifest
    {
        public List<SourceInfo> addList;
        public List<string> delList;
    }
}