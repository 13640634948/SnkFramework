namespace snk.framework.nuget
{
    namespace patch
    {
        /// <summary>
        /// 补丁源(PatchSource)
        /// </summary>
        public struct SnkSourceInfo
        {
            /// <summary>
            /// 源文件键值
            /// </summary>
            public string key;

            /// <summary>
            /// 源文件版本
            /// </summary>
            public string version;

            /// <summary>
            /// 源文件大小
            /// </summary>
            public long size;

            /// <summary>
            /// 源文件验证值
            /// </summary>
            public string code;
        }

    }
}