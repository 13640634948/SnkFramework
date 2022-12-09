using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SnkFramework.Network.Web;
using SnkFramework.PatchService.Runtime.Core;
using UnityEngine;
using Cysharp.Threading.Tasks;
using SnkFramework.PatchService.Runtime.Base;

namespace SnkFramework.PatchService.Runtime
{
    public class SnkRemoteSourceRepository : ISnkRemoteSourceRepository
    {
        
        /// <summary>
        /// Json解析器
        /// </summary>
        private ISnkPatchJsonParser _jsonParser;
        private ISnkPatchJsonParser JsonParser => this._jsonParser ?? new SnkPatchJsonParser();

        
        private const string ROOTPATH = "https://windfantasy-1255691311.cos.ap-beijing.myqcloud.com/PatcherRepository";
        public int Version => _versionInfos.resVersion;

        private SnkVersionInfos _versionInfos;
        
        public async Task Initialize()
        {
            string uri = ROOTPATH + "/windf_iOS/version_info.json";
            await SnkWeb.HttpGet(uri, (result, context) =>
            {
                _versionInfos = JsonUtility.FromJson<SnkVersionInfos>(context);
            });
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

        public async Task<T> internalGet<T>(string uri) where T : class
        {
            //Debug.Log("[GET-URI]" + uri);
            T obj = null;
            await SnkWeb.HttpGet(uri, (result, context) =>
            {
                obj = JsonParser.FromJson<T>(context);
            });
            return obj;
        }
        //https://windfantasy-1255691311.cos.ap-beijing.myqcloud.com/PatcherRepository/windf_iOS/version_1/diff_manifest.json
        public async Task<List<SnkSourceInfo>> GetSourceInfoList(int version)
        {
            string uri = ROOTPATH + "/windf_iOS/version_{0}/" + SNK_BUILDER_CONST.SOURCE_FILE_NAME;
            var list = await internalGet<List<SnkSourceInfo>>(string.Format(uri,version));
            return list;
        }

        public async Task<SnkDiffManifest> GetDiffManifest(int version)
        {
            if (version == 1)
                return null;
            string uri = ROOTPATH + "/windf_iOS/version_{0}/" + SNK_BUILDER_CONST.DIFF_FILE_NAME;
            var list = await internalGet<SnkDiffManifest>(string.Format(uri,version));
            return list;
        }

        public async Task TakeFileToLocal(string dirPath, string key, int version)
        {
            if (Directory.Exists("PersistentDataPath") == false)
                Directory.CreateDirectory("PersistentDataPath");
            string tmpPath = ROOTPATH + "/windf_iOS/version_{0}/source_files/" + key;
            string uri = string.Format(tmpPath, version);
            await SnkWeb.HttpDownload(uri, "PersistentDataPath", result =>
            {
                if(result)
                    Debug.Log("Download Success: " + uri);
                else
                    Debug.LogError("Download Fail: " + uri);
            });
        }
    }
}