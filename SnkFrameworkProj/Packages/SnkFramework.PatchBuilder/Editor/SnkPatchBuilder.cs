using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using SnkFramework.PatchBuilder.Runtime;
using SnkFramework.PatchBuilder.Runtime.Base;
using SnkFramework.PatchBuilder.Runtime.Core;

namespace SnkFramework.PatchBuilder
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
            private readonly SnkBuilderSettings _settings;
            
            /// <summary>
            /// 补丁包清单
            /// </summary>
            private readonly SnkPatcherManifest _patcherManifest;
            
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
            private ISnkPatchCompressor Compressor=> _compressor ?? new SnkPatchCompressor();

            /// <summary>
            /// Json解析器
            /// </summary>
            private ISnkPatchJsonParser _jsonParser;
            private ISnkPatchJsonParser JsonParser => this._jsonParser ?? new SnkPatchJsonParser();
            
            /// <summary>
            /// 从渠道名加载渠道补丁包构建器
            /// </summary>
            /// <param name="channelName">渠道名字</param>
            /// <returns>补丁包构建器</returns>
            public static SnkPatchBuilder Load(string channelName)
            {
                SnkPatchBuilder builder = new SnkPatchBuilder(channelName);
                return builder;
            } 
            
            /// <summary>
            /// 构建方法
            /// </summary>
            /// <param name="channelName"></param>
            private SnkPatchBuilder(string channelName)
            {
                this._channelName = channelName;

                this._settings = LoadSettings();
                this._patcherManifest = LoadPatcherManifest();
                if(string.IsNullOrEmpty(this._patcherManifest.lastPatcherName) == false) 
                    this._lastSourceInfoList = LoadLastSourceInfoList(this._patcherManifest.lastPatcherName);
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
            /// 加载设置文件
            /// </summary>
            /// <returns>设置文件</returns>
            private SnkBuilderSettings LoadSettings()
            {
                var fileInfo = new FileInfo(Path.Combine(ChannelRepoPath, SNK_BUILDER_CONST.SETTING_FILE_NAME));
                if (fileInfo.Exists == false)
                    this.SaveSettings(new SnkBuilderSettings());
                string jsonString = File.ReadAllText(fileInfo.FullName);
                return this.JsonParser.FromJson<SnkBuilderSettings>(jsonString);
            }

            /// <summary>
            /// 保存设置文件
            /// </summary>
            /// <param name="settings">设置文件</param>
            private void SaveSettings(SnkBuilderSettings settings)
            {
                var fileInfo = new FileInfo(Path.Combine(ChannelRepoPath, SNK_BUILDER_CONST.SETTING_FILE_NAME));
                if (fileInfo.Directory!.Exists == false)
                    fileInfo.Directory.Create();
                File.WriteAllText(fileInfo.FullName,this.JsonParser.ToJson(settings));
            }

            /// <summary>
            /// 加载补丁包清单
            /// </summary>
            /// <returns></returns>
            private SnkPatcherManifest LoadPatcherManifest()
            {
                var fileInfo = new FileInfo(Path.Combine(ChannelRepoPath, SNK_BUILDER_CONST.PATCHER_FILE_NAME));
                if (fileInfo.Exists == false)
                    return new SnkPatcherManifest();
                string jsonString = File.ReadAllText(fileInfo.FullName);
                return this.JsonParser.FromJson<SnkPatcherManifest>(jsonString);
            }

            /// <summary>
            /// 保存补丁包清单
            /// </summary>
            /// <param name="patcherManifest"></param>
            private void SavePatcherManifest(SnkPatcherManifest patcherManifest)
            {
                string patcherManifestPath = Path.Combine(ChannelRepoPath, SNK_BUILDER_CONST.PATCHER_FILE_NAME);
                File.WriteAllText(patcherManifestPath, this.JsonParser.ToJson(patcherManifest));
            }

            /// <summary>
            /// 加载最新资源信息列表
            /// </summary>
            /// <returns>最新资源信息列表</returns>
            private List<SnkSourceInfo> LoadLastSourceInfoList(string patcherName)
            {
                FileInfo fileInfo = new FileInfo(Path.Combine(ChannelRepoPath, patcherName, SNK_BUILDER_CONST.SOURCE_FILE_NAME));
                if (fileInfo.Exists == false)
                    return null;
                string jsonString = File.ReadAllText(fileInfo.FullName);
                return this.JsonParser.FromJson<List<SnkSourceInfo>>(jsonString);
            }

            /// <summary>
            /// 保存最新资源信息列表
            /// </summary>
            /// <param name="sourceInfoList">最新资源信息列表</param>
            private void SaveLastSourceInfoList(List<SnkSourceInfo> sourceInfoList, string patcherName)
            {
                string manifestPath = Path.Combine(ChannelRepoPath, patcherName, SNK_BUILDER_CONST.SOURCE_FILE_NAME);
                File.WriteAllText(manifestPath, this.JsonParser.ToJson(sourceInfoList));
            }

            /// <summary>
            /// 生成资源信息列表
            /// </summary>
            /// <param name="version">信息列表的版本号</param>
            /// <param name="sourceFinder">资源探测器</param>
            /// <returns>资源信息列表</returns>
            private List<SnkSourceInfo> GenerateSourceInfoList(int version, ISnkSourceFinder sourceFinder)
            {
                List<SnkSourceInfo> sourceInfoList = new List<SnkSourceInfo>();

                bool result = sourceFinder.TrySurvey(out var fileInfos,  out var dirFullPath);
                if(result == false)
                    return null;

                foreach (var fileInfo in fileInfos)
                {
                    var sourceInfo = new SnkSourceInfo
                    {
                        name = fileInfo.FullName.Replace(dirFullPath, string.Empty).Substring(1),
                        version = version,
                        md5 = fileInfo.FullName.GetHashCode().ToString(),
                        size = fileInfo.Length
                    };
                    sourceInfoList.Add(sourceInfo);
                }

                return sourceInfoList;
            }

            /// <summary>
            /// 生成资源差异清单
            /// </summary>
            /// <param name="prevSourceInfoList">上一个版本的资源信息列表</param>
            /// <param name="currSourceInfoList">当前版本的资源列表</param>
            /// <returns>资源差异清单</returns>
            private SnkDiffManifest GenerateDiffManifest(IReadOnlyCollection<SnkSourceInfo> prevSourceInfoList, IReadOnlyCollection<SnkSourceInfo> currSourceInfoList)
            {
                if (prevSourceInfoList == null)
                    return null;
                var comparer = new SnkSourceInfoComparer();

                var diffManifest = new SnkDiffManifest
                {
                    delList = prevSourceInfoList.Except(currSourceInfoList, comparer).Select(a => a.name).ToList(),
                    addList = currSourceInfoList.Except(prevSourceInfoList, comparer).ToList()
                };
                return diffManifest;
            }

            /// <summary>
            /// 复制资源文件到指定目录下
            /// </summary>
            /// <param name="toDirectoryFullPath">目标目录的绝对路径</param>
            /// <param name="sourceInfoList">需要复制的资源信息列表</param>
            private void CopySourceTo(string toDirectoryFullPath, List<SnkSourceInfo> sourceInfoList)
            {
                foreach (var sourceInfo in sourceInfoList)
                {
                    var fromFileInfo = new FileInfo(sourceInfo.name);
                    var toFileInfo = new FileInfo(Path.Combine(toDirectoryFullPath, sourceInfo.name));
                    if (toFileInfo.Directory!.Exists == false)
                        toFileInfo.Directory.Create();
                    fromFileInfo.CopyTo(toFileInfo.FullName);
                }
            }

            /// <summary>
            /// 构建
            /// </summary>
            /// <param name="sourceFinderList">资源探测器列表</param>
            /// <param name="isForce">是否是强更补丁包</param>
            /// <param name="isCompress">是否进行压缩</param>
            /// <returns>补丁包信息</returns>
            public SnkPatcher Build(IEnumerable<ISnkSourceFinder> sourceFinderList, bool isForce = false)
            {
                var prevVersion = _patcherManifest.lastVersion;
                var currVersion = prevVersion + 1;

                var patcher = new SnkPatcher
                {
                    version = currVersion,
                    isForce = isForce,
                };

                //生成当前目标目录的资源信息列表
                var currSourceInfoList = new List<SnkSourceInfo>();
                foreach (var sourcePath in sourceFinderList)
                {
                    var list = GenerateSourceInfoList(currVersion, sourcePath);
                    if (list == null)
                        continue;
                    currSourceInfoList.AddRange(list);
                }

                //拼接补丁包名字
                patcher.name = string.Format(SNK_BUILDER_CONST.PATCHER_NAME_FORMATER, prevVersion,currVersion, ++_settings.buildNum);
                string patcherDirPath = Path.Combine(ChannelRepoPath, patcher.name);

                if (Directory.Exists(patcherDirPath) == false)
                    Directory.CreateDirectory(patcherDirPath);
                
                List<SnkSourceInfo> willMoveSourceList;
                if (_lastSourceInfoList == null)
                {
                    //生成初始资源包（拓展包）
                    _lastSourceInfoList = new List<SnkSourceInfo>();
                    _lastSourceInfoList = currSourceInfoList;
                    willMoveSourceList = currSourceInfoList;
                }
                else
                {
                    //生成差异列表
                    var diffManifest = GenerateDiffManifest(_lastSourceInfoList, currSourceInfoList);

                    //移除所有变化的资源
                    _lastSourceInfoList.RemoveAll(a => diffManifest.addList.Exists(b => a.name == b.name));
                    _lastSourceInfoList.RemoveAll(a => diffManifest.delList.Exists(b => a.name == b));

                    //添加新增或者更新的资源
                    _lastSourceInfoList.AddRange(diffManifest.addList);

                    //将要复制的资源
                    willMoveSourceList = diffManifest.addList;

                    //保存差异清单
                    var diffManifestFileInfo = new FileInfo(Path.Combine(patcherDirPath, SNK_BUILDER_CONST.DIFF_FILE_NAME));
                    if(diffManifestFileInfo.Exists)
                        diffManifestFileInfo.Delete();
                    File.WriteAllText(diffManifestFileInfo.FullName, this.JsonParser.ToJson(diffManifest));
                }
                
                //保存最新的资源清单
                this.SaveLastSourceInfoList(_lastSourceInfoList, patcher.name);
                
                //复制资源文件
                CopySourceTo(patcherDirPath, willMoveSourceList);
                patcher.sourceCount = willMoveSourceList.Count;
                
                
                //压缩
                if (SNK_BUILDER_CONST.COMPRESS_MODE)
                {
                    FileInfo zipFileInfo = new FileInfo(patcherDirPath + SNK_BUILDER_CONST.COMPRESS_FILE_SUFFIX);
                    Compressor.Compress(patcherDirPath, zipFileInfo.FullName, CompressionLevel.Optimal, true);
                    Directory.Delete(patcherDirPath, true);
                    patcher.totalSize = zipFileInfo.Length;
                }
                else
                {
                    patcher.totalSize = willMoveSourceList.Sum(a => a.size);
                }

                //刷新补丁包列表
                _patcherManifest.lastVersion = currVersion;
                _patcherManifest.lastPatcherName = patcher.name;
                _patcherManifest.patcherList.Add(patcher);
                this.SavePatcherManifest(_patcherManifest);

                //保存设置文件
                this.SaveSettings(_settings);
                return patcher;
            }
        }
    }
}
