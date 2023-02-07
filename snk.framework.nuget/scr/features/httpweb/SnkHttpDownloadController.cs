using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using SnkFramework.NuGet.Features.HttpWeb;

namespace SnkFramework.NuGet.Features
{
    namespace HttpWeb
    {
        /// <summary>
        /// 下载执行器
        /// todo:1.总下载速度 2.总进度
        /// </summary>
        public class SnkHttpDownloadImplementer
        {
            /// <summary>
            /// 任务工厂
            /// </summary>
            private static TaskFactory _taskFactory;

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
                return await _taskFactory.StartNew(() => task.DownloadFile().Result);
            }
        }
    }
}
