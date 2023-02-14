using System.Collections.Generic;
using System.Net;

namespace SnkFramework.NuGet.Features
{
    namespace HttpWeb
    {
        /// <summary>
        /// http结果
        /// </summary>
        public class SnkHttpResult
        {
            /// <summary>
            /// 是否错误
            /// </summary>
            public bool isError => code != SNK_HTTP_ERROR_CODE.succeed;

            /// <summary>
            /// 错误信息
            /// </summary>
            public string errorMessage = string.Empty;

            /// <summary>
            /// 是否完成
            /// </summary>
            public bool isDone = false;

            /// <summary>
            /// 异常信息
            /// </summary>
            public SNK_HTTP_ERROR_CODE code;

            /// <summary>
            /// http状态吗
            /// </summary>
            public HttpStatusCode httpStatusCode;
        }

        /// <summary>
        /// Head结果
        /// </summary>
        public class SnkHttpHeadResult : SnkHttpResult
        {
            /// <summary>
            /// 返回的长度
            /// </summary>
            public long length = -1;
        }

        /// <summary>
        /// Get结果
        /// </summary>
        public class SnkHttpGetResult : SnkHttpResult
        {
            /// <summary>
            /// Head字典
            /// </summary>
            public Dictionary<string, string> headDict;

            /// <summary>
            /// 返回的数据
            /// </summary>
            public byte[] data;
        }

        /// <summary>
        /// post结果
        /// </summary>
        public class SnkHttpPostResult : SnkHttpResult
        {
            /// <summary>
            /// Head字典
            /// </summary>
            public Dictionary<string, string> headDict;

            /// <summary>
            /// 返回内容
            /// </summary>
            public byte[] data;
        }

        /// <summary>
        /// 下载结果
        /// </summary>
        public class SnkHttpDownloadResult : SnkHttpResult
        {
            /// <summary>
            /// 已下载的大小
            /// </summary>
            public long downloadedFileSize;

            /// <summary>
            /// 是否是取消下载
            /// </summary>
            public bool isCancelDownload;
        }
    }
}