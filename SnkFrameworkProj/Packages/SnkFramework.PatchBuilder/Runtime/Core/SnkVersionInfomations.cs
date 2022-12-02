using System.Collections.Generic;

namespace SnkFramework.PatchBuilder.Runtime.Core
{
    public class SnkVersionInfos
    {
        /// <summary>
        /// 应用版本
        /// </summary>
        public int appVersion;

        /// <summary>
        /// 资源版本
        /// </summary>
        public int resVersion;

        /// <summary>
        /// 历史版本
        /// </summary>
        public List<int> histories;
    }
}