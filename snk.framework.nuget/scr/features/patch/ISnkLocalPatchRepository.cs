namespace SnkFramework.NuGet.Features
{
    namespace Patch
    {
        public interface ISnkLocalPatchRepository : ISnkPatchRepository
        {
            /// <summary>
            /// 本地仓库路径
            /// </summary>
            string LocalPath { get; }

            /// <summary>
            /// 更新本地版本号
            /// </summary>
            /// <param name="resVersion">资源版本</param>
            void UpdateLocalResVersion(int resVersion);
        }
    }
}