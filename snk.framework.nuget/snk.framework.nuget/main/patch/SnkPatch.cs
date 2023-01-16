using System.Collections.Generic;

using snk.framework.nuget.basic;

namespace snk.framework.nuget
{
    namespace patch
    {
        public class SnkPatch
        {
            public static SnkPatchBuilder CreatePatchBuilder(string projPath, string channelName, string appVersion, SnkPatchSettings settings = null)
            {
                var builder = new SnkPatchBuilder(projPath, channelName, appVersion, settings ?? new SnkPatchSettings());
                return builder;
            }

            public static SnkPatchExecuor CreatePatchExecuor<TLocalRepo, TRemoteRepo>()
                where TLocalRepo : class, ISnkLocalPatchRepository, new()
                where TRemoteRepo : class, ISnkRemotePatchRepository, new()
            {
                var localRepo = new TLocalRepo();
                var remoteRepo = new TRemoteRepo();
                return new SnkPatchExecuor(localRepo, remoteRepo);
            }

            public static SnkPatchExecuor CreatePatchExecuor(ISnkLocalPatchRepository localRepo = null,
                ISnkRemotePatchRepository remoteRepo = null)
            {
                localRepo = localRepo ?? new SnkLocalPatchRepository();
                remoteRepo = remoteRepo ?? new SnkRemotePatchRepository();
                return new SnkPatchExecuor(localRepo, remoteRepo);
            }

            public static List<SnkSourceInfo> GenerateSourceInfoList(string resVersion, ISnkFileFinder fileFinder, ref Dictionary<string,string> keyPathMapping)
            {
                if (fileFinder.TrySurvey(out var fileInfos, out var dirFullPath) == false)
                    return null;

                var list = new List<SnkSourceInfo>();
                foreach (var fileInfo in fileInfos)
                {
                    var info = new SnkSourceInfo
                    {
                        key = fileInfo.FullName.Replace(dirFullPath, string.Empty).Substring(1),
                        version = resVersion,
                        size = fileInfo.Length,
                        code = Snk.Get<ISnkCodeGenerator>().GetMD5ByMD5CryptoService(fileInfo.FullName)
                    };
                    list.Add(info);
                    keyPathMapping.Add(info.key, fileInfo.FullName);
                };
                return list;
            }

            /// <summary>
            /// 复制资源文件到指定目录下
            /// </summary>
            /// <param name="toDirectoryFullPath">目标目录的绝对路径</param>
            /// <param name="sourceInfoList">需要复制的资源信息列表</param>
            internal static void CopySourceTo(string toDirectoryFullPath, List<SnkSourceInfo> sourceInfoList, Dictionary<string,string> keyPathMapping)
            {
                foreach (var sourceInfo in sourceInfoList)
                {
                    if (keyPathMapping.TryGetValue(sourceInfo.key, out string fullPath))
                    {
                        var fromFileInfo = new System.IO.FileInfo(fullPath);
                        var toFileInfo = new System.IO.FileInfo(System.IO.Path.Combine(toDirectoryFullPath, sourceInfo.key));
                        if (toFileInfo.Directory.Exists == false)
                            toFileInfo.Directory.Create();
                        fromFileInfo.CopyTo(toFileInfo.FullName);
                    }
                    else
                    {
                        throw new System.Exception("路径映射表中，没有好到key对应的路径. key:" + sourceInfo.key);
                    }
                }
            }

        }
    }
}