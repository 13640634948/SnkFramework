using System;
using System.Collections.Generic;
using System.Net;

namespace SnkFramework.NuGet.Features
{
    namespace HttpWeb
    {
        /// <summary>
        /// Http结果
        /// </summary>
        public class SnkHttpResult
        {
            /// <summary>
            /// Http状态码
            /// </summary>
            public HttpStatusCode code;

            /// <summary>
            /// 是否完成
            /// </summary>
            public bool isDone;

            /// <summary>
            /// 是否出错
            /// </summary>
            public bool isError;

            /// <summary>
            /// 错误信息
            /// </summary>
            public string errorMessage;

            /// <summary>
            /// 设置错误
            /// </summary>
            /// <param name="result"></param>
            /// <param name="errMsg"></param>
            /// <returns></returns>
            public SnkHttpResult SetError(string errMsg)
            {
                this.errorMessage = errMsg;
                this.isError = true;
                this.isDone = true;
                return this;
            }
        }

        /// <summary>
        /// Get结果
        /// </summary>
        public class SnkHttpGetResult : SnkHttpResult
        {
            public byte[] data;
        }

        /// <summary>
        /// Head结果
        /// </summary>
        public class SnkHttpHeadResult : SnkHttpResult
        {
            public long length;
        }

        /// <summary>
        /// post结果
        /// </summary>
        public class SnkHttpPostResult : SnkHttpResult
        {
            public Dictionary<string, string> rspHeaderDict;
            public byte[] data;
        }
    }
}