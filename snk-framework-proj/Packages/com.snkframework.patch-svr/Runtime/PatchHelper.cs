using System.Collections.Generic;
using System.IO;
using System.Linq;
using SnkFramework.PatchService.Runtime.Core;

namespace SnkFramework.PatchService.Runtime
{
    public class PatchHelper
    {
        /// <summary>
        /// 生成资源差异清单
        /// </summary>
        /// <param name="prevSourceInfoList">上一个版本的资源信息列表</param>
        /// <param name="currSourceInfoList">当前版本的资源列表</param>
        /// <returns>资源差异清单</returns>
        public static SnkDiffManifest GenerateDiffManifest(IReadOnlyCollection<SnkSourceInfo> prevSourceInfoList, IReadOnlyCollection<SnkSourceInfo> currSourceInfoList)
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
        public static void CopySourceTo(string toDirectoryFullPath, List<SnkSourceInfo> sourceInfoList)
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