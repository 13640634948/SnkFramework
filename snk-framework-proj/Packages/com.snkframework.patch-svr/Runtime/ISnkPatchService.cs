using System.Collections.Generic;
using System.Threading.Tasks;
using SnkFramework.PatchService.Runtime.Core;

namespace SnkFramework.PatchService.Runtime
{
    public interface ISnkPatchService
    {
        public int LocalResVersion { get; }
        public int RemoteResVersion { get; }

        public Task Initialize();
        public bool IsLatestVersion();
        public Task<SnkDiffManifest> PreviewPatchSynchronyPromise();
        public Task ApplyDiffManifest(SnkDiffManifest diffManifest);
        public Task<SnkDiffManifest> PreviewRepairSourceToLatestVersion();
    }
}