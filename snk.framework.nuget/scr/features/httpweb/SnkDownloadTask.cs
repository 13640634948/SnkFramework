using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SnkFramework.NuGet.Features
{
    namespace HttpWeb
    {


        /// <summary>
        /// 下载任务
        /// </summary>
        public class SnkDownloadTask
        {
            /// <summary>
            /// 下载地址
            /// </summary>
            private string _uri;

            /// <summary>
            /// 保存地址
            /// </summary>
            private string _savePath;

            /// <summary>
            /// 断线续传
            /// </summary>
            private bool _downloadFormBreakpoint = false;

            /// <summary>
            /// 取消下载令牌
            /// </summary>
            private CancellationTokenSource _cts;

            /// <summary>
            /// 结果
            /// </summary>
            private SnkHttpDownloadResult _result;

            /// <summary>
            /// HttpClient
            /// </summary>
            private HttpClient _httpClient;

            /// <summary>
            /// 总大小
            /// </summary>
            private long _totalSize = -1;

            /// <summary>
            /// 已下载文件的大小
            /// </summary>
            private long _downloadedSize = -1;

            /// <summary>
            /// 是否下载中
            /// </summary>
            private bool _isDownloading = false;

            /// <summary>
            /// 是否下载中
            /// </summary>
            /// <returns></returns>
            public bool IsDownloading() => _isDownloading;

            /// <summary>
            /// 获取文件总大小
            /// </summary>
            /// <returns></returns>
            public long GetTotalSize() => _totalSize;

            /// <summary>
            /// 获取已下载大小
            /// </summary>
            /// <returns></returns>
            public long GetDownloadedSize() => _downloadedSize;

            /// <summary>
            /// 构造方法
            /// </summary>
            /// <param name="param"></param>
            public SnkDownloadTask(string uri, string savePath, HttpClient httpClient)
            {
                _result = new SnkHttpDownloadResult();
                _cts = new CancellationTokenSource();
                this._uri = uri;
                this._savePath = savePath;
                this._httpClient = httpClient;
            }

            /// <summary>
            /// 设置断点续传
            /// </summary>
            /// <param name="flag"></param>
            public void SetDownloadFormBreakpoint(bool flag)
            {
                this._downloadFormBreakpoint = flag;
            }

            /// <summary>
            /// 异步下载
            /// </summary>
            /// <returns></returns>
            public async Task<SnkHttpDownloadResult> AsyncDownloadFile()
            {
                var curPosition = 0L;
                FileStream fileStream = null;
                try
                {
                    do
                    {
                        var fileInfo = new FileInfo(_savePath);
                        if (!fileInfo.Directory.Exists)
                        {
                            fileInfo.Directory.Create();
                        }
                        if (_downloadFormBreakpoint)
                        {
                            if (fileInfo.Exists)
                            {
                                fileStream = File.Open(_savePath, FileMode.Open, FileAccess.ReadWrite);
                                curPosition = fileStream.Length;
                                fileStream.Seek(curPosition, SeekOrigin.Current);
                            }
                            else
                            {
                                fileStream = new FileStream(_savePath, FileMode.Create, FileAccess.ReadWrite);
                            }
                        }
                        else
                        {
                            if (fileInfo.Exists)
                            {
                                fileInfo.Delete();
                            }
                            fileStream = new FileStream(_savePath, FileMode.Create, FileAccess.ReadWrite);
                        }
                        var request = new HttpRequestMessage();
                        request.RequestUri = new Uri(_uri);
                        request.Method = HttpMethod.Get;
                        request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(curPosition, null);
                        using (var rsp = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false))
                        {
                            _result.httpStatusCode = rsp.StatusCode;
                            rsp.EnsureSuccessStatusCode();
                            _totalSize = (long)rsp.Content.Headers.ContentRange.Length;//文件总大小
                            if (_totalSize == 0)
                            {
                                _result.errorMessage = string.Format("本地文件长度大于等于总文件长度，请检查\n本地文件长度:{0}\n远端文件长度:{1}\n下载地址:{2}", fileStream.Length, _totalSize, _uri);
                                _result.code = SNK_HTTP_ERROR_CODE.target_file_size_is_zero;
                                break;
                            }
                            if (fileStream.Length >= _totalSize)
                            {
                                _result.errorMessage = string.Format("本地文件长度大于等于总文件长度，请检查\n本地文件长度:{0}\n远端文件长度:{1}\n下载地址:{2}", fileStream.Length, _totalSize, _uri);
                                _result.code = SNK_HTTP_ERROR_CODE.file_error;
                                break;
                            }
                            using (var rspStream = await rsp.Content.ReadAsStreamAsync().ConfigureAwait(false))
                            {
                                if (rspStream == null)
                                {
                                    _result.errorMessage = string.Format("下载出现异常,无法获取rsp流\n下载地址:{0}", _uri);
                                    _result.code = SNK_HTTP_ERROR_CODE.download_error;
                                    break;
                                }
                                var avg = 40960;
                                var size = 0;
                                var buffer = new byte[avg];
                                size = rspStream.Read(buffer, 0, avg);
                                _isDownloading = true;
                                while (size > 0)
                                {
                                    if (_cts.IsCancellationRequested)
                                    {
                                        _result.isCancelDownload = true;
                                        break;
                                    }
                                    fileStream.Write(buffer, 0, size);
                                    size = rspStream.Read(buffer, 0, avg);
                                    fileStream.Flush(true);
                                    _downloadedSize = fileStream.Length;
                                }
                            }
                        }
                    }
                    while (false);
                }
                catch (Exception e)
                {
                    var errorMsg = string.Format("下载出现异常\n下载地址:{0}\n错误信息:{1}\n堆栈:{2}", _uri, e.Message, e.StackTrace);
                    _result.errorMessage = errorMsg;
                    _result.code = SNK_HTTP_ERROR_CODE.download_error;
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