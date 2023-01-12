using System.Collections.Generic;

namespace SnkFramework.PatchService.Runtime.Core
{
    public class VersionMeta
    {
        public int version;
        public long size;
        public int count;
    }

    public class SnkVersionInfos
    {
        /// <summary>
        /// 应用版本
        /// </summary>
        public string appVersion;

        /// <summary>
        /// 资源版本
        /// </summary>
        //public int resVersion;

        public string verificationCode;
        
        /// <summary>
        /// 历史版本
        /// </summary>
        public List<VersionMeta> histories;
    }
}