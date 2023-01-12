using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks; 
using SnkFramework.PatchService.Runtime.Base;
using SnkFramework.PatchService.Runtime.Core;

namespace SnkFramework.PatchService.Runtime
{
    public class SnkPatchService : ISnkPatchService
    {
        public static ISnkPatchJsonParser jsonParser => new SnkPatchJsonParser();
        
        private ISnkLocalSourceRepository _localRepo;
        private ISnkRemoteSourceRepository _remoteRepo;

        public int LocalResVersion => this._localRepo.Version;
        public int RemoteResVersion=> this._remoteRepo.Version;
        

        private bool _initialized = false;

        public SnkPatchService(ISnkLocalSourceRepository localRepo = null, ISnkRemoteSourceRepository remoteRepo = null)
        {
            _localRepo = localRepo ?? new SnkLocalSourceRepository();
            _remoteRepo = remoteRepo ?? new SnkRemoteSourceRepository();
        }

        public async Task Initialize(SnkPatchSettings settings)
        { 
            await this._localRepo.Initialize(settings);
            await this._remoteRepo.Initialize(settings);
            this._initialized = true;
        }

        private void AvailableCheck()
        {
            if (_initialized == false)
                throw new SnkException("PatchService is not initialized");
            if (_localRepo == null)
                throw new SnkException("ILocalPatchStorage is null");
            if (_remoteRepo == null)
                throw new SnkException("IRemotePatchStorage is null");
        }

        /// <summary>
        /// 当前是否是最新版本
        /// </summary>
        /// <returns>bool:最新版本；SnkDiffManifest：升级到最新版本的差异列表</returns>
        public bool IsLatestVersion()
        {
            this.AvailableCheck();
            return this._localRepo.Version == this._remoteRepo.Version;
        }

        /// <summary>
        /// 收集升级资源文件
        /// </summary>
        /// <param name="from">从from版本</param>
        /// <param name="to">到to版本</param>
        /// <returns>升级资源列表</returns>
        public async Task<SnkDiffManifest> PreviewPatchSynchronyPromise()
        {
            //从远端版本列表中，筛选出比本地版本号大的资源版本
            var upgradeList = (from versionMeta in this._remoteRepo.GetResVersionHistories()
                where Math.Abs(versionMeta.version) > this._localRepo.Version
                select Math.Abs(versionMeta.version)).ToList();
            
            var remoteManifest = await this._remoteRepo.GetSourceInfoList(upgradeList[^1]);
            if (remoteManifest == null)
                throw new NullReferenceException("获取远端manifest.json失败。resVersion:" + upgradeList[^1]);

            var finder = new SnkSourceFinder
            {
                sourceDirPath = this._localRepo.LocalPath
            };
            var localManifest = PatchHelper.GenerateSourceInfoList(-1, finder);


            return new SnkDiffManifest
            {
                addList = remoteManifest.Except(localManifest, PatchHelper.comparer).ToList(),
                delList = localManifest.Except(remoteManifest, PatchHelper.comparer).Select(a => a.name).ToList(),
            };
        }
 
        /// <summary>
        /// 应用补丁
        /// </summary>
        /// <param name="promise"></param>
        public async Task ApplyDiffManifest(SnkDiffManifest diffManifest)
        {
            var localPath = Path.Combine(this._localRepo.LocalPath, SNK_BUILDER_CONST.PATCH_ASSETS_DIR_NAME);
            foreach (var filePath in diffManifest.delList)
            {
                File.Delete(filePath);
            }  
            
            foreach (var sourceInfo in diffManifest.addList)
            {
                await this._remoteRepo.TakeFileToLocal(localPath, sourceInfo.name, sourceInfo.version);
            }
            this._localRepo.UpdateLocalResVersion(this._remoteRepo.Version);
        }

        private async Task<SnkDiffManifest> PreviewRollBackTo(int version)
        {
            var localList = await this._localRepo.GetSourceInfoList();
            var remoteList = await this._remoteRepo.GetSourceInfoList(version);
            
            return new SnkDiffManifest
            {
                addList = remoteList.Except(localList, PatchHelper.comparer).ToList(),
                delList = localList.Except(remoteList, PatchHelper.comparer).Select(a => a.name).ToList(),
            };
        }

        public async Task<SnkDiffManifest> PreviewRepairSourceToLatestVersion()
            => await PreviewRollBackTo(this._remoteRepo.Version);
    }
}