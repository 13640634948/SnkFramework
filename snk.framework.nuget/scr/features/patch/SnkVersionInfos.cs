using System.Collections.Generic;

namespace SnkFramework.NuGet
{
    namespace Patch
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