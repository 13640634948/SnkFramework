using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SnkFramework.NuGet.Features
{
    namespace HttpWeb
    {
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
            /// 是否取消下载
            /// </summary>
            public bool isCancelDownload;
        }

        /// <summary>
        /// 下载任务
        /// </summary>
        public class SnkDownloadTask
        {
            /// <summary>
            /// 下载参数
            /// </summary>
            private DownloadParam _param;

            /// <summary>
            /// 取消下载令牌
            /// </summary>
            private CancellationTokenSource _cts;

            /// <summary>
            /// 结果
            /// </summary>
            private SnkHttpDownloadResult _result;

            /// <summary>
            /// 构造方法
            /// </summary>
            /// <param name="param"></param>
            public SnkDownloadTask(DownloadParam param)
            {
                _result = new SnkHttpDownloadResult();
                _cts = new CancellationTokenSource();
                this._param = param;
            }

            /// <summary>
            /// 下载
            /// </summary>
            /// <returns></returns>
            public async Task<SnkHttpDownloadResult> DownloadFile()
            {
                var curPosition = 0L;
                FileStream fileStream = null;
                try
                {
                    var req = WebRequest.CreateHttp(_param.uri);
                    var fileInfo = new FileInfo(_param.savePath);

                    if (_param.downloadFormBreakpoint)
                    {
                        if (fileInfo.Exists)
                        {
                            fileStream = File.Open(_param.savePath, FileMode.Open, FileAccess.ReadWrite);
                            curPosition = fileStream.Length;
                            fileStream.Seek(curPosition, SeekOrigin.Current);
                        }
                        else
                        {
                            fileStream = new FileStream(_param.savePath, FileMode.Create, FileAccess.ReadWrite);
                        }
                    }
                    else
                    {
                        if (fileInfo.Exists)
                        {
                            fileInfo.Delete();
                        }
                        fileStream = new FileStream(_param.savePath, FileMode.Create, FileAccess.ReadWrite);
                    }

                    if (curPosition > 0)
                        req.AddRange(curPosition);
                    req.Method = WebRequestMethods.Http.Get;
                    var rsp = await req.GetResponseAsync() as HttpWebResponse;
                    if (_cts.IsCancellationRequested)
                    {
                        _result.isCancelDownload = true;
                    }
                    if (rsp == null)
                    {
                        return _result.SetError("返回rsp为空") as SnkHttpDownloadResult;
                    }
                    _result.code = rsp.StatusCode;
                    if (rsp.StatusCode != HttpStatusCode.OK && rsp.StatusCode != HttpStatusCode.PartialContent)
                    {
                        return _result.SetError($"Http状态码异常,错误码为:{rsp.StatusCode}") as SnkHttpDownloadResult;
                    }

                    var rspStream = rsp.GetResponseStream();
                    if (rspStream == null)
                    {
                        return _result.SetError("返回rspStream为空") as SnkHttpDownloadResult;
                    }

                    var lengthContent = rsp.Headers.Get("Content-Length");
                    var parseLengthResult = long.TryParse(lengthContent, out var length);
                    if (!parseLengthResult)
                    {
                        return _result.SetError("无法解析Content-Length") as SnkHttpDownloadResult;
                    }
                    if (fileStream.Length >= length)
                    {
                        return _result.SetError("本地文件长度大于等于下载长度，请检查") as SnkHttpDownloadResult;
                    }

                    var avg = 1024;
                    var size = 0;
                    var buffer = new byte[avg];
                    size = rspStream.Read(buffer, 0, avg);
                    while (size > 0)
                    {
                        if (_cts.IsCancellationRequested)
                        {
                            _result.isCancelDownload = true;
                            break;
                        }
                        fileStream?.Write(buffer, 0, size);
                        size = rspStream.Read(buffer, 0, avg);
                        _param.progressCallback?.Invoke(fileStream.Length, length);
                    }
                    _result.downloadedFileSize = fileStream.Length;
                }
                catch (Exception e)
                {
                    var errorMsg = string.Format("下载出现异常\n下载地址:{0}\n错误信息:{1}\n堆栈:{2}", _param.uri, e.Message, e.StackTrace);
                    SnkNuget.Logger?.Error(errorMsg);
                    _result.SetError(errorMsg);
                }
                finally
                {
                    fileStream?.Dispose();
                    _result.isDone = true;
                }

                return _result;
            }


            /// <summary>
            /// 取消下载
            /// </summary>
            public void CancelDownload()
            {
                _cts?.Cancel();
            }

        }
    }
}