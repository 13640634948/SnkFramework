using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnkFramework.NuGet;
using SnkFramework.NuGet.Features.Patch;
using SnkFramework.Runtime;
using SnkFramework.Runtime.Basic;

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
        private bool _isDone = false;
        public bool IsDone => this._isDone;

        public float Progress => 0;// this._patchCtrl.DownloadProgress;
        
        private List<SnkSourceInfo> _addList;
        private List<string> _delList;
        
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
            }
            catch (Exception e)
            {
                SnkLogHost.Default.Exception(e, "BFPatchService.Initialize");
            }
        }

        public async Task<bool> IsNeedPatch()
        {
            (_addList, _delList) = await this._patchCtrl.PreviewDiff();
            return _addList.Count > 0 || _delList.Count > 0;
        }

        public async Task Apply()
        {
            await this._patchCtrl.Apply(_addList, _delList);
            this._isDone = true;
        }
    }
}