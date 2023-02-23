using System.Collections.Generic;
using System.Threading.Tasks;
using SnkFramework.NuGet.Asynchronous;

namespace SnkFramework.NuGet.Features
{
    namespace Patch
    {
        public struct DownloadProgress
        {
            public float progress;
        }

        public interface ISnkRemotePatchRepository : ISnkPatchRepository
        {
            /// <summary>
            /// 资源版本历史
            /// </summary>
            /// <returns></returns>
            List<SnkVersionMeta> GetResVersionHistories();

            /// <summary>
            /// 获取远端文件保存至本地
            /// </summary>
            /// <param name="dirPath">本地目录</param>
            /// <param name="key">远端文件key</param>
            /// <param name="resVersion">对应资源版本号</param>
            /// <returns>任务</returns>
            //void TakeFileToLocal(string dirPath, string key, int resVersion);

            void EnqueueDownloadQueue(string dirPath, string key, int resVersion);

            void StartupDownload(ISnkProgressPromise<DownloadProgress> progressPromise);
        }
    }
}