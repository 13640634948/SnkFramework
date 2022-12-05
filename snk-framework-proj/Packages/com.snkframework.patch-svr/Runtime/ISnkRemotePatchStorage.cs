using System.Collections.Generic;
using SnkFramework.PatchBuilder.Runtime.Core;

namespace SnkFramework.PatchService.Runtime
{
    public interface ISnkRemotePatchStorage : ISnkPatchStorage
    {
        public List<int> GetResVersionHistories();

        public List<SnkSourceInfo> GetSourceInfoList(int version);
        public SnkDiffManifest GetDiffManifest(int version);

        public void TakeFileToLocal(string dirPath, string key);
        
    }
}