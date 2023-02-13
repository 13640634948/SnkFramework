using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SnkFramework.NuGet.Features
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
            target_file_size_is_zero,
        }

        /// <summary>
        /// web请求
        /// </summary>
        public class SnkHttpWeb
        {
            /// <summary>
            /// 任务工厂（私有）
            /// </summary>
            private static TaskFactory _taskFactory;

            /// <summary>
            /// 任务工厂
            /// </summary>
            protected static TaskFactory taskFactory
            {
                get
                {
                    if (_taskFactory == null)
                    {
                        _taskFactory = new TaskFactory();
                    }
                    return _taskFactory;
                }
            }

            /// <summary>
            /// Head方法
            /// </summary>
            /// <param name="uri"></param>
            /// <param name="timeout"></param>
            /// <returns></returns>
            public static async Task<SnkHttpHeadResult> Head(string uri, TimeSpan timeout = default)
            {
                return await taskFactory.StartNew(() =>
                {
                    var result = new SnkHttpHeadResult();
                    try
                    {
                        var httpClient = new HttpClient();
                        if (timeout != default)
                        {
                            httpClient.Timeout = timeout;
                        }
                        using (var hrm = new HttpRequestMessage(HttpMethod.Head, uri))
                        {
                            using (var rsp = httpClient.SendAsync(hrm).Result)
                            {
                                result.httpStatusCode = rsp.StatusCode;
                                rsp.EnsureSuccessStatusCode();
                                if (rsp.Content != null && rsp.Content.Headers != null)
                                {
                                    result.length = (long)rsp.Content.Headers.ContentLength;
                                }
                                else
                                {
                                    result.code = SNK_HTTP_ERROR_CODE.can_not_find_length;
                                    result.errorMessage = $"服务器没有返回content-length,无法获取文件长度\n访问地址:{uri}";
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        result.code = SNK_HTTP_ERROR_CODE.error;
                        result.errorMessage = $"请求{uri}出现异常\n异常信息:{e.Message}\n异常堆栈:{e.StackTrace}";
                    }
                    finally
                    {
                        result.isDone = true;
                    }
                    return result;
                });
            }

            /// <summary>
            /// Get方法
            /// </summary>
            /// <param name="uri"></param>
            /// <param name="timeout"></param>
            /// <returns></returns>
            public static async Task<SnkHttpGetResult> Get(string uri, TimeSpan timeout = default)
            {
                return await taskFactory.StartNew(() =>
                {
                    var result = new SnkHttpGetResult();
                    try
                    {
                        var httpClient = new HttpClient();
                        if (timeout != default)
                        {
                            httpClient.Timeout = timeout;
                        }
                        using (var hrm = new HttpRequestMessage(HttpMethod.Get, uri))
                        {
                            using (var rsp = httpClient.SendAsync(hrm).Result)
                            {
                                result.httpStatusCode = rsp.StatusCode;
                                rsp.EnsureSuccessStatusCode();
                                rsp.Headers.GetEnumerator();
                                result.data = rsp.Content.ReadAsByteArrayAsync().Result;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        result.code = SNK_HTTP_ERROR_CODE.error;
                        result.errorMessage = $"请求{uri}出现异常\n异常信息:{e.Message}\n异常堆栈:{e.StackTrace}";
                    }
                    finally
                    {
                        result.isDone = true;
                    }
                    return result;
                });
            }

            /// <summary>
            /// Post方法
            /// </summary>
            /// <param name="uri"></param>
            /// <param name="headDict"></param>
            /// <param name="content"></param>
            /// <param name="timeout"></param>
            /// <returns></returns>
            public static async Task<SnkHttpPostResult> Post(string uri, Dictionary<string, string> headDict, byte[] content, TimeSpan timeout = default)
            {
                return await taskFactory.StartNew(() =>
                {
                    var result = new SnkHttpPostResult();
                    try
                    {
                        var httpClient = new HttpClient();
                        if (timeout != default)
                        {
                            httpClient.Timeout = timeout;
                        }
                        using (var hrm = new HttpRequestMessage(HttpMethod.Post, uri))
                        {
                            if (headDict != null && headDict.Count > 0)
                            {
                                httpClient.DefaultRequestHeaders.Clear();
                                foreach (var kv in headDict)
                                {
                                    hrm.Headers.Add(kv.Key, kv.Value);
                                }
                            }
                            if (content != null && content.Length > 0)
                            {
                                hrm.Content = new ByteArrayContent(content);
                            }
                            using (var rsp = httpClient.SendAsync(hrm).Result)
                            {
                                result.httpStatusCode = rsp.StatusCode;
                                rsp.EnsureSuccessStatusCode();
                                if (rsp.Headers != null)
                                {
                                    foreach (var kv in rsp.Headers)
                                    {
                                        if (result.headDict != null)
                                        {
                                            result.headDict = new Dictionary<string, string>();
                                        }
                                        result.headDict.Add(kv.Key, kv.Value.ToString());
                                    }
                                }
                                result.data = rsp.Content.ReadAsByteArrayAsync().Result;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        result.code = SNK_HTTP_ERROR_CODE.error;
                        result.errorMessage = $"请求{uri}出现异常\n异常信息:{e.Message}\n异常堆栈:{e.StackTrace}";
                    }
                    finally
                    {
                        result.isDone = true;
                    }
                    return result;
                });
            }
        }
    }
}