using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SnkFramework.Network.Web;
using SnkFramework.PatchService.Runtime.Core;
using UnityEngine;

namespace SnkFramework.PatchService.Runtime
{
    public class SnkRemoteSourceRepository : SnkSourceRepository, ISnkRemoteSourceRepository
    {
        private const string ROOTPATH = "https://windfantasy-1255691311.cos.ap-beijing.myqcloud.com/PatcherRepository";
        public override int Version => _versionInfos.resVersion;

        private SnkVersionInfos _versionInfos;

        public override async Task Initialize(SnkPatchSettings settings)
        {
            await base.Initialize(settings);
            var uri = Path.Combine(ROOTPATH, _settings.channelName, SNK_BUILDER_CONST.VERSION_INFO_FILE_NAME);
            var (result, jsonString) = await SnkWeb.HttpGet(uri);
            if (result == false)
            {
                throw new AggregateException("获取远端版本信息失败。URL:" + uri);
            }
            _versionInfos = SnkPatchService.jsonParser.FromJson<SnkVersionInfos>(jsonString);
        }

        public IEnumerable<int> GetResVersionHistories() => this._versionInfos.histories;

        private async Task<T> InternalGet<T>(int version, string fileName) where T : class
        {
            var uri = Path.Combine(ROOTPATH, _settings.channelName, PatchHelper.GetVersionDirectoryName(version), fileName);
            var (result, jsonString) = await SnkWeb.HttpGet(uri);
            return result == false ? null : SnkPatchService.jsonParser.FromJson<T>(jsonString);
        }

        public override async Task<List<SnkSourceInfo>> GetSourceInfoList(int version)
            => await InternalGet<List<SnkSourceInfo>>(version, SNK_BUILDER_CONST.SOURCE_FILE_NAME);

        public async Task<SnkDiffManifest> GetDiffManifest(int version)
            => await InternalGet<SnkDiffManifest>(version, SNK_BUILDER_CONST.DIFF_FILE_NAME);

        public async Task TakeFileToLocal(string dirPath, string key, int version)
        {
            var uri = Path.Combine(ROOTPATH, _settings.channelName,
                PatchHelper.GetVersionDirectoryName(version),
                SNK_BUILDER_CONST.VERSION_SOURCE_MID_DIR_PATH, key);
            
            var localDirName = this._settings.repoRootPath;
            if (Directory.Exists(localDirName) == false)
                Directory.CreateDirectory(localDirName);
            var savePath = Path.Combine(localDirName, key);
            
            var result = await SnkWeb.HttpDownload(uri, savePath);
            if (result == false)
                throw new Exception("[Download-Error]" + uri);
        }
    }
}