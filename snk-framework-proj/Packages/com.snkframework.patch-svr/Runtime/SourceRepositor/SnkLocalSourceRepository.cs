using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SnkFramework.PatchService.Runtime.Core;

namespace SnkFramework.PatchService.Runtime
{
    public class SnkLocalSourceRepository : SnkSourceRepository, ISnkLocalSourceRepository
    {
        private int _version = -1;
        public override int Version => _version;
        public string LocalPath { get; }

        private string setingsPath;

        public override async Task Initialize(SnkPatchSettings settings)
        {
            await base.Initialize(settings);
            setingsPath = Path.Combine(settings.repoRootPath, _settings.clientSettingsFileName);
            this._version = await LoadLocalResVersion();
        }

        private async Task<int> LoadLocalResVersion()
        {
            var version = -1;
            if (!File.Exists(setingsPath)) 
                return version;
            var text = await File.ReadAllTextAsync(setingsPath);
            version = int.Parse(text.Trim());
            return version;
        }

        public void UpdateLocalResVersion(int resVersion)
        {
            const string path = "persistentDataPath/.client";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, resVersion.ToString());
            this._version = resVersion;
        }

        public override async Task<List<SnkSourceInfo>> GetSourceInfoList(int version = -1)
        {
            var list = new List<SnkSourceInfo>();
            var filePaths = Directory.GetFiles(_settings.repoRootPath, "*.*", SearchOption.AllDirectories);
            foreach (var filePath in filePaths)
            {
                if(Path.GetFileName(filePath).StartsWith("."))
                    continue;
                var info = new SnkSourceInfo
                {
                    name = filePath.Replace(_settings.repoRootPath + "/", string.Empty),
                    md5 = PatchHelper.getMD5ByMD5CryptoService(filePath)
                };
                list.Add(info);
            }
            return list;
        }

    }
}