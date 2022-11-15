using System.Collections.Generic;

namespace SnkFramework.PatchBuilder.Runtime.Base
{
    /// <summary>
    /// 资源差异清单
    /// </summary>
    [System.Serializable]
    public class DiffManifest
    {
        /// <summary>
        /// 新增、更新的资源列表
        /// </summary>
        public List<SourceInfo> addList;
        
        /// <summary>
        /// 删除的资源名列表
        /// </summary>
        public List<string> delList;
    }
}