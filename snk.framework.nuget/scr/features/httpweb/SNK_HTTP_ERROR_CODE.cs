﻿namespace SnkFramework.NuGet.Features
{
    namespace HttpWeb
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public enum SNK_HTTP_ERROR_CODE
        {
            /// <summary>
            /// 成功
            /// </summary>
            succeed = 0,

            /// <summary>
            /// 错误
            /// </summary>
            error = 1,

            /// <summary>
            /// 无法找到文件长度
            /// </summary>
            can_not_find_length = 2,

            /// <summary>
            /// 下载异常
            /// </summary>
            download_error = 3,

            /// <summary>
            /// 文件错误
            /// </summary>
            file_error = 4,

            /// <summary>
            /// 目标文件大小为0
            /// </summary>
            target_file_size_is_zero = 5,
        }
    }
}