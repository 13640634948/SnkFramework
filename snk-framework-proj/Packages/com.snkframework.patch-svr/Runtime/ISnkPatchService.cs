using System.Threading.Tasks;
using SnkFramework.PatchService.Runtime.Core;

namespace SnkFramework.PatchService.Runtime
{
    public interface ISnkPatchService
    {
        public Task Initialize();
        public bool IsLatestVersion();
        public Task<SnkDiffManifest> PreviewPatchSynchronyPromise();
        public void ApplyPatch(SnkDiffManifest diffManifest);
    }
}