using System;
using System.IO;

namespace SnkFramework.PatchBuilder.Runtime.Base
{
    /// <summary>
    /// 资源探测器
    /// </summary>
    [System.Serializable]
    public class SourceFinder : ISourceFinder
    {
        /// <summary>
        /// 资源目录路径
        /// </summary>
        public string sourceDirPath;
        
        /// <summary>
        /// 筛选关键字
        /// </summary>
        public string[] filters;
        
        /// <summary>
        /// 忽略关键字
        /// </summary>
        public string[] ignores;

        /// <summary>
        /// 尝试探测资源
        /// </summary>
        /// <param name="fileInfos">构建出的资源文件信息</param>
        /// <param name="dirFullPath">资源目录的根路径</param>
        /// <returns>操作结果：true：成功， false：失败</returns>
        public bool TrySurvey(out FileInfo[] fileInfos, out string dirFullPath)
        {
            fileInfos = null;
            dirFullPath = String.Empty;
            DirectoryInfo dirInfo = new DirectoryInfo(sourceDirPath);
            if (dirInfo.Exists == false)
                return false;
            dirFullPath = dirInfo.Parent!.FullName;
            fileInfos =  dirInfo.GetFiles("*.*", SearchOption.AllDirectories);
            return true;
        }
    }
}