using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SnkFramework.PatchService.Runtime.Base;
using SnkFramework.PatchService.Runtime.Core;
using UnityEngine;

namespace SnkFramework.PatchService.Runtime
{
    public class SnkLocalSourceRepository : SnkSourceRepository, ISnkLocalSourceRepository
    {
        private int _version = -1;
        public override int Version => _version;
        public string LocalPath => this._settings.repoRootPath;

        private string _versionFilePath;

        public override async Task Initialize(SnkPatchSettings settings)
        {
            await base.Initialize(settings);
            _versionFilePath = Path.Combine(settings.repoRootPath, _settings.versionFileName);
            var dirInfo = new DirectoryInfo(settings.repoRootPath);
            if(dirInfo.Exists == false)
                dirInfo.Create();
            this._version = await LoadLocalResVersion();
            Debug.Log("初始化本地仓库");
            Debug.Log("本地资源版本号:" + _version);
        }

        private async Task<int> LoadLocalResVersion()
        {
            var version = -1;
            if (!File.Exists(_versionFilePath)) 
                return version;
            var text = await File.ReadAllTextAsync(_versionFilePath);
            version = int.Parse(text.Trim());
            return version;
        }

        public void UpdateLocalResVersion(int resVersion)
        {
            var path = Path.Combine(this._settings.repoRootPath, _settings.versionFileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, resVersion.ToString());
            this._version = resVersion;
        }

        public override async Task<List<SnkSourceInfo>> GetSourceInfoList(int version = -1)
        {
            var sourceFinder = new SnkSourceFinder()
            {
                sourceDirPath = Path.Combine(this._settings.repoRootPath, this._settings.versionFileName),
            };
            return PatchHelper.GenerateSourceInfoList(version, sourceFinder);
        }

    }
}//