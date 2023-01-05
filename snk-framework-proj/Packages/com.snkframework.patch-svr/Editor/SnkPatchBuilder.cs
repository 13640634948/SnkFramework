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
            /// 渠道名
            /// </summary>
            private readonly string _channelName;

            /// <summary>
            /// 设置文件
            /// </summary>
            //private readonly SnkBuilderSettings _settings;

            /// <summary>
            /// 版本信息
            /// </summary>
            private readonly SnkVersionInfos _versionInfos;

            /// <summary>
            /// 最新资源列表
            /// </summary>
            private List<SnkSourceInfo> _lastSourceInfoList;

            /// <summary>
            /// 渠道仓库根目录
            /// </summary>
            private string ChannelRepoPath => Path.Combine(SNK_BUILDER_CONST.REPO_ROOT_PATH, _channelName);

            /// <summary>
            /// 压缩器
            /// </summary>
            private ISnkPatchCompressor _compressor;

            private ISnkPatchCompressor Compressor => _compressor ?? new SnkPatchCompressor();

            /// <summary>
            /// Json解析器
            /// </summary>
            private ISnkPatchJsonParser _jsonParser;

            private ISnkPatchJsonParser JsonParser =>
                this._jsonParser ??
                RegisterJsonParser<SnkPatchJsonParser>(); // throw new NullReferenceException("没有配置json解析器");

            public ISnkPatchJsonParser RegisterJsonParser<TJsonParser>()
                where TJsonParser : class, ISnkPatchJsonParser, new()
                => this._jsonParser = new TJsonParser();

            /// <summary>
            /// 从渠道名加载渠道补丁包构建器
            /// </summary>
            /// <param name="channelName">渠道名字</param>
            /// <returns>补丁包构建器</returns>
            public static SnkPatchBuilder Load(string channelName)
            {
                var builder = new SnkPatchBuilder(channelName);
                return builder;
            }

            /// <summary>
            /// 构建方法
            /// </summary>
            /// <param name="channelName"></param>
            private SnkPatchBuilder(string channelName)
            {
                this._channelName = channelName;
                if (!LoadVersionInfos(out this._versionInfos)) return;
                var patcherName = PatchHelper.GetVersionDirectoryName(this._versionInfos.resVersion);
                if (string.IsNullOrEmpty(patcherName) == false)
                    this._lastSourceInfoList = LoadLastSourceInfoList(patcherName);
            }

            /// <summary>
            /// 重写Json解析器
            /// </summary>
            /// <typeparam name="TJsonParser"></typeparam>
            public void OverrideJsonParser<TJsonParser>() where TJsonParser : class, ISnkPatchJsonParser, new()
                => this._jsonParser = new TJsonParser();

            /// <summary>
            /// 重写压缩器
            /// </summary>
            /// <typeparam name="TCompressor">压缩器类型</typeparam>
            public void OverrideCompressor<TCompressor>() where TCompressor : class, ISnkPatchCompressor, new()
                => this._compressor = new TCompressor();


            /// <summary>
            /// 加载版本信息
            /// </summary>
            /// <param name="versionInfos">版本信息引用对象</param>
            /// <returns>结果，true：成功，false：失败</returns>
            private bool LoadVersionInfos(out SnkVersionInfos versionInfos)
            {
                var fileInfo = new FileInfo(Path.Combine(ChannelRepoPath, SNK_BUILDER_CONST.VERSION_INFO_FILE_NAME));
                if (fileInfo.Exists == false)
                {
                    versionInfos = new SnkVersionInfos
                    {
                        histories = new List<int>()
                    };
                    return false;
                }

                var jsonString = File.ReadAllText(fileInfo.FullName);
                versionInfos = this.JsonParser.FromJson<SnkVersionInfos>(jsonString);
                return true;
            }

            /// <summary>
            /// 保存版本信息
            /// </summary>
            /// <param name="versionInfos">版本信息对象</param>
            private void SaveVersionInfos(SnkVersionInfos versionInfos)
            {
                var versionInfosPath = Path.Combine(ChannelRepoPath, SNK_BUILDER_CONST.VERSION_INFO_FILE_NAME);
                File.WriteAllText(versionInfosPath, this.JsonParser.ToJson(versionInfos));
            }

            /// <summary>
            /// 加载最新资源信息列表
            /// </summary>
            /// <returns>最新资源信息列表</returns>
            private List<SnkSourceInfo> LoadLastSourceInfoList(string patcherName)
            {
                var fileInfo = new FileInfo(Path.Combine(ChannelRepoPath, patcherName, SNK_BUILDER_CONST.SOURCE_FILE_NAME));
                if (fileInfo.Exists == false)
                    return null;
                var jsonString = File.ReadAllText(fileInfo.FullName);
                return this.JsonParser.FromJson<List<SnkSourceInfo>>(jsonString);
            }

            /// <summary>
            /// 保存最新资源信息列表
            /// </summary>
            /// <param name="sourceInfoList">最新资源信息列表</param>
            /// <param name="versionDirName">版本目录名</param>
            private void SaveLastSourceInfoList(List<SnkSourceInfo> sourceInfoList, string versionDirName)
            {
                var manifestPath = Path.Combine(ChannelRepoPath, versionDirName, SNK_BUILDER_CONST.SOURCE_FILE_NAME);
                File.WriteAllText(manifestPath, this.JsonParser.ToJson(sourceInfoList));
            }

            /// <summary>
            /// 生成资源信息列表
            /// </summary>
            /// <param name="version">信息列表的版本号</param>
            /// <param name="sourceFinder">资源探测器</param>
            /// <returns>资源信息列表</returns>
            private List<SnkSourceInfo> GenerateSourceInfoList(int resVersion, ISnkSourceFinder sourceFinder)
            {
                var result = sourceFinder.TrySurvey(out var fileInfos, out var dirFullPath);
                if (result == false)
                    return null;

                var dirName = PatchHelper.GetVersionDirectoryName(resVersion);

                return fileInfos.Select(fileInfo => new SnkSourceInfo
                    {
                        name = fileInfo.FullName.Replace(dirFullPath, string.Empty).Substring(1),
                        version = resVersion,
                        md5 = PatchHelper.getMD5ByMD5CryptoService(fileInfo.FullName),
                        size = fileInfo.Length,
                        dir = dirName,
                    }).ToList();
            }


            /// <summary>
            /// 构建
            /// </summary>
            /// <param name="sourceFinderList">资源探测器列表</param>
            /// <param name="isResForce">是否是强制版本资源</param>
            /// <param name="appVersion">应用版本</param>
            /// <returns>补丁包信息</returns>
            public void Build(IEnumerable<ISnkSourceFinder> sourceFinderList, bool isResForce = false,
                int appVersion = 0)
            {
                if (appVersion > 0)
                    this._versionInfos.appVersion = appVersion;

                var resVersion = ++this._versionInfos.resVersion;
                this._versionInfos.histories.Add(resVersion * (isResForce ? -1 : 1));

                //生成当前目标目录的资源信息列表
                var currSourceInfoList = new List<SnkSourceInfo>();
                foreach (var sourcePath in sourceFinderList)
                {
                    var list = GenerateSourceInfoList(resVersion, sourcePath);
                    if (list == null)
                        continue;
                    currSourceInfoList.AddRange(list);
                }

                //拼接补丁包名字
                var dirName = PatchHelper.GetVersionDirectoryName(resVersion);
                var patcherDirPath = Path.Combine(ChannelRepoPath, dirName);

                //路径有效性
                if (Directory.Exists(patcherDirPath) == false)
                    Directory.CreateDirectory(patcherDirPath);

                //首次构建资源包时，没有上一个版本资源
                _lastSourceInfoList ??= new List<SnkSourceInfo>();

                //生成差异列表
                var diffManifest = PatchHelper.GenerateDiffManifest(_lastSourceInfoList, currSourceInfoList);

                //移除所有变化的资源
                _lastSourceInfoList.RemoveAll(a => diffManifest.addList.Exists(b => a.name == b.name));
                _lastSourceInfoList.RemoveAll(a => diffManifest.delList.Exists(b => a.name == b));

                //添加新增或者更新的资源
                _lastSourceInfoList.AddRange(diffManifest.addList);

                //将要复制的资源
                var willMoveSourceList = diffManifest.addList;

                //保存差异清单
                var diffManifestFileInfo = new FileInfo(Path.Combine(patcherDirPath, SNK_BUILDER_CONST.DIFF_FILE_NAME));
                if (diffManifestFileInfo.Exists)
                    diffManifestFileInfo.Delete();
                File.WriteAllText(diffManifestFileInfo.FullName, this.JsonParser.ToJson(diffManifest));

                //保存最新的资源清单
                this.SaveLastSourceInfoList(_lastSourceInfoList, dirName);

                //复制资源文件
                var patchSourceRootDirPath = Path.Combine(patcherDirPath, SNK_BUILDER_CONST.VERSION_SOURCE_MID_DIR_PATH);
                PatchHelper.CopySourceTo(patchSourceRootDirPath, willMoveSourceList);

                //保存版本信息
                this.SaveVersionInfos(this._versionInfos);

                //保存设置文件
                //this.SaveSettings(_settings);
            }
        }
    }
}