using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnkFramework.NuGet.Features
{
    namespace Patch
    {
        public interface ISnkPatchRepository
        {
            /// <summary>
            /// 版本号
            /// </summary>
            int Version { get; }

            /// <summary>
            /// 仓库初始化
            /// </summary>
            /// <param name="patchController">补丁控制器</param>
            /// <returns>任务</returns>
            Task Initialize(ISnkPatchController patchController);

            /// <summary>
            /// 获取资源信息列表
            /// </summary>
            /// <param name="version">版本号</param>
            /// <returns>资源信息列表</returns>
            Task<List<SnkSourceInfo>> GetSourceInfoList(int version = -1);
        }
    }
}