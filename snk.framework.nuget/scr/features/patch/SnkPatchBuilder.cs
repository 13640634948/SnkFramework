using System.Collections.Generic;
using System.IO;
using System.Linq;

using SnkFramework.NuGet.Basic;
using SnkFramework.NuGet.Exceptions;
using SnkFramework.NuGet.Logging;

namespace SnkFramework.NuGet.Features
{
    namespace Patch
    {
        public class SnkPatchBuilder
        {
            private readonly string _projPath;
            private readonly string _channelName;
            private readonly string _appVersion;
            private readonly SnkPatchSettings _settings;
            private readonly ISnkJsonParser _jsonParser;

            public SnkPatchBuilder(string projPath, string channelName, string appVersion, SnkPatchSettings settings, ISnkJsonParser jsonParser)
            {
                this._projPath = projPath;
                this._channelName = channelName;
                this._appVersion = appVersion;
                this._settings = settings;
                this._jsonParser = jsonParser;
            }

            private SnkVersionInfos LoadVersionInfos(string appVersionPath)
            {
                var fileInfo = new FileInfo(Path.Combine(appVersionPath, _settings.versionInfoFileName));
                if (fileInfo.Exists == false)
                {
                    var versionInfos = new SnkVersionInfos
                    {
                        histories = new List<SnkVersionMeta>()
                    };
                    return versionInfos;
                }

                var jsonString = File.ReadAllText(fileInfo.FullName);
                return this._jsonParser.FromJson<SnkVersionInfos>(jsonString);
            }


            public void Build(List<ISnkFileFinder> finderList)
            {
                if (finderList == null || finderList.Count == 0)
                {
                    throw new SnkException("filderList is null or len = 0");
                }
                SnkLogHost.Default?.Info(Path.GetFullPath(this._projPath));

                var appVersionPath = Path.Combine(this._projPath, this._channelName, _appVersion);
                if (Directory.Exists(appVersionPath) == false)
                    Directory.CreateDirectory(appVersionPath);

                var lastSourceInfoList = new List<SnkSourceInfo>();
                var resVersion = 0;
                
                var versionInfos = LoadVersionInfos(appVersionPath);
                if (versionInfos.histories.Count > 0)
                {
                    var lastResVersion = versionInfos.histories.Last().version;
                    resVersion = lastResVersion + 1;

                    //加载最新的资源列表
                    var lastResManifestPath = Path.Combine(appVersionPath, lastResVersion.ToString(), _settings.manifestFileName);
                    var fileInfo = new FileInfo(lastResManifestPath);
                    if (fileInfo.Exists == true)
                    {
                        var jsonString = File.ReadAllText(fileInfo.FullName);
                        var list = this._jsonParser.FromJson<List<SnkSourceInfo>>(jsonString);
                        lastSourceInfoList.AddRange(list);
                    }
                }

                var projResPath = Path.Combine(appVersionPath, resVersion.ToString());
                if (Directory.Exists(projResPath) == false)
                    Directory.CreateDirectory(projResPath);

                //生成当前目标目录的资源信息列表
                var keyPathMapping = new Dictionary<string, string>();
                var currSourceInfoList = new List<SnkSourceInfo>();

                foreach (var finder in finderList)
                {
                    var list = SnkPatch.GenerateSourceInfoList(resVersion.ToString(), finder, out keyPathMapping);
                    if (list == null)
                        throw new SnkException("没有找到资源文件. path:" + finder.SourceDirPath);
                    currSourceInfoList.AddRange(list);
                }

                //生成差异列表
                var (addList, delList) = SnkPatch.CompareToDiff(lastSourceInfoList, currSourceInfoList);

                // 删除资源
                lastSourceInfoList.RemoveAll(a => delList.Exists(b => a.key == b));

                //新增资源，更新资源
                lastSourceInfoList.RemoveAll(a => addList.Exists(b => a.key == b.key));
                lastSourceInfoList.AddRange(addList);

                //保存最新的资源清单
                var manifestPath = Path.Combine(projResPath, this._settings.manifestFileName);
                File.WriteAllText(manifestPath, this._jsonParser.ToJson(lastSourceInfoList));

                //复制资源文件
                var patchAssetsDirPath = Path.Combine(projResPath, this._settings.assetsDirName);
                SnkPatch.CopySourceTo(patchAssetsDirPath, addList, keyPathMapping);

                //新版本元信息
                var versionMeta = new SnkVersionMeta
                {
                    version = resVersion,
                    size = addList.Sum(a => a.size),
                    count = addList.Count,
                    code = SnkNuget.CodeGenerator.GetMD5ByMD5CryptoService(manifestPath)
                };
                versionInfos.histories.Add(versionMeta);
                versionInfos.appVersion = _appVersion;

                //保存版本信息
                var versionInfosPath = Path.Combine(appVersionPath, this._settings.versionInfoFileName);
                File.WriteAllText(versionInfosPath, this._jsonParser.ToJson(versionInfos));
            }
        }
    }
}