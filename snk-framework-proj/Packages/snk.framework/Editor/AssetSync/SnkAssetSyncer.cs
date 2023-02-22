using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SnkFramework.Network.ContentDelivery.Editor;
using SnkFramework.NuGet;
using SnkFramework.NuGet.Basic;
using SnkFramework.NuGet.Features.Logging;
using SnkFramework.NuGet.Features.Patch;
using SnkFramework.Runtime.Engine;
using UnityEditor;
using UnityEngine;

namespace SnkFramework.Editor.AssetSync
{
    public class SnkAssetSyncer<TCodeGenerator>
    where TCodeGenerator : class, ISnkCodeGenerator, new()
    {
        private const string MetaFileName = ".snk_manifest";
        private readonly ISnkCodeGenerator _codeGenerator;
        private readonly string _localPath;
        private readonly string _dirName;

        public SnkAssetSyncer(string localPath)
        {
            this._localPath = System.IO.Path.GetFullPath(localPath);
            this._codeGenerator = new TCodeGenerator();

            this._dirName = Path.GetFileName(this._localPath);
        }

        private void InternalRefreshFileMeta(string dirFullPath)
        {
            var dirInfo = new DirectoryInfo(dirFullPath);

            var manifestPath = dirInfo.FullName + "/" + MetaFileName;
            if (File.Exists(manifestPath))
                File.Delete(manifestPath);

            var dirName = dirInfo.FullName.Replace(this._localPath, string.Empty);
            var manifestContext = dirName + "/\n";
            foreach (var fileInfo in dirInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly))
            {
                if(fileInfo.FullName.Contains(".DS_Store"))
                    continue;
                manifestContext += $"{fileInfo.Name}&{_codeGenerator.GetMD5ByHashAlgorithm(fileInfo.FullName)}\n";
            }
            File.WriteAllText(manifestPath, manifestContext.Trim());
        }
        
        private void RefreshFileMeta()
        {
            InternalRefreshFileMeta(_localPath);
            foreach (var dir in Directory.GetDirectories(this._localPath))
                InternalRefreshFileMeta(dir);
        }

        public void LocalSyncToRemote<TStorage>()
            where TStorage : class, ISnkContentDeliveryStorage, new()
        {
            SnkNuget.Logger ??= new SnkLogger("AssetSyncer", new SnkUnityLoggerProvider());

            ISnkContentDeliveryStorage storage = null;
            try
            {
                storage = new TStorage();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message + "\n" + e.StackTrace);
                Debug.LogError("创建远端仓库对象失败，请检查配置是否正确。type:" + typeof(TStorage).FullName);
                Debug.LogError("请到菜单：Window/SnkFramework/File Storage 配置基础参数");
            }

            if (storage == null)
                throw new NullReferenceException("创建仓库失败");
            
            storage.SetProgressCallbackHandle((key, transferredBytes, totalBytes,curr,count) =>
            {
                var title = "上传资源至-" + storage.StorageName;
                var progress = transferredBytes / (float)totalBytes;
                var info = $"({curr}/{count}) {progress*100:0.00}%  {key}";
                EditorUtility.DisplayProgressBar(title, info, transferredBytes/(float)totalBytes);
            });
            
            var manifestList =
                (from tuple in storage.LoadObjects()
                where tuple.Item1.Contains(MetaFileName)
                select tuple.Item1).ToList();
            
            var remoteSourceInfoList = new List<SnkSourceInfo>();
            foreach (var manifestKey in manifestList)
            {
                var bytes = storage.TaskObject(manifestKey);
                var content = System.Text.Encoding.UTF8.GetString(bytes.ToArray());
                var lines = content.Trim().Split("\n");
                for (var i = 1; i < lines.Length; i++)
                {
                    var p = lines[i].Split("&");
                    remoteSourceInfoList.Add(new SnkSourceInfo()
                    {
                        key = this._dirName + lines[0] + p[0],
                        code = p[1],
                    });
                }
            }
            
            RefreshFileMeta();
            var finder = new SnkFileFinder(_localPath) { ignores = new[] { ".DS_Store" } };
            var localSourceInfoList = SnkPatch.GenerateSourceInfoList("0", finder, out _);
            var (addList, delList) = SnkPatch.CompareToDiff(remoteSourceInfoList, localSourceInfoList);
            
            delList.AddRange(manifestList);
            storage.DeleteObjects(delList);
            storage.PutObjects(addList.Select(info => info.key).ToList());
        }
    }
}