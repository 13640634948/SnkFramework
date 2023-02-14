using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SnkFramework.NuGet.Features.HttpWeb;

namespace SnkFramework.NuGet.Features
{
    namespace HttpWeb
    {
        /// <summary>
        /// 下载执行器
        /// </summary>
        public class SnkHttpDownloadImplementer
        {
            /// <summary>
            /// 任务工厂
            /// </summary>
            private static TaskFactory s_taskFactory = null;
            private static TaskFactory s_TaskFactory => s_taskFactory ?? new TaskFactory();

            private static HttpClient _httpClient = new HttpClient();

            /// <summary>
            /// 使下载任务生效
            /// </summary>
            /// <param name="task"></param>
            public static async Task<SnkHttpDownloadResult> Implement(SnkDownloadTask task)
            {
                return await s_TaskFactory.StartNew(() => task.DownloadFileAsync().Result);
            }

            /// <summary>
            /// 创建下载任务
            /// </summary>
            /// <param name="uri"></param>
            /// <param name="savePath"></param>
            /// <returns></returns>
            public static SnkDownloadTask CreateDownloadTask(string uri,string savePath)
            {
                var downloadTask = new SnkDownloadTask(uri,savePath, _httpClient);
                return downloadTask;
            }
        }
    }
}
