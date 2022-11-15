using System.Collections.Generic;

namespace SnkFramework.PatchRepository.Runtime.Base
{
    [System.Serializable]
    public class PatcherManifest
    {
        public int lastVersion;
        public string lastPatcherName;
        public List<Patcher> patcherList = new List<Patcher>();
    }
}