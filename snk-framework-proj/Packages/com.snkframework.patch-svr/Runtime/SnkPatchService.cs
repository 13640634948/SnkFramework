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

        private bool _initialized = false;

        public static ISnkPatchService CreatePatchService(PatchSettings settings)
            => CreatePatchService<SnkLocalSourceRepository, SnkRemoteSourceRepository>(settings);
        public static ISnkPatchService CreatePatchService<TLocalRepository, TRemoteRepository>(PatchSettings settings)
            where TLocalRepository : class, ISnkLocalSourceRepository, new()
            where TRemoteRepository : class, ISnkRemoteSourceRepository, new()
        {
            var service = new SnkPatchService
            {
                _settings = settings,
                _localRepo = new TLocalRepository(),
                _remoteRepo = new TRemoteRepository()
            };

            service._localRepo.SetupSettings(settings);
            service._remoteRepo.SetupSettings(settings);
            return service;
        }

        protected SnkPatchService()
        {
            
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

        private PatchSettings _settings;
        public async Task Initialize()
        {
            if (_initialized)
                return;

            await this._localRepo.Initialize();
            await this._remoteRepo.Initialize();
            _initialized = true;
        }

        /// <summary>
        /// 当前是否是最新版本
        /// </summary>
        /// <returns>bool:最新版本；SnkDiffManifest：升级到最新版本的差异列表</returns>
        public async Task<(bool,SnkDiffManifest)> IsLatestVersion()
        {
            this.AvailableCheck();
            if (this._localRepo.Version == this._remoteRepo.Version)
                return (true, null);

            var unupgradedList = from version in this._remoteRepo.GetResVersionHistories()
                where Math.Abs(version) > this._localRepo.Version
                select Math.Abs(version);

            var list = unupgradedList.ToList();
            var diffManifest = await CollectUpgradSourceInfoList(list[0], list[^1]);
            return (false, diffManifest);
        }

        /// <summary>
        /// 收集升级资源文件
        /// </summary>
        /// <param name="from">从from版本</param>
        /// <param name="to">到to版本</param>
        /// <returns>升级资源列表</returns>
        private async Task<SnkDiffManifest> CollectUpgradSourceInfoList(int from, int to)
        {
            SnkDiffManifest resultDiffManifest = new SnkDiffManifest()
            {
                addList = new List<SnkSourceInfo>(),
                delList = new List<string>()
            };

            for (var i = from; i <= to; i++)
            {
                var list = await this._remoteRepo.GetDiffManifest(i);
                if(list == null)
                    continue;
                //移除所有变化的资源
                resultDiffManifest.addList.RemoveAll(a => list.addList.Exists(b => a.name == b.name));
                resultDiffManifest.delList.RemoveAll(a => list.delList.Exists(b => a == b));

                //添加新版本中的资源
                resultDiffManifest.addList.AddRange(list.addList);
                resultDiffManifest.delList.AddRange(list.delList);
            }

            return resultDiffManifest;
        }

        /// <summary>
        /// 比对本地文件与远端文件是否一致
        /// </summary>
        /// <param name="sourceKey">资源Key</param>
        /// <param name="equalityComparer">比较器</param>
        /// <returns>比对结果</returns>
        private SnkSourceCompareResult CompareLocalWithRemote(string sourceKey, IEqualityComparer<SnkSourceInfo> equalityComparer = null)
        {
            if (this._localRepo.Exist(sourceKey) == false)
                return SnkSourceCompareResult.local_does_not_exist;
            if (this._remoteRepo.Exist(sourceKey) == false)
                return SnkSourceCompareResult.remote_does_not_exist;

            var localSourceInfo = this._localRepo.GetSourceInfo(sourceKey);
            var remoteSourceInfo = this._remoteRepo.GetSourceInfo(sourceKey);

            var equality = false;
            if (equalityComparer != null)
                equality = equalityComparer.Equals(localSourceInfo, remoteSourceInfo);
            else
                equality = localSourceInfo.md5 == remoteSourceInfo.md5;
            return equality ? SnkSourceCompareResult.same : SnkSourceCompareResult.not_same;
        }

        /// <summary>
        /// 预览补丁同步信息
        /// </summary>
        /// <param name="diffManifest"></param>
        /// <returns></returns>
        /// <exception cref="SnkException"></exception>
        public SnkPatchSynchronyPromise PreviewPatchSynchronyPromise(SnkDiffManifest diffManifest)
        {
            var promise = new SnkPatchSynchronyPromise();
            foreach (var sourceInfo in diffManifest.addList)
            {
                var compareResult = CompareLocalWithRemote(sourceInfo.name);
                if (compareResult == SnkSourceCompareResult.remote_does_not_exist)
                    throw new SnkException("remote is not exist. sourceKey:" +
                                           Path.Combine(sourceInfo.dir, sourceInfo.name));
                promise.SourceInfoList.Add(sourceInfo);
            }

            return promise;
        }

        /// <summary>
        /// 应用补丁
        /// </summary>
        /// <param name="promise"></param>
        public void ApplyPatch(SnkPatchSynchronyPromise promise)
        {
            string localPath = this._localRepo.LocalPath;
            foreach (var sourceInfo in promise.SourceInfoList)
            {
                this._remoteRepo.TakeFileToLocal(localPath, sourceInfo.name, sourceInfo.version);
            }
        }
    }
}