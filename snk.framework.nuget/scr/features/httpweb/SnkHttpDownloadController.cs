using System.Net.Http;
using System.Threading.Tasks;

namespace SnkFramework.NuGet.Features
{
    namespace HttpWeb
    {
        /// <summary>
        /// 下载控制器
        /// </summary>
        public class SnkHttpDownloadController
        {
            /// <summary>
            /// 任务工厂（私有）
            /// </summary>
            private static TaskFactory s_taskFactory = null;

            /// <summary>
            /// 任务工厂
            /// </summary>
            private static TaskFactory s_TaskFactory => s_taskFactory ?? new TaskFactory();

            /// <summary>
            /// http客户端
            /// </summary>
            private static HttpClient _httpClient = new HttpClient();
  
            /// <summary>
            /// 使下载任务生效
            /// </summary>
            /// <param name="task"></param>
            public static void Implement(ISnkDownloadTask task)
                => s_TaskFactory.StartNew(() => task.DownloadFileAsync());

            /// <summary>
            /// 创建下载任务
            /// </summary>
            /// <param name="uri"></param>
            /// <param name="savePath"></param>
            /// <returns></returns>
            public static ISnkDownloadTask CreateDownloadTask(string uri,string savePath)
            {
                var downloadTask = new SnkDownloadTask(uri,savePath, _httpClient);
                return downloadTask;
            }
        }
    }
}
