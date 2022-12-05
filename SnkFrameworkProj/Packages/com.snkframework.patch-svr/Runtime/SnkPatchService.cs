using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SnkFramework.PatchBuilder.Runtime.Core;

namespace SnkFramework.PatchService.Runtime
{
    public class SnkPatchService : ISnkPatchService
    {
        private ISnkLocalPatchStorage _localStorage;
        private ISnkRemotePatchStorage _remoteStorage;

        private bool _initialized = false;

        public Func<ISnkLocalPatchStorage> localPatchStorageCreator;
        public Func<ISnkRemotePatchStorage> remotePatchStorageCreator;
        
        protected void AvailableCheck()
        {
            if (_initialized == false)
                throw new SnkException("PatchService is not initialized");
            if (_localStorage == null)
                throw new SnkException("ILocalPatchStorage is null");
            if (_remoteStorage == null)
                throw new SnkException("IRemotePatchStorage is null");
        }
        
        public async Task Initialize()
        {
            if (_initialized)
                return;

            this._localStorage = this.localPatchStorageCreator();
            this._remoteStorage = this.remotePatchStorageCreator();
            
            await this._localStorage.Initialize();
            await this._remoteStorage.Initialize();
            _initialized = true;
        }

        /// <summary>
        /// 当前是否是最新版本
        /// </summary>
        /// <param name="unupgradedList">需要升级的版本列表</param>
        /// <returns>是否需要升级（true：需要，false：不需要）</returns>
        public bool IsLatestVersion(ref IEnumerable<int> unupgradedList)
        {
            this.AvailableCheck();
            if (this._localStorage.Version == this._remoteStorage.Version)
                return true;

            unupgradedList= from version in this._remoteStorage.GetResVersionHistories()
                where Math.Abs(version) > this._localStorage.Version
                select this._localStorage.Version;
             
            return false;
        }


        /// <summary>
        /// 收集升级资源文件
        /// </summary>
        /// <param name="from">从from版本</param>
        /// <param name="to">到to版本</param>
        /// <returns>升级资源列表</returns>
        public SnkDiffManifest CollectUpgradSourceInfoList(int from, int to)
        {
            SnkDiffManifest resultDiffManifest = new SnkDiffManifest()
            {
                addList = new List<SnkSourceInfo>(),
                delList = new List<string>()
            };

            for (var i = from; i < to; i++)
            {
                var list = this._remoteStorage.GetDiffManifest(i);
                
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
            if (this._localStorage.Exist(sourceKey) == false)
                return SnkSourceCompareResult.local_does_not_exist;
            if (this._remoteStorage.Exist(sourceKey) == false)
                return SnkSourceCompareResult.remote_does_not_exist;

            var localSourceInfo = this._localStorage.GetSourceInfo(sourceKey);
            var remoteSourceInfo = this._remoteStorage.GetSourceInfo(sourceKey);

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
                    throw new SnkException("remote is not exist. sourceKey:" + Path.Combine(sourceInfo.dir, sourceInfo.name));
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
            string localPath = this._localStorage.LocalPath;
            foreach (var sourceInfo in promise.SourceInfoList)
            {
                this._remoteStorage.TakeFileToLocal(localPath, sourceInfo.name);
            }
        }

    }
}