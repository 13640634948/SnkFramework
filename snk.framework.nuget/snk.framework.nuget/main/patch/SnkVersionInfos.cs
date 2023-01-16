using System.Collections.Generic;

namespace snk.framework.nuget
{
    namespace patch
    {
        public class SnkVersionInfos
        {
            /// <summary>
            /// 应用版本
            /// </summary>
            public string appVersion;

            /// <summary>
            /// 历史版本
            /// </summary>
            public List<SnkVersionMeta> histories;
        }
    }
}