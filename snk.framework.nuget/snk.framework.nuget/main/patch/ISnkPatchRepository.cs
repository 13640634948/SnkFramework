using System.Collections.Generic;
using System.Threading.Tasks;

namespace snk.framework.nuget
{
    namespace patch
    {
        public interface ISnkPatchRepository
        {
            int Version { get; }
            Task Initialize();
            Task<List<SnkSourceInfo>> GetSourceInfoList(int version = -1);
        }
    }
}