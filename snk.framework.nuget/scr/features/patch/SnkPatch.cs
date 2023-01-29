//#define SNK_LOG

using System.Collections.Generic;

using SnkFramework.NuGet.Basic;

namespace SnkFramework.NuGet.Features
{
    namespace Patch
    {
        public class SnkPatch
        {
            public static SnkPatchBuilder CreatePatchBuilder(string projPath, string channelName, string appVersion, SnkPatchSettings settings = null)
            {
                var builder = new SnkPatchBuilder(projPath, channelName, appVersion, settings ?? new SnkPatchSettings());
                return builder;
            }

            public static ISnkPatchController CreatePatchExecuor<TLocalRepo, TRemoteRepo>(string channelName, string appVersion, SnkPatchControlSettings settings)
                where TLocalRepo : class, ISnkLocalPatchRepository, new()
                where TRemoteRepo : class, ISnkRemotePatchRepository, new()
                => new SnkPatchController<TLocalRepo, TRemoteRepo>(channelName, appVersion, settings);

            public static ISnkPatchController CreatePatchExecuor(string channelName, string appVersion, SnkPatchControlSettings settings)
                => new SnkPatchController<SnkLocalPatchRepository, SnkRemotePatchRepository>(channelName, appVersion, settings);

            public static List<SnkSourceInfo> GenerateSourceInfoList(string resVersion, ISnkFileFinder fileFinder, out Dictionary<string,string> keyPathMapping)
            {
                keyPathMapping = new Dictionary<string, string>();
                if (fileFinder.TrySurvey(out var fileInfos) == false)
                    return null;

                var list = new List<SnkSourceInfo>();
                var dirInfo = new System.IO.DirectoryInfo(fileFinder.SourceDirPath);
#if SNK_LOG
                Snk.Get<ISnkLogger>().Print("[" + fileFinder.SourceDirPath + "]" + dirInfo.Name);
#endif
                foreach (var fileInfo in fileInfos)
                {
                    var info = new SnkSourceInfo
                    {
                        key = fileInfo.FullName.Replace(dirInfo.FullName, dirInfo.Name),
                        version = resVersion,
                        size = fileInfo.Length,
                        code = Snk.Get<ISnkCodeGenerator>().GetMD5ByMD5CryptoService(fileInfo.FullName)
                    };
                    list.Add(info);
                    keyPathMapping?.Add(info.key, fileInfo.FullName);
                };
                return list;
            }

            public static void CopySourceTo(string toDirectoryFullPath, List<SnkSourceInfo> sourceInfoList, Dictionary<string,string> keyPathMapping)
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

            public static (List<SnkSourceInfo>, List<string>) CompareToDiff(List<SnkSourceInfo> from, List<SnkSourceInfo> to)
            {
#if SNK_LOG
                string diffLogString = string.Empty;
                foreach (var a in from)
                    diffLogString += "[FROM]key:" + a.key + ", code:" + a.code + "\n";

                foreach (var a in to)
                    diffLogString += "[TO]key:" + a.key + ", code:" + a.code + "\n";

#endif
                var addList = new List<SnkSourceInfo>();
                foreach (var a in to)
                {
                    if (from.Exists(xx => xx.key == a.key && xx.code == a.code))
                        continue;
                    addList.Add(a);
#if SNK_LOG
                    diffLogString += "[ADD]key:" + a.key + ", code:" + a.code + "\n";
#endif
                }

                var delList = new List<string>();
                foreach (var a in from)
                {
                    if (to.Exists(xx => xx.key == a.key))
                        continue;
                    delList.Add(a.key);
#if SNK_LOG
                    diffLogString += "[DEL]key:" + a.key + "\n";
#endif
                }
#if SNK_LOG
                Snk.Get<ISnkLogger>().Print(diffLogString.Trim());
#endif
                return (addList, delList);
            }
        }
    }
}