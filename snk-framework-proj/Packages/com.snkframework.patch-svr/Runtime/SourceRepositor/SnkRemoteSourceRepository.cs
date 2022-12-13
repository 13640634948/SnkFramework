using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SnkFramework.Network.Web;
using SnkFramework.PatchService.Runtime.Core;

namespace SnkFramework.PatchService.Runtime
{
    public class SnkRemoteSourceRepository : ISnkRemoteSourceRepository
    {
        private const string ROOTPATH = "https://windfantasy-1255691311.cos.ap-beijing.myqcloud.com/PatcherRepository";
        public int Version => _versionInfos.resVersion;

        private SnkVersionInfos _versionInfos;
        private PatchSettings _settings;
  
        void ISnkSourceRepository.SetupSettings(PatchSettings settings)
        {
            this._settings = settings;
        }

        public async Task Initialize()
        {
            string uri = Path.Combine(ROOTPATH, _settings.channelName, SNK_BUILDER_CONST.VERSION_INFO_FILE_NAME);
            var (result, jsonString) = await SnkWeb.HttpGet(uri);
            if (result == false)
            {
                throw new AggregateException("获取远端版本信息失败。URL:" + uri);
            }
            _versionInfos = SnkPatchService.jsonParser.FromJson<SnkVersionInfos>(jsonString);
        }

        public bool Exist(string sourceKey)
        {
            throw new System.NotImplementedException();
        }

        public SnkSourceInfo GetSourceInfo(string key)
        {
            throw new System.NotImplementedException();
        }

        public List<int> GetResVersionHistories() => this._versionInfos.histories;

        private async Task<T> InternalGet<T>(string uri) where T : class
        {
            var (result, jsonString) = await SnkWeb.HttpGet(uri);
            return result == false ? null : SnkPatchService.jsonParser.FromJson<T>(jsonString);
        }

        public async Task<List<SnkSourceInfo>> GetSourceInfoList(int version)
        {
            var uri = Path.Combine(ROOTPATH, _settings.channelName, SNK_BUILDER_CONST.VERSION_DIR_NAME_FORMATER, SNK_BUILDER_CONST.SOURCE_FILE_NAME);
            var list = await InternalGet<List<SnkSourceInfo>>(string.Format(uri, version));
            return list;
        }

        public async Task<SnkDiffManifest> GetDiffManifest(int version)
        {
            string uri = Path.Combine(ROOTPATH, _settings.channelName, SNK_BUILDER_CONST.VERSION_DIR_NAME_FORMATER, SNK_BUILDER_CONST.DIFF_FILE_NAME);
            var list = await InternalGet<SnkDiffManifest>(string.Format(uri, version));
            return list;
        }

        public async Task TakeFileToLocal(string dirPath, string key, int version)
        {
            var localDirName = this._settings.repoRootPath;
            if (Directory.Exists(localDirName) == false)
                Directory.CreateDirectory(localDirName);
            var tmpPath = Path.Combine(ROOTPATH, _settings.channelName,
                SNK_BUILDER_CONST.VERSION_DIR_NAME_FORMATER,
                SNK_BUILDER_CONST.VERSION_SOURCE_MID_DIR_PATH, key);
            var uri = string.Format(tmpPath, version);
            var result = await SnkWeb.HttpDownload(uri, Path.Combine(localDirName, key));
            if (result == false)
                throw new Exception("[Download-Error]" + uri);
        }
    }
}