using System;
namespace SnkFramework.NuGet.Features
{
    namespace HttpWeb
    {
        public static class SnkDownloadTaskExtension
        {
            /// <summary>
            /// 获取下载进度
            /// </summary>
            /// <param name="downloadTask"></param>
            /// <returns></returns>
            public static float GetDownloadProgress(this ISnkDownloadTask downloadTask)
            {
                if (downloadTask == null)
                    return 0;
                if (downloadTask.DownloadedSize <= 0 || downloadTask.TotalSize <= 0)
                    return 0;
                return (float)downloadTask.DownloadedSize / downloadTask.TotalSize;
            }
        }
    }
}