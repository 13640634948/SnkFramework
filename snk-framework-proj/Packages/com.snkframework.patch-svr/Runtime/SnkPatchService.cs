using System;
using System.Collections.Generic;
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
        
        private SnkPatchSettings _settings;

        private bool _initialized = false;

        public static async Task<ISnkPatchService> CreatePatchService(SnkPatchSettings settings)
            => await CreatePatchService<SnkLocalSourceRepository, SnkRemoteSourceRepository>(settings);
        
        public static async Task<ISnkPatchService> CreatePatchService<TLocalRepository, TRemoteRepository>(SnkPatchSettings settings)
            where TLocalRepository : class, ISnkLocalSourceRepository, new()
            where TRemoteRepository : class, ISnkRemoteSourceRepository, new()
        {
            var service = new SnkPatchService
            {
                _settings = settings,
                _localRepo = new TLocalRepository(),
                _remoteRepo = new TRemoteRepository()
            };
            await service.Initialize();
            return service;
        }

        protected SnkPatchService()
        {
            
        }

        public async Task Initialize()
        {
            await this._localRepo.Initialize(this._settings);
            await this._remoteRepo.Initialize(this._settings);
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
            var resultDiffManifest = new SnkDiffManifest();

            //从远端版本列表中，筛选出比本地版本号大的资源版本
            var upgradeList = (from version in this._remoteRepo.GetResVersionHistories()
                where Math.Abs(version) > this._localRepo.Version
                select Math.Abs(version)).ToList();

            var currVersion = upgradeList[0];//第一个版本
            while (currVersion <= upgradeList[^1])//最后一个版本
            {
                var list = await this._remoteRepo.GetDiffManifest(currVersion);
                if (list == null)
                    throw new NullReferenceException("获取远端DiffManife.json失败。resVersion:" + currVersion);
                
                //移除新增列表中删除
                resultDiffManifest.addList.RemoveAll(a => list.delList.Exists(b => a.name == b));
                
                //移除所有变化的资源
                resultDiffManifest.addList.RemoveAll(a => list.addList.Exists(b => a.name == b.name));
                resultDiffManifest.delList.RemoveAll(a => list.delList.Exists(b => a == b));

                //添加新版本中的资源
                resultDiffManifest.addList.AddRange(list.addList);
                resultDiffManifest.delList.AddRange(list.delList);

                ++currVersion;
            }
            
            return resultDiffManifest;
        }
 
        /// <summary>
        /// 应用补丁
        /// </summary>
        /// <param name="promise"></param>
        public async Task ApplyDiffManifest(SnkDiffManifest diffManifest)
        {
            string localPath = this._localRepo.LocalPath;
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
            return PatchHelper.GenerateDiffManifest(localList, remoteList);
        }

        public async Task<SnkDiffManifest> PreviewRepairSourceToLatestVersion()
            => await PreviewRollBackTo(this._remoteRepo.Version);
    }
}