using System;
using System.Threading.Tasks;
using SnkFramework.NuGet;
using SnkFramework.NuGet.Features.Patch;
using SnkFramework.Runtime;
using SnkFramework.Runtime.Basic;
using UnityEngine;

namespace BFFramework.Runtime.Services
{
    public class BFPatchService : BFServiceBase, IBFPatchService
    {
        private string _channelName = "windf_iOS";
        private string _appVersion = "1.0.0";
        private string[] _urls =
        {
            "https://windfantasy-1255691311.cos.ap-beijing.myqcloud.com/PersistentRepo"
        };
        private string _localPatchRepoPath = "BFPatchService";
        
        public ISnkPatchController _patchCtrl;
        public BFPatchService()
        {
            SnkNuget.Logger = SnkLogHost.Default;
            
            var settings = new SnkPatchControlSettings
            {
                remoteURLs = this._urls,
                localPatchRepoPath = this._localPatchRepoPath
            };
            this._patchCtrl = SnkPatch.CreatePatchExecuor<SnkJsonParser>(_channelName, _appVersion, settings);
        }

        public async Task Initialize()
        {
            try
            {
                await this._patchCtrl.Initialize();
                var (addList, delList) = await this._patchCtrl.PreviewDiff();
                await this._patchCtrl.Apply(addList, delList);
            }
            catch (Exception e)
            {
                SnkLogHost.Default.Exception(e, "BFPatchService.Initialize");
            }
        }
    }
}