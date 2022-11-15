using System.Collections.Generic;
using System.IO;
using System.Linq;
using SnkFramework.PatchRepository.Runtime.Base;
using UnityEngine;

namespace SnkFramework.PatchRepository
{
    namespace Runtime
    {
        public class Repository
        {
            private readonly string mRepositoryName;
            private string mRepositoryPath => Path.Combine(REPO_CONST.REPO_ROOT_PATH, mRepositoryName);

            private readonly SourceRepositorySettings _settings;
            private readonly PatcherManifest _patcherManifest;
            private List<SourceInfo> _lastSourceManifest;

            public static Repository Load(string repositoryName) => new(repositoryName);

            private Repository(string repositoryName)
            {
                this.mRepositoryName = repositoryName;

                this._settings = LoadSettings();
                this._patcherManifest = LoadPatcherManifest();
                this._lastSourceManifest = LoadSourceManifest();
            }

            private SourceRepositorySettings LoadSettings()
            {
                var fileInfo = new FileInfo(Path.Combine(mRepositoryPath, REPO_CONST.REPO_SETTING_FILE_NAME));
                if (fileInfo.Exists == false)
                    this.SaveSettings(new SourceRepositorySettings());
                string jsonString = System.IO.File.ReadAllText(fileInfo.FullName);
                return JsonUtility.FromJson<SourceRepositorySettings>(jsonString);
            }

            private void SaveSettings(SourceRepositorySettings settings)
            {
                var fileInfo = new FileInfo(Path.Combine(mRepositoryPath, REPO_CONST.REPO_SETTING_FILE_NAME));
                if (fileInfo.Directory!.Exists == false)
                    fileInfo.Directory.Create();
                System.IO.File.WriteAllText(fileInfo.FullName, JsonUtility.ToJson(settings));
            }

            private PatcherManifest LoadPatcherManifest()
            {
                var fileInfo = new FileInfo(Path.Combine(mRepositoryPath, REPO_CONST.PATCHER_FILE_NAME));
                if (fileInfo.Exists == false)
                    return new PatcherManifest();
                string jsonString = System.IO.File.ReadAllText(fileInfo.FullName);
                return JsonUtility.FromJson<PatcherManifest>(jsonString);
            }

            private void SavePatcherManifest(PatcherManifest patcherManifest)
            {
                string patcherManifestPath = Path.Combine(mRepositoryPath, REPO_CONST.PATCHER_FILE_NAME);
                System.IO.File.WriteAllText(patcherManifestPath, JsonUtility.ToJson(patcherManifest));
            }

            private List<SourceInfo> LoadSourceManifest()
            {
                FileInfo fileInfo = new FileInfo(Path.Combine(mRepositoryPath, REPO_CONST.SOURCE_FILE_NAME));
                if (fileInfo.Exists == false)
                    return null;
                string[] lines = File.ReadAllLines(fileInfo.FullName);
                List<SourceInfo> list = new List<SourceInfo>();
                foreach (var lineString in lines)
                {
                    if (string.IsNullOrEmpty(lineString))
                        continue;
                    var sourceInfo = SourceInfo.ValueOf(lineString);
                    list.Add(sourceInfo);
                }

                return list;
            }

            private void SaveSourceManifest(List<SourceInfo> sourceManifest)
            {
                string manifestPath = Path.Combine(mRepositoryPath, REPO_CONST.SOURCE_FILE_NAME);
                string[] lines = new string[sourceManifest.Count];
                for (int i = 0; i < sourceManifest.Count; i++)
                    lines[i] = sourceManifest[i].ToSerializable();
                System.IO.File.WriteAllLines(manifestPath, lines);
            }

            public List<SourceInfo> GenerateSourceManifest(int version, SourcePath sourcePath)
            {
                List<SourceInfo> sourceInfoList = new List<SourceInfo>();
                DirectoryInfo dirInfo = new DirectoryInfo(sourcePath.fullPath);
                if (dirInfo.Exists == false)
                    return null;

                foreach (var fileInfo in dirInfo.GetFiles("*.*", SearchOption.AllDirectories))
                {
                    var sourceInfo = new SourceInfo();
                    sourceInfo.name = fileInfo.FullName.Replace(dirInfo.Parent!.FullName, string.Empty).Substring(1);
                    sourceInfo.version = version;
                    sourceInfo.md5 = fileInfo.FullName.GetHashCode().ToString();
                    sourceInfo.size = fileInfo.Length;
                    sourceInfoList.Add(sourceInfo);
                }

                return sourceInfoList;
            }

            public DiffManifest GenerateDiffManifest(List<SourceInfo> oldSourceInfoList,
                List<SourceInfo> newSourceInfoList)
            {
                if (oldSourceInfoList == null)
                    return null;
                SourceInfoComparer comparer = new SourceInfoComparer();

                var diffManifest = new DiffManifest();
                diffManifest.delList =
                    oldSourceInfoList.Except(newSourceInfoList, comparer).Select(a => a.name).ToList();
                diffManifest.addList = newSourceInfoList.Except(oldSourceInfoList, comparer).ToList();
                return diffManifest;
            }

            public Patcher Build(IEnumerable<SourcePath> sourcePaths, bool force = false, bool compress = false)
            {
                var prevVersion = _patcherManifest.lastVersion;
                var currVersion = prevVersion + 1;

                var patcher = new Patcher
                {
                    version = currVersion,
                    force = force,
                    compress = compress
                };

                //生成当前目标目录的资源信息列表
                var currSourceInfoList = new List<SourceInfo>();
                foreach (var sourcePath in sourcePaths)
                {
                    var list = GenerateSourceManifest(currVersion, sourcePath);
                    if (list == null)
                        continue;
                    currSourceInfoList.AddRange(list);
                }

                patcher.name = string.Format($"patcher_{prevVersion}_{currVersion}({++_settings.buildNum})");
                string currPatcherPath = Path.Combine(mRepositoryPath, patcher.name);

                List<SourceInfo> willMoveSourceList;
                if (_lastSourceManifest == null)
                {
                    _lastSourceManifest = new List<SourceInfo>();
                    _lastSourceManifest = currSourceInfoList;
                    willMoveSourceList = currSourceInfoList;
                }
                else
                {
                    //生成差异列表
                    var diffManifest = GenerateDiffManifest(_lastSourceManifest, currSourceInfoList);

                    _lastSourceManifest.RemoveAll(a => diffManifest.addList.Exists(b => a.name == b.name));
                    _lastSourceManifest.RemoveAll(a => diffManifest.delList.Exists(b => a.name == b));
                    _lastSourceManifest.AddRange(diffManifest.addList);
                    willMoveSourceList = diffManifest.addList;

                    var diffManifestFileInfo = new FileInfo(Path.Combine(currPatcherPath, REPO_CONST.DIFF_FILE_NAME));
                    if (diffManifestFileInfo.Directory!.Exists == false)
                        diffManifestFileInfo.Directory.Create();
                    System.IO.File.WriteAllText(diffManifestFileInfo.FullName, JsonUtility.ToJson(diffManifest));
                }

                this.SaveSourceManifest(_lastSourceManifest);

                SourceFileMoveTo(currPatcherPath, willMoveSourceList);
                patcher.sourceCount = willMoveSourceList.Count;
                patcher.totalSize = willMoveSourceList.Sum(a => a.size);

                _patcherManifest.lastVersion = currVersion;
                _patcherManifest.lastPatcherName = patcher.name;
                _patcherManifest.patcherList.Add(patcher);
                this.SavePatcherManifest(_patcherManifest);

                this.SaveSettings(_settings);
                return patcher;
            }

            private void SourceFileMoveTo(string toDirectoryFullPath, List<SourceInfo> sourceInfoList)
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
        }
    }
}