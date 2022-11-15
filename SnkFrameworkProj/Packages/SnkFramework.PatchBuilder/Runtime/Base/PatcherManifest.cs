using System.Collections.Generic;

namespace SnkFramework.PatchBuilder.Runtime.Base
{
    /// <summary>
    /// 补丁包清单
    /// </summary>
    [System.Serializable]
    public class PatcherManifest
    {
        /// <summary>
        /// 最新版本号
        /// </summary>
        public int lastVersion;
        
        /// <summary>
        /// 最新补丁包名字
        /// </summary>
        public string lastPatcherName;
        
        /// <summary>
        /// 补丁包列表
        /// </summary>
        public List<Patcher> patcherList = new List<Patcher>();
    }
}