using System.Threading.Tasks;
using SnkFramework.PatchService.Runtime.Core;

namespace SnkFramework.PatchService.Runtime
{
    public interface ISnkPatchService
    {
        public Task Initialize();
        public Task<(bool, SnkDiffManifest)> IsLatestVersion();
        public SnkPatchSynchronyPromise PreviewPatchSynchronyPromise(SnkDiffManifest diffManifest);
        public void ApplyPatch(SnkPatchSynchronyPromise promise);
    }
}