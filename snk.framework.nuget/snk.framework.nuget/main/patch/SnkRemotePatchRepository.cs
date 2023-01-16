using System.Collections.Generic;
using System.Threading.Tasks;

namespace snk.framework.nuget
{
    namespace patch
    {
        public class SnkRemotePatchRepository : ISnkRemotePatchRepository
        {
            public int Version { get; }

            public Task Initialize()
            {
                throw new System.NotImplementedException();
            }

            public Task<List<SnkSourceInfo>> GetSourceInfoList(int version = -1)
            {
                throw new System.NotImplementedException();
            }

            public IEnumerable<SnkVersionMeta> GetResVersionHistories()
            {
                throw new System.NotImplementedException();
            }

            public Task TakeFileToLocal(string dirPath, string key, int resVersion)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}