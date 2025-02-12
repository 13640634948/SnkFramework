using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using SnkFramework.NuGet.Basic;
using System.Reflection;
using SnkFramework.NuGet.Logging;

namespace SnkFramework.NuGet.Features
{
    namespace Patch
    {
        public class SnkLocalPatchRepository : ISnkLocalPatchRepository
        {
            private static readonly ISnkLog s_log = SnkLogHost.GetLogger<SnkLocalPatchRepository>();
            public int Version => _versionInfos.resVersion;

            public string LocalPath => _patchCtrl.Settings.localPatchRepoPath;

            protected ISnkPatchController _patchCtrl;

            private SnkLocalPatchVersionInfos _versionInfos;

            public async Task Initialize(ISnkPatchController patchController)
            {
                this._patchCtrl = patchController;

                if (Directory.Exists(this.LocalPath) == false)
                    Directory.CreateDirectory(this.LocalPath);

                this._versionInfos = await loadLocalVersionInfos();
            }

            private async Task<SnkLocalPatchVersionInfos> loadLocalVersionInfos()
            {
                var fileInfo = new FileInfo(Path.Combine(this.LocalPath, this._patchCtrl.Settings.versionInfoFileName));
                if (fileInfo.Exists == false)
                {
                    var versionInfos = new SnkLocalPatchVersionInfos();
                    versionInfos.resVersion = -1;
                    return versionInfos;
                }

                string json = string.Empty;
                using (var fileStream = fileInfo.OpenRead())
                {
                    var buffer = new byte[1024 * 2];
                    var encoding = new UTF8Encoding(true);
                    while (await fileStream.ReadAsync(buffer, 0, buffer.Length) > 0)
                        json += encoding.GetString(buffer);
                }
                if (string.IsNullOrEmpty(json) == true)
                    return null;

                return this._patchCtrl.JsonParser.FromJson<SnkLocalPatchVersionInfos>(json);
            }

            public Task<List<SnkSourceInfo>> GetSourceInfoList(int version = -1)
            {
                if(s_log.IsInfoEnabled)
                    s_log?.Info($"[RemoteRepo]GetSourceInfoList.fromVersion:{version}");

                var list = new List<SnkSourceInfo>();

                var rootDirInfo = new DirectoryInfo(this.LocalPath);
                if (rootDirInfo.Exists == false)
                    throw new DirectoryNotFoundException(this.LocalPath);

                foreach (var dirInfo in rootDirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly))
                {
                    var finder = new SnkFileFinder(dirInfo.FullName);
                    list.AddRange(SnkPatch.GenerateSourceInfoList(version.ToString(), finder, out var _));
                }

                return Task.FromResult<List<SnkSourceInfo>>(list);
            }

            public void UpdateLocalResVersion(int resVersion)
            {
                this._versionInfos.resVersion = resVersion;

                var fileInfo = new FileInfo(Path.Combine(this.LocalPath, this._patchCtrl.Settings.versionInfoFileName));
                if (fileInfo.Exists)
                    fileInfo.Delete();

                if (fileInfo.Directory.Exists == false)
                    fileInfo.Directory.Create();

                var json = this._patchCtrl.JsonParser.ToJson(this._versionInfos);
                File.WriteAllText(fileInfo.FullName, json);
            }

        }
    }
}