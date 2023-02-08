using System.Threading.Tasks;
using SnkFramework.NuGet.Features.Patch;
using SnkFramework.Runtime.Basic;

namespace BFFramework.Runtime.Services
{
    public class BFPatchService : BFServiceBase, IBFPatchService
    {
        private string _channelName;
        private string _appVersion;
        private string[] _urls = { "", "" };
        private string _localPatchRepoPath = "";
        
        public ISnkPatchController _patchCtrl;
        public BFPatchService()
        {
            var settings = new SnkPatchControlSettings
            {
                remoteURLs = this._urls,
                localPatchRepoPath = this._localPatchRepoPath
            };
            this._patchCtrl = SnkPatch.CreatePatchExecuor<SnkJsonParser>(_channelName, _appVersion, settings);
        }

        public async Task Initialize()
            => this._patchCtrl.Initialize();
    }
}