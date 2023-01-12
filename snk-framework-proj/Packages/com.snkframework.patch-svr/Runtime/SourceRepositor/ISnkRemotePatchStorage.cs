using System.Collections.Generic;
using System.Threading.Tasks;
using SnkFramework.PatchService.Runtime.Core;

namespace SnkFramework.PatchService.Runtime
{
    public interface ISnkRemoteSourceRepository : ISnkSourceRepository
    {
        public IEnumerable<VersionMeta> GetResVersionHistories();

        public Task<SnkDiffManifest> GetDiffManifest(int version);

        public Task TakeFileToLocal(string dirPath, string key, int resVersion);

    }
}