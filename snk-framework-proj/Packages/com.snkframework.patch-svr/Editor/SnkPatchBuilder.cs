using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SnkFramework.PatchService.Runtime;
using SnkFramework.PatchService.Runtime.Base;
using SnkFramework.PatchService.Runtime.Core;

namespace SnkFramework.PatchService
{
    namespace Editor
    {
        /// <summary>
        /// 渠道补丁包构建器
        /// </summary>
        public class SnkPatchBuilder
        {
            /// <summary>
            /// Json解析器
            /// </summary>
            private static ISnkPatchJsonParser s_jsonParser;

            private static ISnkPatchJsonParser JsonParser => s_jsonParser ?? new SnkPatchJsonParser();
 
            /// <summary>
            /// 加载版本信息
            /// </summary>
            /// <param name="versionInfos">版本信息引用对象</param>
            /// <returns>结果，true：成功，false：失败</returns>
            private static SnkVersionInfos LoadVersionInfos(string appVersionPath)
            {
                var fileInfo = new FileInfo(Path.Combine(appVersionPath, SNK_BUILDER_CONST.VERSION_INFO_FILE_NAME));
                if (fileInfo.Exists == false)
                {
                    SnkVersionInfos versionInfos = new SnkVersionInfos
                    {
                        histories = new List<VersionMeta>()
                    };
                    return versionInfos;
                }

                var jsonString = File.ReadAllText(fileInfo.FullName);
                return JsonParser.FromJson<SnkVersionInfos>(jsonString);
            }

            /// <summary>
            /// 加载最新资源信息列表
            /// </summary>
            /// <returns>最新资源信息列表</returns>
            private static IEnumerable<SnkSourceInfo> loadHistorySourceInfoList(string appVersionPath, int version)
            {
                var patcherName = PatchHelper.GetVersionDirectoryName(version);
                if (string.IsNullOrEmpty(patcherName))
                    return null;
                
                var fileInfo = new FileInfo(Path.Combine(appVersionPath, patcherName, SNK_BUILDER_CONST.MANIFEST_FILE_NAME));
                if (fileInfo.Exists == false)
                    return null;
                var jsonString = File.ReadAllText(fileInfo.FullName);
                return JsonParser.FromJson<List<SnkSourceInfo>>(jsonString);
            }
 
            public static void Build(string channelName, Version appVersion, IEnumerable<ISnkSourceFinder> sourceFinderList)
            {
                
                var appVersionPath = Path.Combine(SNK_BUILDER_CONST.REPO_ROOT_PATH, channelName, appVersion.ToString());
                
                var lastSourceInfoList = new List<SnkSourceInfo>();
                var resVersion = 0;
                
                var versionInfos = LoadVersionInfos(appVersionPath);
                if (versionInfos.histories.Count > 0)
                {
                    var lastResVersion = versionInfos.histories[^1].version;
                    resVersion = lastResVersion + 1;
                    lastSourceInfoList.AddRange(loadHistorySourceInfoList(appVersionPath, lastResVersion));// new List<SnkSourceInfo>();
                }

                //生成当前目标目录的资源信息列表
                var currSourceInfoList = new List<SnkSourceInfo>();
                foreach (var sourcePath in sourceFinderList)
                {
                    var list = PatchHelper.GenerateSourceInfoList(resVersion, sourcePath);
                    if (list == null)
                        continue;
                    currSourceInfoList.AddRange(list);
                }

                //生成差异列表
                var diffManifest = new SnkDiffManifest
                {
                    addList = currSourceInfoList.Except(lastSourceInfoList, PatchHelper.comparer).ToList(),
                    delList = lastSourceInfoList.Except(currSourceInfoList, PatchHelper.comparer).Select(a => a.name).ToList(),
                };
                
                // 删除资源
                lastSourceInfoList.RemoveAll(a => diffManifest.delList.Exists(b => a.name == b));
                
                //新增资源，更新资源
                lastSourceInfoList.RemoveAll(a => diffManifest.addList.Exists(b => a.name == b.name));
                lastSourceInfoList.AddRange(diffManifest.addList);

                //拼接补丁包名字
                var currPatchPath = Path.Combine(appVersionPath, resVersion.ToString());
                //路径有效性
                if (Directory.Exists(currPatchPath))
                    Directory.Delete(currPatchPath,true);
                Directory.CreateDirectory(currPatchPath);

                //保存最新的资源清单
                var manifestPath = Path.Combine(appVersionPath, resVersion.ToString(), SNK_BUILDER_CONST.MANIFEST_FILE_NAME);
                File.WriteAllText(manifestPath, JsonParser.ToJson(lastSourceInfoList));

                //复制资源文件
                var patchAssetsDirPath = Path.Combine(currPatchPath, SNK_BUILDER_CONST.PATCH_ASSETS_DIR_NAME);
                PatchHelper.CopySourceTo(patchAssetsDirPath, diffManifest.addList);

                //新版本元信息
                var versionMeta = new VersionMeta
                {
                    version = resVersion,
                    size = diffManifest.addList.Sum(a => a.size),
                    count = diffManifest.addList.Count
                };
                versionInfos.histories.Add(versionMeta);
                
                //保存版本信息
                var versionInfosPath = Path.Combine(appVersionPath, SNK_BUILDER_CONST.VERSION_INFO_FILE_NAME);
                File.WriteAllText(versionInfosPath, JsonParser.ToJson(versionInfos));
            }
        }
    }
}