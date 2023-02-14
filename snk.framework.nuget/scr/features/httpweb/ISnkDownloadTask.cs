using System;
using System.Threading.Tasks;

namespace SnkFramework.NuGet.Features
{
    namespace HttpWeb
    {
        /// <summary>
        /// 下载任务接口
        /// </summary>
        public interface ISnkDownloadTask
        {
            /// <summary>
            /// 下载地址
            /// </summary>
            string URL { get; }

            /// <summary>
            /// 保存地址
            /// </summary>
            string SavePath { get; }

            /// <summary>
            /// 是否下载中
            /// </summary>
            /// <returns></returns>
            bool IsDownloading { get; }

            /// <summary>
            /// 文件总大小
            /// </summary>
            /// <returns></returns>
            long TotalSize { get; }

            /// <summary>
            /// 已下载大小
            /// </summary>
            /// <returns></returns>
            long DownloadedSize { get; }

            /// <summary>
            /// 是否断点续传
            /// </summary>
            bool DownloadFromBreakpoint { get; }

            /// <summary>
            /// 下载文件
            /// </summary>
            /// <param name="buffSize"></param>
            /// <returns></returns>
            Task<SnkHttpDownloadResult> DownloadFileAsync(int buffSize = 1024 * 4 * 10);

            /// <summary>
            /// 设置断点下载
            /// </summary>
            /// <param name="flag"></param>
            void SetDownloadFormBreakpoint(bool flag);

            /// <summary>
            /// 取消下载
            /// </summary>
            void CancelDownload();
        }
    }
}