using System;

namespace SnkFramework.NuGet.Features
{
    namespace HttpWeb
    {
        /// <summary>
        /// 下载参数
        /// </summary>
        public class SnkDownloadParams
        {
            /// <summary>
            /// 下载地址
            /// </summary>
            public string uri;

            /// <summary>
            /// 保存地址
            /// </summary>
            public string savePath;

            /// <summary>
            /// 下载中回调
            /// </summary>
            public Action<long, long> progressCallback;

            /// <summary>
            /// 断线续传
            /// </summary>
            public bool downloadFormBreakpoint;
        }
    }
}