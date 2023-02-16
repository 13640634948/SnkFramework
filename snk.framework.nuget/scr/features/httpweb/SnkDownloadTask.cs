using System;
using System.IO;
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
        internal class SnkDownloadTask : ISnkDownloadTask
        {
            public class DownloadProgress
            {
                private long _downloadedBytes;
                private long _totalBytes;

                public int percentage { get; protected set; }

                public long downloadedBytes
                {
                    get => _downloadedBytes;
                    set
                    {
                        _downloadedBytes = value;
                        this.refresh();
                    }
                }

                public long totalBytes
                {
                    get => _totalBytes;
                    set
                    {
                        _totalBytes = value;
                        this.refresh();
                    }
                }

                private void refresh()
                {
                    if (_downloadedBytes <= 0 || _totalBytes <= 0)
                    {
                        percentage = 0;
                        return;
                    }
                    percentage = (int)((_downloadedBytes * 100) / _totalBytes);
                }
            }

            /// <summary>
            /// 下载地址（私有）
            /// </summary>
            public string _url;

            /// <summary>
            /// 下载地址
            /// </summary>
            public string URL => _url;

            /// <summary>
            /// 保存地址（私有）
            /// </summary>
            private string _savePath;

            /// <summary>
            /// 保存地址
            /// </summary>
            public string SavePath => _savePath;

            /// <summary>
            /// 是否下载中（私有）
            /// </summary>
            private bool _isDownloading;

            /// <summary>
            /// 是否下载中
            /// </summary>
            /// <returns></returns>
            public bool IsDownloading => _isDownloading;

            /// <summary>
            /// 文件总大小（私有）
            /// </summary>
            private long _totalSize = -1;

            /// <summary>
            /// 文件总大小
            /// </summary>
            /// <returns></returns>
            public long TotalSize => _totalSize;

            /// <summary>
            /// 已下载大小（私有）
            /// </summary>
            private long _downloadedSize = -1;

            /// <summary>
            /// 已下载大小
            /// </summary>
            /// <returns></returns>
            public long DownloadedSize => _downloadedSize;

            /// <summary>
            /// 是否断点续传（私有）
            /// </summary>
            private bool _downloadFromBreakpoint;

            /// <summary>
            /// 是否断点续传
            /// </summary>
            public bool DownloadFromBreakpoint => _downloadFromBreakpoint;

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
            /// 构造方法
            /// </summary>
            /// <param name="url"></param>
            /// <param name="savePath"></param>
            /// <param name="httpClient"></param>
            public SnkDownloadTask(string url, string savePath, HttpClient httpClient)
            {
                _result = new SnkHttpDownloadResult();
                _cts = new CancellationTokenSource();
                this._url = url;
                this._savePath = savePath;
                this._httpClient = httpClient;
            }

            /// <summary>
            /// 设置断点续传
            /// </summary>
            /// <param name="flag"></param>
            public void SetDownloadFormBreakpoint(bool flag)
            {
                this._downloadFromBreakpoint = flag;
            }

            /// <summary>
            /// 异步下载
            /// </summary>
            /// <returns></returns>
            public async Task<SnkHttpDownloadResult> DownloadFileAsync(int buffSize = 1024 * 4 * 10)
            {
                try
                {
                    using (var request = new HttpRequestMessage())
                    {
                        request.RequestUri = new Uri(_url);
                        request.Method = HttpMethod.Get;
                        var fileInfo = new FileInfo(SavePath);
                        if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
                            fileInfo.Directory.Create();
                        FileStream fileStream = null;
                        if (fileInfo.Exists)
                        {
                            if (_downloadFromBreakpoint)
                            {
                                fileStream = File.Open(SavePath, FileMode.Open, FileAccess.ReadWrite);
                                fileStream.Seek(0, SeekOrigin.End);
                            }
                            else
                            {
                                fileInfo.Delete();
                            }
                        }

                        using (fileStream =
                                   fileStream ?? new FileStream(SavePath, FileMode.Create, FileAccess.ReadWrite))
                        {  
                            request.Headers.Range =
                                new System.Net.Http.Headers.RangeHeaderValue(fileStream.Position, null);
                            using (var rsp = await _httpClient
                                       .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                                       .ConfigureAwait(false))
                            {
                                _result.httpStatusCode = rsp.StatusCode;
                                rsp.EnsureSuccessStatusCode();
                                _totalSize = rsp.Content.Headers.ContentRange.Length ?? 0;
                                if (fileStream.Length >= _totalSize)
                                {
                                    _result.errorMessage =
                                        $"本地文件长度大于等于总文件长度，请检查\n本地文件长度:{fileStream.Length}\n远端文件长度:{_totalSize}\n下载地址:{_url}";
                                    _result.code = SNK_HTTP_ERROR_CODE.file_error;
                                    _result.isDone = true;
                                    return _result;
                                }

                                using (var rspStream = await rsp.Content.ReadAsStreamAsync().ConfigureAwait(false))
                                {
                                    if (rspStream == null)
                                    {
                                        _result.errorMessage = $"下载出现异常,无法获取rsp流\n下载地址:{_url}";
                                        _result.code = SNK_HTTP_ERROR_CODE.download_error;
                                        _result.isDone = true;
                                        return _result;
                                    }

                                    _isDownloading = true;
                                    var len = 0;
                                    var buffer = new byte[buffSize];
                                    while ((len = rspStream.Read(buffer, 0, buffSize)) > 0)
                                    {
                                        if (_cts.IsCancellationRequested)
                                        {
                                            _result.isCancelDownload = true;
                                            break;
                                        }

                                        fileStream.Write(buffer, 0, len);
                                        fileStream.Flush(true);
                                        _downloadedSize = fileStream.Length;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    _result.errorMessage = $"下载出现异常\n下载地址:{_url}\n错误信息:{e.Message}\n堆栈:{e.StackTrace}";
                    _result.code = SNK_HTTP_ERROR_CODE.download_error;
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