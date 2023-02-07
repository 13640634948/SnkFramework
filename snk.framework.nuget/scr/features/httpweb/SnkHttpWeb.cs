using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SnkFramework.NuGet.Features.HttpWeb
{
    /// <summary>
    /// web请求..
    /// </summary>
    public class SnkHttpWeb
    {
        /// <summary>
        /// head方式请求
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static async Task<SnkHttpHeadResult> Head(string uri, int timeout = 0)
        {
            /// <summary>
            /// HttpGet
            /// </summary>
            /// <param name="uri"></param>
            /// <returns></returns>
            public static async Task<(bool, string)> HttpGet(string uri)
            {
                HttpWebRequest req = WebRequest.CreateHttp(uri);
                if (timeout > 0)
                    req.Timeout = timeout;
                req.Method = WebRequestMethods.Http.Head;
                WebResponse webResponse = await req.GetResponseAsync();
                HttpWebResponse rsp = webResponse as HttpWebResponse;
                webResponse = (WebResponse)null;
                if (rsp == null)
                    return result.SetError("无法获取到rsp") as SnkHttpHeadResult;
                result.code = rsp.StatusCode;
                if (rsp.StatusCode != HttpStatusCode.OK)
                    return result.SetError("收到200以外的错误码") as SnkHttpHeadResult;
                result.code = rsp.StatusCode;
                string lengthContent = rsp.Headers.Get("Content-Length");
                long length;
                bool parseLengthResult = long.TryParse(lengthContent, out length);
                if (!parseLengthResult)
                    return result.SetError("无法解析Content-Length") as SnkHttpHeadResult;
                result.length = length;
            }
            catch (Exception ex)
            {
                result.SetError("Get出现异常\n异常信息:" + ex.Message + "\n异常堆栈: " + ex.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// Get方式请求
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static async Task<SnkHttpGetResult> Get(string uri, int timeout = 0)
        {
            SnkHttpGetResult result = new SnkHttpGetResult();
            try
            {
                HttpWebRequest req = WebRequest.CreateHttp(uri);
                if (timeout > 0)
                    req.Timeout = timeout;
                req.Method = WebRequestMethods.Http.Get;
                WebResponse webResponse = await req.GetResponseAsync();
                HttpWebResponse rsp = webResponse as HttpWebResponse;
                webResponse = (WebResponse)null;
                if (rsp == null)
                    return result.SetError("无法获取到rsp") as SnkHttpGetResult;
                result.code = rsp.StatusCode;
                if (rsp.StatusCode != HttpStatusCode.OK)
                    return result.SetError("收到200以外的错误码") as SnkHttpGetResult;
                
                using (Stream rspStream = rsp.GetResponseStream())
                {
                    var length = rsp.ContentLength;
                    using (BufferedStream bs = new BufferedStream(rspStream))
                    {
                        result.data = new byte[length];
                        await bs.ReadAsync(result.data, 0, result.data.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                result.SetError("Get出现异常\n异常信息:" + ex.Message + "\n异常堆栈: " + ex.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// Post方式请求
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="headDict"></param>
        /// <param name="content"></param>
        /// <param name="timeout"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static async Task<SnkHttpPostResult> Post(string uri, Dictionary<string, string> headDict, byte[] content, int timeout = 0)
        {
            SnkHttpPostResult result = new SnkHttpPostResult();
            try
            {
                HttpWebRequest req = WebRequest.CreateHttp(uri);
                if (timeout > 0)
                    req.Timeout = timeout;
                req.Method = WebRequestMethods.Http.Post;
                if (headDict != null && headDict.Count > 0)
                {
                    foreach (var kv in headDict)
                        req.Headers.Add(kv.Key, kv.Value);
                }
                if (content != null && content.Length > 0)
                {
                    req.ContentLength = content.Length;
                    using (var reqStream = req.GetRequestStream())
                    {
                        reqStream.Write(content, 0, content.Length);
                    }
                }

                WebResponse webResponse = await req.GetResponseAsync();
                HttpWebResponse rsp = webResponse as HttpWebResponse;
                webResponse = (WebResponse)null;
                if (rsp == null)
                {
                    return result.SetError("无法获取到rsp") as SnkHttpPostResult;
                }
                result.code = rsp.StatusCode;
                if (rsp.StatusCode != HttpStatusCode.OK)
                {
                    return result.SetError("收到200以外的错误码") as SnkHttpPostResult;
                }
                using (Stream rspStream = rsp.GetResponseStream())
                {
                    if (rspStream == null)
                    {
                        return result.SetError("无法获取到响应流") as SnkHttpPostResult;
                    }
                    var allHeadKeys = rsp.Headers.AllKeys;
                    if (allHeadKeys.Length > 0)
                    {
                        result.rspHeaderDict = new Dictionary<string, string>();
                        for (var i = 0; i < allHeadKeys.Length; i++)
                        {
                            var key = allHeadKeys[i];
                            var value = rsp.Headers[key];
                            result.rspHeaderDict.Add(key, value);
                        }
                    }
                    var length = rsp.ContentLength;
                    using (BufferedStream bs = new BufferedStream(rspStream))
                    {
                        result.data = new byte[length];
                        await bs.ReadAsync(result.data, 0, (int)rspStream.Length);
                    }
                }
                req = (HttpWebRequest)null;
                rsp = (HttpWebResponse)null;
            }
            catch (Exception ex)
            {
                result.SetError("Get出现异常\n异常信息:" + ex.Message + "\n异常堆栈: " + ex.StackTrace);
            }
            return result;
        }
    }
}
