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
            private static TaskFactory _taskFactory;

            private static HttpClient _httpClient = new HttpClient();

            /// <summary>
            /// 使下载任务生效
            /// </summary>
            /// <param name="task"></param>
            public static async Task<SnkHttpDownloadResult> Implement(SnkDownloadTask task)
            {
                if (_taskFactory == null)
                {
                    _taskFactory = new TaskFactory();
                }
                return await _taskFactory.StartNew(() => task.AsyncDownloadFile().Result);
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
